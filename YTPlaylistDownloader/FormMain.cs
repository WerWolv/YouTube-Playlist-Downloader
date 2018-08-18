using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MediaToolkit;
using MediaToolkit.Model;
using YoutubeExplode;

namespace YTPlaylistDownloader
{
    public partial class FormMain : Form
    {
        private YoutubeClient youtube = new YoutubeClient();
        public YouTubeService ytService;
        public List<PlaylistItem> playlistItems = new List<PlaylistItem>();

        public List<string> playlistNames = new List<string>();
        public Dictionary<int, MusicTags> musicTags = new Dictionary<int, MusicTags>();

        public string saveFolderDefault = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public bool canceled = false;
        public CancellationTokenSource cancelToken = new CancellationTokenSource();

        public IProgress<double> downloadProgress;

        public FormInfos formInfos;

        public FolderBrowserDialog OpenFolderDialog = new FolderBrowserDialog();

        public YoutubeClient Youtube { get => this.youtube; set => this.youtube = value; }

        public class MusicTags
        {
            public string artist;
            public string album;
            public string year;
            public string genre;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public FormMain()
        {
            InitializeComponent();

            listQuality.SelectedIndex = 0;
            lblStatus.Text = "";

            if (Properties.Settings.Default.downloadPath == "")
            {
                Properties.Settings.Default.downloadPath = saveFolderDefault;
                Properties.Settings.Default.Save();
            }

            this.txtDownloadPath.Text = Properties.Settings.Default.downloadPath;

            btnSearch.Click += async (s, e) =>
            {
                await login();

                if (txtURL.Text.ToLower().Contains("youtube.com/playlist"))
                {
                    var playListResult = await searchPlaylist(txtURL.Text.Split('&')[0].Split('=')[1]);

                    foreach (var video in playListResult)
                    {
                        playlistItems.Add(video);
                        listVideos.Items.Add(video.Snippet.Title);
                    }

                }
                else if (txtURL.Text.ToLower().Contains("youtube.com/watch"))
                {
                    var videoResult = await getVideo(txtURL.Text.Split('&')[0].Split('=')[1]);
                    listVideos.Items.Add(videoResult.Snippet.Title);

                    PlaylistItem video = new PlaylistItem();
                    video.ContentDetails = new PlaylistItemContentDetails();
                    video.Snippet = new PlaylistItemSnippet();
                    video.Snippet.Thumbnails = new ThumbnailDetails();

                    video.ContentDetails.VideoId = videoResult.Id;
                    video.Snippet.Title = videoResult.Snippet.Title;
                    video.Snippet.Thumbnails.High = videoResult.Snippet.Thumbnails.High;


                    playlistItems.Add(video);
                }
                else
                {
                    this.lblStatus.Text = "Invalid URL!";
                    tmrStatusReset.Start();
                }

                if (playlistItems.Count > 0)
                    btnClear.Enabled = true;

                txtURL.Text = "";

            };

            btnDownload.Click += (s, e) =>
            {
                this.btnDownload.Enabled = false;
                this.btnSearch.Enabled = false;
                this.boxConvert.Enabled = false;
                this.txtURL.Enabled = false;
                this.listQuality.Enabled = false;
                this.listVideos.Enabled = false;
                this.btnChoose.Enabled = false;
                this.txtDownloadPath.Enabled = false;
                this.btnClear.Enabled = false;
                this.btnCancel.Enabled = true;

                this.lblStatus.Text = "Downloading...";


                var selectedIndices = new List<int>();
                foreach (var index in listVideos.SelectedIndices)
                    selectedIndices.Add((int) index);

                listVideos.Items.Clear();
                foreach (var video in playlistItems)
                    listVideos.Items.Add(video.Snippet.Title);

                foreach (var index in listVideos.SelectedIndices)
                    listVideos.SetSelected((int) index, true);

                Thread thread = null;

                downloadProgress = new Progress<double>(value => pBarDownload.Value = (int) (value * 100));

                thread = new Thread(async () =>
                {
                    float doneVideos = 0;
                    foreach (var item in selectedIndices)
                    {
                        Console.WriteLine(playlistItems[item].ContentDetails.EndAt);
                        this.Invoke(new Action(() => imgThumbnail.ImageLocation =
                            playlistItems[item].Snippet.Thumbnails.High.Url));
                        await downloadVideo(playlistItems[item].ContentDetails.VideoId, item);

                        if (canceled) break;

                        this.Invoke(new Action(() => listVideos.Items[item] = "✓ " + listVideos.Items[item]));
                        doneVideos++;
                        this.Invoke(new Action(() => pBarVideos.Value =
                            (int) Math.Min((doneVideos / selectedIndices.Count) * 100, 100)));
                        Thread.Sleep(500);
                    }

                    Thread.Sleep(2000);

                    this.Invoke(new Action(() =>
                    {
                        SendMessage(pBarDownload.Handle, 1040, (IntPtr) 1, IntPtr.Zero);
                        pBarDownload.Value = 0;
                        File.Delete("tmp.mp4");

                        this.lblStatus.Text = "";
                        this.imgThumbnail.ImageLocation = "";

                        this.canceled = false;

                        this.btnSearch.Enabled = true;
                        this.boxConvert.Enabled = true;
                        this.listVideos.Enabled = true;
                        this.txtURL.Enabled = true;
                        this.listQuality.Enabled = true;
                        this.btnChoose.Enabled = true;
                        this.txtDownloadPath.Enabled = true;
                        this.btnClear.Enabled = true;
                        this.btnCancel.Enabled = false;

                        pBarVideos.Value = 0;
                        pBarDownload.Value = 0;

                        thread.Join();
                    }));
                });
                thread.Start();
            };

            btnCancel.Click += (s, e) =>
            {
                this.canceled = true;
                this.cancelToken.Cancel();
                this.lblStatus.Text = "Canceling!";
            };

            btnClear.Click += (s, e) =>
            {
                this.playlistItems.Clear();
                this.playlistNames.Clear();
                this.musicTags.Clear();
                this.listVideos.Items.Clear();
                this.imgThumbnail.Image = null;
                this.btnClear.Enabled = false;
                this.btnDownload.Enabled = false;
            };

            tmrStatusReset.Tick += (s, e) =>
            {
                this.lblStatus.Text = "";
                this.tmrStatusReset.Stop();
            };

            listVideos.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.A)
                    for (int i = listVideos.Items.Count - 1; i >= 0; i--)
                        listVideos.SetSelected(i, true);

