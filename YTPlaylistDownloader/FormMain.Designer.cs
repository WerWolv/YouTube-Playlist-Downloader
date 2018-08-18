namespace YTPlaylistDownloader
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.listVideos = new System.Windows.Forms.ListBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.boxConvert = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.imgThumbnail = new System.Windows.Forms.PictureBox();
            this.tmrStatusReset = new System.Windows.Forms.Timer(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.listQuality = new System.Windows.Forms.ComboBox();
            this.pBarVideos = new System.Windows.Forms.ProgressBar();
            this.pBarDownload = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChoose = new System.Windows.Forms.Button();
            this.txtDownloadPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(773, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(23, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "🔎";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(721, 38);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // listVideos
            // 
            this.listVideos.Dock = System.Windows.Forms.DockStyle.Left;
            this.listVideos.FormattingEnabled = true;
            this.listVideos.Location = new System.Drawing.Point(0, 0);
            this.listVideos.Name = "listVideos";
            this.listVideos.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listVideos.Size = new System.Drawing.Size(291, 492);
            this.listVideos.TabIndex = 0;
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(301, 12);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(466, 20);
            this.txtURL.TabIndex = 1;
            this.txtURL.Text = "Enter YouTube video or playlist URL here...";
            // 
            // boxConvert
            // 
            this.boxConvert.AutoSize = true;
            this.boxConvert.Location = new System.Drawing.Point(568, 69);
            this.boxConvert.Name = "boxConvert";
            this.boxConvert.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.boxConvert.Size = new System.Drawing.Size(100, 17);
            this.boxConvert.TabIndex = 4;
            this.boxConvert.Text = "Convert to MP3";
            this.boxConvert.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(721, 67);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(569, 94);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(33, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Label";
            // 
            // imgThumbnail
            // 
            this.imgThumbnail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgThumbnail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgThumbnail.ErrorImage = null;
            this.imgThumbnail.InitialImage = null;
            this.imgThumbnail.Location = new System.Drawing.Point(304, 136);
            this.imgThumbnail.Name = "imgThumbnail";
            this.imgThumbnail.Size = new System.Drawing.Size(488, 276);
            this.imgThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgThumbnail.TabIndex = 7;
            this.imgThumbnail.TabStop = false;
            // 
            // tmrStatusReset
            // 
            this.tmrStatusReset.Interval = 2000;
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(721, 96);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // listQuality
            // 
            this.listQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listQuality.FormattingEnabled = true;
            this.listQuality.Items.AddRange(new object[] {
            "720p",
            "480p",
            "360p",
            "240p",
            "144p"});
            this.listQuality.Location = new System.Drawing.Point(655, 42);
            this.listQuality.Name = "listQuality";
            this.listQuality.Size = new System.Drawing.Size(60, 21);
            this.listQuality.TabIndex = 2;
            // 
            // pBarVideos
            // 
            this.pBarVideos.Location = new System.Drawing.Point(301, 457);
            this.pBarVideos.Name = "pBarVideos";
            this.pBarVideos.Size = new System.Drawing.Size(493, 23);
            this.pBarVideos.TabIndex = 11;
            // 
            // pBarDownload
            // 
            this.pBarDownload.Location = new System.Drawing.Point(301, 435);
            this.pBarDownload.Name = "pBarDownload";
            this.pBarDownload.Size = new System.Drawing.Size(493, 23);
            this.pBarDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pBarDownload.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(569, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Video Quality";
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(433, 106);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 8;
            this.btnChoose.Text = "Choose...";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // txtDownloadPath
            // 
            this.txtDownloadPath.Location = new System.Drawing.Point(304, 81);
            this.txtDownloadPath.Name = "txtDownloadPath";
            this.txtDownloadPath.Size = new System.Drawing.Size(204, 20);
            this.txtDownloadPath.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Download path";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(806, 492);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDownloadPath);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pBarDownload);
            this.Controls.Add(this.pBarVideos);
            this.Controls.Add(this.listVideos);
            this.Controls.Add(this.listQuality);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.imgThumbnail);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.boxConvert);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "YouTube Playlist Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.imgThumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox boxConvert;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox imgThumbnail;
        private System.Windows.Forms.Timer tmrStatusReset;
        public System.Windows.Forms.ListBox listVideos;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox listQuality;
        private System.Windows.Forms.ProgressBar pBarVideos;
        private System.Windows.Forms.ProgressBar pBarDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.TextBox txtDownloadPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
    }
}

