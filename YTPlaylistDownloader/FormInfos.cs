using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTPlaylistDownloader
{
    public partial class FormInfos : Form
    {
        public FormMain form;

        public FormInfos(FormMain form)
        {
            InitializeComponent();

            this.form = form;

            foreach (var i in form.listVideos.SelectedIndices)
            {
                if(!form.musicTags.ContainsKey((int)i))
                    form.musicTags.Add((int)i, null);

                var musicTags = form.musicTags[(int) i];
                if (musicTags != null)
                {
                    this.txtArtist.Text = musicTags.artist;
                    this.txtAlbum.Text = musicTags.album;
                    this.txtYear.Text = musicTags.year;
                    this.txtGenre.Text = musicTags.genre;
                }

            }

            this.btnAccept.Click += (s, e) =>
            {
                var tags = new FormMain.MusicTags();
                tags.artist = txtArtist.Text;
                tags.album = txtAlbum.Text;
                tags.year = txtYear.Text;
                tags.genre = txtGenre.Text;

                foreach (var i in form.listVideos.SelectedIndices)
                {
                    form.musicTags[(int) i] = tags;
                    this.Close();
                }
            };

            txtYear.KeyPress += (s, e) => e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            this.btnCancel.Click += (s, e) => this.Close();
        }
    }
}