                if (e.KeyCode == Keys.Escape)
                    for (int i = listVideos.Items.Count - 1; i >= 0; i--)
                        listVideos.SetSelected(i, false);

                if (e.KeyCode == Keys.Delete)
                {
                    var selectedItems = new List<string>();
                    foreach (var index in listVideos.SelectedIndices)
                        selectedItems.Add((string) listVideos.Items[(int) index]);

                    foreach (var item in selectedItems)
                    {
                        int index = listVideos.Items.IndexOf(item);
                        playlistItems.RemoveAt(index);
                        playlistNames.RemoveAt(index);
                        listVideos.Items.RemoveAt(index);
                    }
                }
            };

            listVideos.SelectedIndexChanged += (s, e) =>
            {
                if (listVideos.SelectedIndex == -1 || listVideos.SelectedIndex >= playlistItems.Count)
                {
                    imgThumbnail.ImageLocation = "";
                    btnDownload.Enabled = false;
                    return;
                }

                imgThumbnail.ImageLocation = playlistItems[listVideos.SelectedIndex].Snippet.Thumbnails.High.Url;
                btnDownload.Enabled = true;
            };

            listVideos.MouseClick += (s, e) =>
            {
                if (listVideos.IndexFromPoint(e.Location) >= 0)
                {
                    imgThumbnail.ImageLocation = playlistItems[listVideos.IndexFromPoint(e.Location)].Snippet.Thumbnails
                        .High.Url;

                    btnDownload.Enabled = true;
                }
            };

            txtURL.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    btnSearch.PerformClick();
            };

            listVideos.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.A && e.Control)
                {
                    for (int i = 0; i < listVideos.Items.Count; i++)
                        listVideos.SetSelected(i, true);
                    e.SuppressKeyPress = true;
                }
                else e.Handled = true;
            };

            txtDownloadPath.Leave += (s, e) =>
            {
                Properties.Settings.Default.downloadPath = txtDownloadPath.Text;
                Properties.Settings.Default.Save();
            };

            btnChoose.Click += (s, e) =>
            {
                if (OpenFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.downloadPath = OpenFolderDialog.SelectedPath;
                    Properties.Settings.Default.Save();

                    this.txtDownloadPath.Text = OpenFolderDialog.SelectedPath;
                }
            };

            txtURL.GotFocus += (s, e) => txtURL.Text = "";
            txtURL.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtURL.Text))
                    txtURL.Text = "Enter YouTube video or playlist URL here...";
            };

            boxConvert.CheckedChanged += (s, e) =>
            {
                this.listQuality.Enabled = !this.boxConvert.Checked;
                this.listQuality.SelectedIndex = 0;
            };
        }

        private async Task login()
        {
            UserCredential creds;

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                creds = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] {YouTubeService.Scope.Youtube},
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString()));
            }

            this.ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = this.GetType().ToString()
            });
        }



        private async Task<IList<PlaylistItem>> searchPlaylist(string playlistId)
        {
            PlaylistItemListResponse responseVideos;

            string nextPageToken = null;
            string lastPageToken = null;

            List<PlaylistItem> items = new List<PlaylistItem>();
            do
            {
                lastPageToken = nextPageToken;
                var pars = new Dictionary<String, String>();
                pars.Add("part", "snippet,contentDetails");
                pars.Add("maxResults", "50");
                pars.Add("playlistId", playlistId);
                
                if(!string.IsNullOrEmpty(nextPageToken))
                    pars.Add("pageToken", nextPageToken);

                PlaylistItemsResource.ListRequest listItemRequest = ytService.PlaylistItems.List(pars["part"]);

                if (pars.ContainsKey("maxResults"))
                    listItemRequest.MaxResults = Convert.ToInt32(pars["maxResults"]);

                if (pars.ContainsKey("playlistId") && pars["playlistId"] != "")
                    listItemRequest.PlaylistId = pars["playlistId"];

                if (pars.ContainsKey("pageToken") && pars["pageToken"] != "")
                    listItemRequest.PageToken = pars["pageToken"];

                responseVideos = await listItemRequest.ExecuteAsync();
                items.AddRange(responseVideos.Items);
                nextPageToken = responseVideos.NextPageToken;

                pars["part"] = "localizations,snippet";

                PlaylistsResource.ListRequest listRequest = ytService.Playlists.List(pars["part"]);
                listRequest.Id = playlistId;
                var responseName = await listRequest.ExecuteAsync();

                for (int i = 0; i < responseVideos.Items.Count; i++)
                    this.playlistNames.Add(responseName.Items[0].Snippet.Title);
            } while (!string.IsNullOrEmpty(nextPageToken));
            return items;
        }

        private async Task<Video> getVideo(string videoId)
        {
            var pars = new Dictionary<String, String>();
            pars.Add("part", "snippet,contentDetails");
            pars.Add("maxResults", "1");
            pars.Add("id", videoId);

            VideosResource.ListRequest listItemRequest = ytService.Videos.List(pars["part"]);

            if (pars.ContainsKey("maxResults"))
                listItemRequest.MaxResults = Convert.ToInt32(pars["maxResults"]);

            if (pars.ContainsKey("id") && pars["id"] != "")
                listItemRequest.Id = pars["id"];

            var responseVideos = await listItemRequest.ExecuteAsync();
            this.playlistNames.Add("");

            return responseVideos.Items[0];
        }

        private async Task downloadVideo(string videoId, int index)
        {
            var videoInfo = await Youtube.GetVideoMediaStreamInfosAsync(videoId);
            var video = await Youtube.GetVideoAsync(videoId);
            var streamInfo = videoInfo.Muxed.OrderBy(q => q.VideoQuality).Distinct().ToArray();
            var audioInfo = videoInfo.Audio.OrderBy(q => q.Bitrate).Distinct().ToArray();

            string downloadPath = Properties.Settings.Default.downloadPath.Replace('\\', '/');

            if (boxConvert.Checked)
            {
                if (!Directory.Exists(downloadPath + "/music"))
                    Directory.CreateDirectory(downloadPath + "/music");

                this.Invoke(new Action(() => SendMessage(pBarDownload.Handle, 1040, (IntPtr)1, IntPtr.Zero)));

                if(playlistNames[index] != "")
                    Directory.CreateDirectory($@"{downloadPath}/music/{playlistNames[index]}");

                try
                {
                    await Youtube.DownloadMediaStreamAsync(audioInfo[streamInfo.Length - 1], playlistNames[index] == "" ? $@"{downloadPath}/music/{cleanFileName(video.Title)}.mp3" : $@"{downloadPath}/music/{playlistNames[index]}/{cleanFileName(video.Title)}.mp3",
                            downloadProgress, cancelToken.Token);
                    /*await youtube.DownloadMediaStreamAsync(
                        streamInfo[streamInfo.Length - 1], "tmp.mp4",
                        downloadProgress, cancelToken.Token);*/
                }
                catch (TaskCanceledException)
                {

                }
           
                

                /*this.Invoke(new Action(() => SendMessage(pBarDownload.Handle, 1040, (IntPtr)3, IntPtr.Zero)));
                var inputFile = new MediaFile("tmp.mp4");
                var outputFile = playlistNames[index] == "" ? new MediaFile($@"{downloadPath}/music/{cleanFileName(video.Title)}.mp3") : new MediaFile($@"{downloadPath}/music/{playlistNames[index]}/{cleanFileName(video.Title)}.mp3");
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }*/
            }
            else
            {
                int selectedIndex = 0;

                if (!Directory.Exists(downloadPath + "/videos"))
                    Directory.CreateDirectory(downloadPath + "/videos");

                this.Invoke(new Action(() => SendMessage(pBarDownload.Handle, 1040, (IntPtr)1, IntPtr.Zero)));
                this.Invoke(new Action(() => selectedIndex = listQuality.SelectedIndex));

                Console.WriteLine($@"{downloadPath}/videos/{playlistNames[index]}");

                if (playlistNames[index] != "")
                    Directory.CreateDirectory($@"{downloadPath}/videos/{playlistNames[index]}");

                try
                {
                    await Youtube.DownloadMediaStreamAsync(
                        streamInfo[Math.Max(streamInfo.Length - 1 - selectedIndex, 0)],
                        playlistNames[index] == ""
                            ? $@"{downloadPath}/videos/{cleanFileName(video.Title)}.mp4"
                            : $@"{downloadPath}/videos/{playlistNames[index]}/{cleanFileName(video.Title)}.mp4",
                        downloadProgress, cancelToken.Token);
                }
                catch (TaskCanceledException)
                {
                    
                }
              
            }
        }

        private string cleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
