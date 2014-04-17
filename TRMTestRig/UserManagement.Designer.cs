namespace TRMTestRig
{
    partial class UserManagement
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
            this.txtResult = new System.Windows.Forms.TextBox();
            this.tcUserManagement = new System.Windows.Forms.TabControl();
            this.tpRegistration = new System.Windows.Forms.TabPage();
            this.lbGenres = new System.Windows.Forms.ListBox();
            this.cbBusinessType = new System.Windows.Forms.ComboBox();
            this.btnGetUser = new System.Windows.Forms.Button();
            this.lblWordPressId = new System.Windows.Forms.Label();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.cbUserType = new System.Windows.Forms.ComboBox();
            this.lblUserType = new System.Windows.Forms.Label();
            this.txtWordpressId = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.tpAlbumUpload = new System.Windows.Forms.TabPage();
            this.lblAlbumDetails = new System.Windows.Forms.Label();
            this.txtAlbumDetails = new System.Windows.Forms.TextBox();
            this.btnGetAlbumDetails = new System.Windows.Forms.Button();
            this.btnDeleteAlbums = new System.Windows.Forms.Button();
            this.lbArtistAlbums = new System.Windows.Forms.ListBox();
            this.lblArtistAlbums = new System.Windows.Forms.Label();
            this.btnGetAlbums = new System.Windows.Forms.Button();
            this.lblAlbumGenres = new System.Windows.Forms.Label();
            this.lbAlbumGenres = new System.Windows.Forms.ListBox();
            this.txtAlbumResult = new System.Windows.Forms.TextBox();
            this.btnUploadAlbum = new System.Windows.Forms.Button();
            this.txtAlbumLabel = new System.Windows.Forms.TextBox();
            this.txtAlbumProducer = new System.Windows.Forms.TextBox();
            this.txtAlbumTitle = new System.Windows.Forms.TextBox();
            this.dtpAlbumReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.TxtAlbumWordpressId = new System.Windows.Forms.TextBox();
            this.lblAlbumLabel = new System.Windows.Forms.Label();
            this.lblAlbumProducer = new System.Windows.Forms.Label();
            this.lblAlbumTitle = new System.Windows.Forms.Label();
            this.lblAlbumReleaseDate = new System.Windows.Forms.Label();
            this.lblAlbumWordpressId = new System.Windows.Forms.Label();
            this.tpSongUpload = new System.Windows.Forms.TabPage();
            this.btnSelectSongFile = new System.Windows.Forms.Button();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.lbSongAlbums = new System.Windows.Forms.ListBox();
            this.lblSongArtistAlbums = new System.Windows.Forms.Label();
            this.btnGetSongAlbums = new System.Windows.Forms.Button();
            this.lblSongGenres = new System.Windows.Forms.Label();
            this.lbSongGenres = new System.Windows.Forms.ListBox();
            this.txtSongsResults = new System.Windows.Forms.TextBox();
            this.btnSaveSong = new System.Windows.Forms.Button();
            this.txtSongComposer = new System.Windows.Forms.TextBox();
            this.txtSongTitle = new System.Windows.Forms.TextBox();
            this.dtpSongReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.txtSongWordpressId = new System.Windows.Forms.TextBox();
            this.lblSongComposer = new System.Windows.Forms.Label();
            this.lblSongTitle = new System.Windows.Forms.Label();
            this.lblSongReleaseDate = new System.Windows.Forms.Label();
            this.lblSongWordpressId = new System.Windows.Forms.Label();
            this.tpCreatePlaylist = new System.Windows.Forms.TabPage();
            this.txtPlaylistResult = new System.Windows.Forms.TextBox();
            this.btnSavePlaylist = new System.Windows.Forms.Button();
            this.txtPlaylistName = new System.Windows.Forms.TextBox();
            this.lblPlaylistName = new System.Windows.Forms.Label();
            this.lblSelectSongs = new System.Windows.Forms.Label();
            this.lblSelectBusiness = new System.Windows.Forms.Label();
            this.cbBusinessUsers = new System.Windows.Forms.ComboBox();
            this.lbSongsForPlaylist = new System.Windows.Forms.ListBox();
            this.tpEditPlaylist = new System.Windows.Forms.TabPage();
            this.lblSelectPlaylist = new System.Windows.Forms.Label();
            this.cbPlaylists = new System.Windows.Forms.ComboBox();
            this.cbBusinessUserForEdit = new System.Windows.Forms.ComboBox();
            this.lblSelectBusinessForEdit = new System.Windows.Forms.Label();
            this.lbSelectedPlaylistSongs = new System.Windows.Forms.ListBox();
            this.lblSelectedPlaylistSongs = new System.Windows.Forms.Label();
            this.tcUserManagement.SuspendLayout();
            this.tpRegistration.SuspendLayout();
            this.tpAlbumUpload.SuspendLayout();
            this.tpSongUpload.SuspendLayout();
            this.tpCreatePlaylist.SuspendLayout();
            this.tpEditPlaylist.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(19, 364);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(406, 197);
            this.txtResult.TabIndex = 6;
            // 
            // tcUserManagement
            // 
            this.tcUserManagement.Controls.Add(this.tpRegistration);
            this.tcUserManagement.Controls.Add(this.tpAlbumUpload);
            this.tcUserManagement.Controls.Add(this.tpSongUpload);
            this.tcUserManagement.Controls.Add(this.tpCreatePlaylist);
            this.tcUserManagement.Controls.Add(this.tpEditPlaylist);
            this.tcUserManagement.Location = new System.Drawing.Point(13, 13);
            this.tcUserManagement.Name = "tcUserManagement";
            this.tcUserManagement.SelectedIndex = 0;
            this.tcUserManagement.Size = new System.Drawing.Size(457, 593);
            this.tcUserManagement.TabIndex = 7;
            // 
            // tpRegistration
            // 
            this.tpRegistration.Controls.Add(this.lbGenres);
            this.tpRegistration.Controls.Add(this.cbBusinessType);
            this.tpRegistration.Controls.Add(this.txtResult);
            this.tpRegistration.Controls.Add(this.btnGetUser);
            this.tpRegistration.Controls.Add(this.lblWordPressId);
            this.tpRegistration.Controls.Add(this.btnUpdateUser);
            this.tpRegistration.Controls.Add(this.cbUserType);
            this.tpRegistration.Controls.Add(this.lblUserType);
            this.tpRegistration.Controls.Add(this.txtWordpressId);
            this.tpRegistration.Controls.Add(this.btnRegister);
            this.tpRegistration.Location = new System.Drawing.Point(4, 22);
            this.tpRegistration.Name = "tpRegistration";
            this.tpRegistration.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegistration.Size = new System.Drawing.Size(449, 567);
            this.tpRegistration.TabIndex = 0;
            this.tpRegistration.Text = "Registration";
            this.tpRegistration.UseVisualStyleBackColor = true;
            // 
            // lbGenres
            // 
            this.lbGenres.FormattingEnabled = true;
            this.lbGenres.Location = new System.Drawing.Point(229, 56);
            this.lbGenres.Name = "lbGenres";
            this.lbGenres.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbGenres.Size = new System.Drawing.Size(193, 251);
            this.lbGenres.TabIndex = 8;
            this.lbGenres.Visible = false;
            // 
            // cbBusinessType
            // 
            this.cbBusinessType.FormattingEnabled = true;
            this.cbBusinessType.Location = new System.Drawing.Point(101, 84);
            this.cbBusinessType.Name = "cbBusinessType";
            this.cbBusinessType.Size = new System.Drawing.Size(121, 21);
            this.cbBusinessType.TabIndex = 7;
            this.cbBusinessType.Visible = false;
            // 
            // btnGetUser
            // 
            this.btnGetUser.Location = new System.Drawing.Point(101, 169);
            this.btnGetUser.Name = "btnGetUser";
            this.btnGetUser.Size = new System.Drawing.Size(75, 23);
            this.btnGetUser.TabIndex = 6;
            this.btnGetUser.Text = "Get User";
            this.btnGetUser.UseVisualStyleBackColor = true;
            this.btnGetUser.Click += new System.EventHandler(this.btnGetUser_Click);
            // 
            // lblWordPressId
            // 
            this.lblWordPressId.AutoSize = true;
            this.lblWordPressId.Location = new System.Drawing.Point(27, 30);
            this.lblWordPressId.Name = "lblWordPressId";
            this.lblWordPressId.Size = new System.Drawing.Size(67, 13);
            this.lblWordPressId.TabIndex = 0;
            this.lblWordPressId.Text = "WordpressId";
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Location = new System.Drawing.Point(101, 140);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateUser.TabIndex = 5;
            this.btnUpdateUser.Text = "Update";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // cbUserType
            // 
            this.cbUserType.FormattingEnabled = true;
            this.cbUserType.Items.AddRange(new object[] {
            "Admin",
            "Artist",
            "Business",
            "Customer"});
            this.cbUserType.Location = new System.Drawing.Point(101, 56);
            this.cbUserType.Name = "cbUserType";
            this.cbUserType.Size = new System.Drawing.Size(121, 21);
            this.cbUserType.TabIndex = 3;
            this.cbUserType.SelectedIndexChanged += new System.EventHandler(this.cbUserType_SelectedIndexChanged);
            // 
            // lblUserType
            // 
            this.lblUserType.AutoSize = true;
            this.lblUserType.Location = new System.Drawing.Point(28, 59);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(56, 13);
            this.lblUserType.TabIndex = 1;
            this.lblUserType.Text = "User Type";
            // 
            // txtWordpressId
            // 
            this.txtWordpressId.Location = new System.Drawing.Point(101, 30);
            this.txtWordpressId.Name = "txtWordpressId";
            this.txtWordpressId.Size = new System.Drawing.Size(39, 20);
            this.txtWordpressId.TabIndex = 2;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(101, 111);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // tpAlbumUpload
            // 
            this.tpAlbumUpload.Controls.Add(this.lblAlbumDetails);
            this.tpAlbumUpload.Controls.Add(this.txtAlbumDetails);
            this.tpAlbumUpload.Controls.Add(this.btnGetAlbumDetails);
            this.tpAlbumUpload.Controls.Add(this.btnDeleteAlbums);
            this.tpAlbumUpload.Controls.Add(this.lbArtistAlbums);
            this.tpAlbumUpload.Controls.Add(this.lblArtistAlbums);
            this.tpAlbumUpload.Controls.Add(this.btnGetAlbums);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumGenres);
            this.tpAlbumUpload.Controls.Add(this.lbAlbumGenres);
            this.tpAlbumUpload.Controls.Add(this.txtAlbumResult);
            this.tpAlbumUpload.Controls.Add(this.btnUploadAlbum);
            this.tpAlbumUpload.Controls.Add(this.txtAlbumLabel);
            this.tpAlbumUpload.Controls.Add(this.txtAlbumProducer);
            this.tpAlbumUpload.Controls.Add(this.txtAlbumTitle);
            this.tpAlbumUpload.Controls.Add(this.dtpAlbumReleaseDate);
            this.tpAlbumUpload.Controls.Add(this.TxtAlbumWordpressId);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumLabel);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumProducer);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumTitle);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumReleaseDate);
            this.tpAlbumUpload.Controls.Add(this.lblAlbumWordpressId);
            this.tpAlbumUpload.Location = new System.Drawing.Point(4, 22);
            this.tpAlbumUpload.Name = "tpAlbumUpload";
            this.tpAlbumUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tpAlbumUpload.Size = new System.Drawing.Size(449, 567);
            this.tpAlbumUpload.TabIndex = 1;
            this.tpAlbumUpload.Text = "Album Upload";
            this.tpAlbumUpload.UseVisualStyleBackColor = true;
            // 
            // lblAlbumDetails
            // 
            this.lblAlbumDetails.AutoSize = true;
            this.lblAlbumDetails.Location = new System.Drawing.Point(274, 177);
            this.lblAlbumDetails.Name = "lblAlbumDetails";
            this.lblAlbumDetails.Size = new System.Drawing.Size(71, 13);
            this.lblAlbumDetails.TabIndex = 21;
            this.lblAlbumDetails.Text = "Album Details";
            // 
            // txtAlbumDetails
            // 
            this.txtAlbumDetails.Location = new System.Drawing.Point(274, 200);
            this.txtAlbumDetails.Multiline = true;
            this.txtAlbumDetails.Name = "txtAlbumDetails";
            this.txtAlbumDetails.Size = new System.Drawing.Size(169, 162);
            this.txtAlbumDetails.TabIndex = 20;
            // 
            // btnGetAlbumDetails
            // 
            this.btnGetAlbumDetails.Location = new System.Drawing.Point(274, 147);
            this.btnGetAlbumDetails.Name = "btnGetAlbumDetails";
            this.btnGetAlbumDetails.Size = new System.Drawing.Size(75, 23);
            this.btnGetAlbumDetails.TabIndex = 19;
            this.btnGetAlbumDetails.Text = "Get Details";
            this.btnGetAlbumDetails.UseVisualStyleBackColor = true;
            this.btnGetAlbumDetails.Click += new System.EventHandler(this.btnGetAlbumDetails_Click);
            // 
            // btnDeleteAlbums
            // 
            this.btnDeleteAlbums.Location = new System.Drawing.Point(358, 147);
            this.btnDeleteAlbums.Name = "btnDeleteAlbums";
            this.btnDeleteAlbums.Size = new System.Drawing.Size(84, 23);
            this.btnDeleteAlbums.TabIndex = 17;
            this.btnDeleteAlbums.Text = "Delete Album";
            this.btnDeleteAlbums.UseVisualStyleBackColor = true;
            this.btnDeleteAlbums.Click += new System.EventHandler(this.btnDeleteAlbums_Click);
            // 
            // lbArtistAlbums
            // 
            this.lbArtistAlbums.FormattingEnabled = true;
            this.lbArtistAlbums.Location = new System.Drawing.Point(274, 32);
            this.lbArtistAlbums.Name = "lbArtistAlbums";
            this.lbArtistAlbums.Size = new System.Drawing.Size(169, 108);
            this.lbArtistAlbums.TabIndex = 16;
            // 
            // lblArtistAlbums
            // 
            this.lblArtistAlbums.AutoSize = true;
            this.lblArtistAlbums.Location = new System.Drawing.Point(271, 7);
            this.lblArtistAlbums.Name = "lblArtistAlbums";
            this.lblArtistAlbums.Size = new System.Drawing.Size(67, 13);
            this.lblArtistAlbums.TabIndex = 15;
            this.lblArtistAlbums.Text = "Artist Albums";
            // 
            // btnGetAlbums
            // 
            this.btnGetAlbums.Location = new System.Drawing.Point(169, 7);
            this.btnGetAlbums.Name = "btnGetAlbums";
            this.btnGetAlbums.Size = new System.Drawing.Size(95, 23);
            this.btnGetAlbums.TabIndex = 14;
            this.btnGetAlbums.Text = "Get Albums";
            this.btnGetAlbums.UseVisualStyleBackColor = true;
            this.btnGetAlbums.Click += new System.EventHandler(this.btnGetAlbums_Click);
            // 
            // lblAlbumGenres
            // 
            this.lblAlbumGenres.AutoSize = true;
            this.lblAlbumGenres.Location = new System.Drawing.Point(10, 163);
            this.lblAlbumGenres.Name = "lblAlbumGenres";
            this.lblAlbumGenres.Size = new System.Drawing.Size(73, 13);
            this.lblAlbumGenres.TabIndex = 13;
            this.lblAlbumGenres.Text = "Album Genres";
            // 
            // lbAlbumGenres
            // 
            this.lbAlbumGenres.FormattingEnabled = true;
            this.lbAlbumGenres.Location = new System.Drawing.Point(89, 163);
            this.lbAlbumGenres.Name = "lbAlbumGenres";
            this.lbAlbumGenres.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAlbumGenres.Size = new System.Drawing.Size(175, 199);
            this.lbAlbumGenres.TabIndex = 12;
            // 
            // txtAlbumResult
            // 
            this.txtAlbumResult.Location = new System.Drawing.Point(10, 406);
            this.txtAlbumResult.Multiline = true;
            this.txtAlbumResult.Name = "txtAlbumResult";
            this.txtAlbumResult.Size = new System.Drawing.Size(254, 155);
            this.txtAlbumResult.TabIndex = 11;
            // 
            // btnUploadAlbum
            // 
            this.btnUploadAlbum.Location = new System.Drawing.Point(189, 368);
            this.btnUploadAlbum.Name = "btnUploadAlbum";
            this.btnUploadAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnUploadAlbum.TabIndex = 10;
            this.btnUploadAlbum.Text = "Save Album";
            this.btnUploadAlbum.UseVisualStyleBackColor = true;
            this.btnUploadAlbum.Click += new System.EventHandler(this.btnUploadAlbum_Click);
            // 
            // txtAlbumLabel
            // 
            this.txtAlbumLabel.Location = new System.Drawing.Point(130, 123);
            this.txtAlbumLabel.Name = "txtAlbumLabel";
            this.txtAlbumLabel.Size = new System.Drawing.Size(134, 20);
            this.txtAlbumLabel.TabIndex = 9;
            // 
            // txtAlbumProducer
            // 
            this.txtAlbumProducer.Location = new System.Drawing.Point(130, 92);
            this.txtAlbumProducer.Name = "txtAlbumProducer";
            this.txtAlbumProducer.Size = new System.Drawing.Size(134, 20);
            this.txtAlbumProducer.TabIndex = 8;
            // 
            // txtAlbumTitle
            // 
            this.txtAlbumTitle.Location = new System.Drawing.Point(130, 63);
            this.txtAlbumTitle.Name = "txtAlbumTitle";
            this.txtAlbumTitle.Size = new System.Drawing.Size(134, 20);
            this.txtAlbumTitle.TabIndex = 7;
            // 
            // dtpAlbumReleaseDate
            // 
            this.dtpAlbumReleaseDate.Location = new System.Drawing.Point(130, 32);
            this.dtpAlbumReleaseDate.Name = "dtpAlbumReleaseDate";
            this.dtpAlbumReleaseDate.Size = new System.Drawing.Size(134, 20);
            this.dtpAlbumReleaseDate.TabIndex = 6;
            // 
            // TxtAlbumWordpressId
            // 
            this.TxtAlbumWordpressId.Location = new System.Drawing.Point(130, 7);
            this.TxtAlbumWordpressId.Name = "TxtAlbumWordpressId";
            this.TxtAlbumWordpressId.Size = new System.Drawing.Size(32, 20);
            this.TxtAlbumWordpressId.TabIndex = 5;
            // 
            // lblAlbumLabel
            // 
            this.lblAlbumLabel.AutoSize = true;
            this.lblAlbumLabel.Location = new System.Drawing.Point(7, 123);
            this.lblAlbumLabel.Name = "lblAlbumLabel";
            this.lblAlbumLabel.Size = new System.Drawing.Size(65, 13);
            this.lblAlbumLabel.TabIndex = 4;
            this.lblAlbumLabel.Text = "Album Label";
            // 
            // lblAlbumProducer
            // 
            this.lblAlbumProducer.AutoSize = true;
            this.lblAlbumProducer.Location = new System.Drawing.Point(7, 92);
            this.lblAlbumProducer.Name = "lblAlbumProducer";
            this.lblAlbumProducer.Size = new System.Drawing.Size(82, 13);
            this.lblAlbumProducer.TabIndex = 3;
            this.lblAlbumProducer.Text = "Album Producer";
            // 
            // lblAlbumTitle
            // 
            this.lblAlbumTitle.AutoSize = true;
            this.lblAlbumTitle.Location = new System.Drawing.Point(7, 63);
            this.lblAlbumTitle.Name = "lblAlbumTitle";
            this.lblAlbumTitle.Size = new System.Drawing.Size(59, 13);
            this.lblAlbumTitle.TabIndex = 2;
            this.lblAlbumTitle.Text = "Album Title";
            // 
            // lblAlbumReleaseDate
            // 
            this.lblAlbumReleaseDate.AutoSize = true;
            this.lblAlbumReleaseDate.Location = new System.Drawing.Point(7, 32);
            this.lblAlbumReleaseDate.Name = "lblAlbumReleaseDate";
            this.lblAlbumReleaseDate.Size = new System.Drawing.Size(104, 13);
            this.lblAlbumReleaseDate.TabIndex = 1;
            this.lblAlbumReleaseDate.Text = "Album Release Date";
            // 
            // lblAlbumWordpressId
            // 
            this.lblAlbumWordpressId.AutoSize = true;
            this.lblAlbumWordpressId.Location = new System.Drawing.Point(7, 7);
            this.lblAlbumWordpressId.Name = "lblAlbumWordpressId";
            this.lblAlbumWordpressId.Size = new System.Drawing.Size(70, 13);
            this.lblAlbumWordpressId.TabIndex = 0;
            this.lblAlbumWordpressId.Text = "Wordpress Id";
            // 
            // tpSongUpload
            // 
            this.tpSongUpload.Controls.Add(this.btnSelectSongFile);
            this.tpSongUpload.Controls.Add(this.lblSelectedFile);
            this.tpSongUpload.Controls.Add(this.lbSongAlbums);
            this.tpSongUpload.Controls.Add(this.lblSongArtistAlbums);
            this.tpSongUpload.Controls.Add(this.btnGetSongAlbums);
            this.tpSongUpload.Controls.Add(this.lblSongGenres);
            this.tpSongUpload.Controls.Add(this.lbSongGenres);
            this.tpSongUpload.Controls.Add(this.txtSongsResults);
            this.tpSongUpload.Controls.Add(this.btnSaveSong);
            this.tpSongUpload.Controls.Add(this.txtSongComposer);
            this.tpSongUpload.Controls.Add(this.txtSongTitle);
            this.tpSongUpload.Controls.Add(this.dtpSongReleaseDate);
            this.tpSongUpload.Controls.Add(this.txtSongWordpressId);
            this.tpSongUpload.Controls.Add(this.lblSongComposer);
            this.tpSongUpload.Controls.Add(this.lblSongTitle);
            this.tpSongUpload.Controls.Add(this.lblSongReleaseDate);
            this.tpSongUpload.Controls.Add(this.lblSongWordpressId);
            this.tpSongUpload.Location = new System.Drawing.Point(4, 22);
            this.tpSongUpload.Name = "tpSongUpload";
            this.tpSongUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tpSongUpload.Size = new System.Drawing.Size(449, 567);
            this.tpSongUpload.TabIndex = 2;
            this.tpSongUpload.Text = "Song Upload";
            this.tpSongUpload.UseVisualStyleBackColor = true;
            // 
            // btnSelectSongFile
            // 
            this.btnSelectSongFile.Location = new System.Drawing.Point(188, 120);
            this.btnSelectSongFile.Name = "btnSelectSongFile";
            this.btnSelectSongFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectSongFile.TabIndex = 35;
            this.btnSelectSongFile.Text = "Browse Files";
            this.btnSelectSongFile.UseVisualStyleBackColor = true;
            this.btnSelectSongFile.Click += new System.EventHandler(this.btnSelectSongFile_Click);
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(9, 125);
            this.lblSelectedFile.MaximumSize = new System.Drawing.Size(180, 0);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(142, 13);
            this.lblSelectedFile.TabIndex = 34;
            this.lblSelectedFile.Text = "Please select a file to upload";
            // 
            // lbSongAlbums
            // 
            this.lbSongAlbums.FormattingEnabled = true;
            this.lbSongAlbums.Location = new System.Drawing.Point(273, 31);
            this.lbSongAlbums.Name = "lbSongAlbums";
            this.lbSongAlbums.Size = new System.Drawing.Size(169, 108);
            this.lbSongAlbums.TabIndex = 33;
            // 
            // lblSongArtistAlbums
            // 
            this.lblSongArtistAlbums.AutoSize = true;
            this.lblSongArtistAlbums.Location = new System.Drawing.Point(270, 6);
            this.lblSongArtistAlbums.Name = "lblSongArtistAlbums";
            this.lblSongArtistAlbums.Size = new System.Drawing.Size(67, 13);
            this.lblSongArtistAlbums.TabIndex = 32;
            this.lblSongArtistAlbums.Text = "Artist Albums";
            // 
            // btnGetSongAlbums
            // 
            this.btnGetSongAlbums.Location = new System.Drawing.Point(168, 6);
            this.btnGetSongAlbums.Name = "btnGetSongAlbums";
            this.btnGetSongAlbums.Size = new System.Drawing.Size(95, 23);
            this.btnGetSongAlbums.TabIndex = 31;
            this.btnGetSongAlbums.Text = "Get Albums";
            this.btnGetSongAlbums.UseVisualStyleBackColor = true;
            this.btnGetSongAlbums.Click += new System.EventHandler(this.btnGetSongAlbums_Click);
            // 
            // lblSongGenres
            // 
            this.lblSongGenres.AutoSize = true;
            this.lblSongGenres.Location = new System.Drawing.Point(9, 210);
            this.lblSongGenres.Name = "lblSongGenres";
            this.lblSongGenres.Size = new System.Drawing.Size(69, 13);
            this.lblSongGenres.TabIndex = 30;
            this.lblSongGenres.Text = "Song Genres";
            // 
            // lbSongGenres
            // 
            this.lbSongGenres.FormattingEnabled = true;
            this.lbSongGenres.Location = new System.Drawing.Point(88, 210);
            this.lbSongGenres.Name = "lbSongGenres";
            this.lbSongGenres.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSongGenres.Size = new System.Drawing.Size(175, 199);
            this.lbSongGenres.TabIndex = 29;
            // 
            // txtSongsResults
            // 
            this.txtSongsResults.Location = new System.Drawing.Point(9, 444);
            this.txtSongsResults.Multiline = true;
            this.txtSongsResults.Name = "txtSongsResults";
            this.txtSongsResults.Size = new System.Drawing.Size(254, 116);
            this.txtSongsResults.TabIndex = 28;
            // 
            // btnSaveSong
            // 
            this.btnSaveSong.Location = new System.Drawing.Point(188, 415);
            this.btnSaveSong.Name = "btnSaveSong";
            this.btnSaveSong.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSong.TabIndex = 27;
            this.btnSaveSong.Text = "Save Song";
            this.btnSaveSong.UseVisualStyleBackColor = true;
            this.btnSaveSong.Click += new System.EventHandler(this.btnSaveSong_Click);
            // 
            // txtSongComposer
            // 
            this.txtSongComposer.Location = new System.Drawing.Point(129, 91);
            this.txtSongComposer.Name = "txtSongComposer";
            this.txtSongComposer.Size = new System.Drawing.Size(134, 20);
            this.txtSongComposer.TabIndex = 25;
            // 
            // txtSongTitle
            // 
            this.txtSongTitle.Location = new System.Drawing.Point(129, 62);
            this.txtSongTitle.Name = "txtSongTitle";
            this.txtSongTitle.Size = new System.Drawing.Size(134, 20);
            this.txtSongTitle.TabIndex = 24;
            // 
            // dtpSongReleaseDate
            // 
            this.dtpSongReleaseDate.Location = new System.Drawing.Point(129, 31);
            this.dtpSongReleaseDate.Name = "dtpSongReleaseDate";
            this.dtpSongReleaseDate.Size = new System.Drawing.Size(134, 20);
            this.dtpSongReleaseDate.TabIndex = 23;
            // 
            // txtSongWordpressId
            // 
            this.txtSongWordpressId.Location = new System.Drawing.Point(129, 6);
            this.txtSongWordpressId.Name = "txtSongWordpressId";
            this.txtSongWordpressId.Size = new System.Drawing.Size(32, 20);
            this.txtSongWordpressId.TabIndex = 22;
            // 
            // lblSongComposer
            // 
            this.lblSongComposer.AutoSize = true;
            this.lblSongComposer.Location = new System.Drawing.Point(6, 91);
            this.lblSongComposer.Name = "lblSongComposer";
            this.lblSongComposer.Size = new System.Drawing.Size(82, 13);
            this.lblSongComposer.TabIndex = 20;
            this.lblSongComposer.Text = "Song Composer";
            // 
            // lblSongTitle
            // 
            this.lblSongTitle.AutoSize = true;
            this.lblSongTitle.Location = new System.Drawing.Point(6, 62);
            this.lblSongTitle.Name = "lblSongTitle";
            this.lblSongTitle.Size = new System.Drawing.Size(55, 13);
            this.lblSongTitle.TabIndex = 19;
            this.lblSongTitle.Text = "Song Title";
            // 
            // lblSongReleaseDate
            // 
            this.lblSongReleaseDate.AutoSize = true;
            this.lblSongReleaseDate.Location = new System.Drawing.Point(6, 31);
            this.lblSongReleaseDate.Name = "lblSongReleaseDate";
            this.lblSongReleaseDate.Size = new System.Drawing.Size(100, 13);
            this.lblSongReleaseDate.TabIndex = 18;
            this.lblSongReleaseDate.Text = "Song Release Date";
            // 
            // lblSongWordpressId
            // 
            this.lblSongWordpressId.AutoSize = true;
            this.lblSongWordpressId.Location = new System.Drawing.Point(6, 6);
            this.lblSongWordpressId.Name = "lblSongWordpressId";
            this.lblSongWordpressId.Size = new System.Drawing.Size(70, 13);
            this.lblSongWordpressId.TabIndex = 17;
            this.lblSongWordpressId.Text = "Wordpress Id";
            // 
            // tpCreatePlaylist
            // 
            this.tpCreatePlaylist.Controls.Add(this.lbSongsForPlaylist);
            this.tpCreatePlaylist.Controls.Add(this.cbBusinessUsers);
            this.tpCreatePlaylist.Controls.Add(this.lblSelectBusiness);
            this.tpCreatePlaylist.Controls.Add(this.txtPlaylistResult);
            this.tpCreatePlaylist.Controls.Add(this.btnSavePlaylist);
            this.tpCreatePlaylist.Controls.Add(this.txtPlaylistName);
            this.tpCreatePlaylist.Controls.Add(this.lblPlaylistName);
            this.tpCreatePlaylist.Controls.Add(this.lblSelectSongs);
            this.tpCreatePlaylist.Location = new System.Drawing.Point(4, 22);
            this.tpCreatePlaylist.Name = "tpCreatePlaylist";
            this.tpCreatePlaylist.Size = new System.Drawing.Size(449, 567);
            this.tpCreatePlaylist.TabIndex = 3;
            this.tpCreatePlaylist.Text = "Create Playlist";
            this.tpCreatePlaylist.UseVisualStyleBackColor = true;
            // 
            // txtPlaylistResult
            // 
            this.txtPlaylistResult.Location = new System.Drawing.Point(9, 416);
            this.txtPlaylistResult.Multiline = true;
            this.txtPlaylistResult.Name = "txtPlaylistResult";
            this.txtPlaylistResult.Size = new System.Drawing.Size(428, 138);
            this.txtPlaylistResult.TabIndex = 29;
            // 
            // btnSavePlaylist
            // 
            this.btnSavePlaylist.Location = new System.Drawing.Point(361, 387);
            this.btnSavePlaylist.Name = "btnSavePlaylist";
            this.btnSavePlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnSavePlaylist.TabIndex = 4;
            this.btnSavePlaylist.Text = "Save Playlist";
            this.btnSavePlaylist.UseVisualStyleBackColor = true;
            this.btnSavePlaylist.Click += new System.EventHandler(this.btnSaveSong_Click);
            // 
            // txtPlaylistName
            // 
            this.txtPlaylistName.Location = new System.Drawing.Point(8, 68);
            this.txtPlaylistName.Name = "txtPlaylistName";
            this.txtPlaylistName.Size = new System.Drawing.Size(223, 20);
            this.txtPlaylistName.TabIndex = 2;
            // 
            // lblPlaylistName
            // 
            this.lblPlaylistName.AutoSize = true;
            this.lblPlaylistName.Location = new System.Drawing.Point(5, 52);
            this.lblPlaylistName.Name = "lblPlaylistName";
            this.lblPlaylistName.Size = new System.Drawing.Size(141, 13);
            this.lblPlaylistName.TabIndex = 1;
            this.lblPlaylistName.Text = "Type a name for your playlist";
            // 
            // lblSelectSongs
            // 
            this.lblSelectSongs.AutoSize = true;
            this.lblSelectSongs.Location = new System.Drawing.Point(6, 91);
            this.lblSelectSongs.Name = "lblSelectSongs";
            this.lblSelectSongs.Size = new System.Drawing.Size(165, 13);
            this.lblSelectSongs.TabIndex = 0;
            this.lblSelectSongs.Text = "Select songs to add to the playlist";
            // 
            // lblSelectBusiness
            // 
            this.lblSelectBusiness.AutoSize = true;
            this.lblSelectBusiness.Location = new System.Drawing.Point(9, 4);
            this.lblSelectBusiness.Name = "lblSelectBusiness";
            this.lblSelectBusiness.Size = new System.Drawing.Size(113, 13);
            this.lblSelectBusiness.TabIndex = 30;
            this.lblSelectBusiness.Text = "Select a business user";
            // 
            // cbBusinessUsers
            // 
            this.cbBusinessUsers.FormattingEnabled = true;
            this.cbBusinessUsers.Location = new System.Drawing.Point(9, 21);
            this.cbBusinessUsers.Name = "cbBusinessUsers";
            this.cbBusinessUsers.Size = new System.Drawing.Size(222, 21);
            this.cbBusinessUsers.TabIndex = 31;
            // 
            // lbSongsForPlaylist
            // 
            this.lbSongsForPlaylist.FormattingEnabled = true;
            this.lbSongsForPlaylist.Location = new System.Drawing.Point(9, 108);
            this.lbSongsForPlaylist.Name = "lbSongsForPlaylist";
            this.lbSongsForPlaylist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSongsForPlaylist.Size = new System.Drawing.Size(427, 264);
            this.lbSongsForPlaylist.TabIndex = 32;
            // 
            // tpEditPlaylist
            // 
            this.tpEditPlaylist.Controls.Add(this.lbSelectedPlaylistSongs);
            this.tpEditPlaylist.Controls.Add(this.lblSelectedPlaylistSongs);
            this.tpEditPlaylist.Controls.Add(this.cbBusinessUserForEdit);
            this.tpEditPlaylist.Controls.Add(this.lblSelectBusinessForEdit);
            this.tpEditPlaylist.Controls.Add(this.cbPlaylists);
            this.tpEditPlaylist.Controls.Add(this.lblSelectPlaylist);
            this.tpEditPlaylist.Location = new System.Drawing.Point(4, 22);
            this.tpEditPlaylist.Name = "tpEditPlaylist";
            this.tpEditPlaylist.Size = new System.Drawing.Size(449, 567);
            this.tpEditPlaylist.TabIndex = 4;
            this.tpEditPlaylist.Text = "Edit playlist";
            this.tpEditPlaylist.UseVisualStyleBackColor = true;
            // 
            // lblSelectPlaylist
            // 
            this.lblSelectPlaylist.AutoSize = true;
            this.lblSelectPlaylist.Location = new System.Drawing.Point(4, 51);
            this.lblSelectPlaylist.Name = "lblSelectPlaylist";
            this.lblSelectPlaylist.Size = new System.Drawing.Size(112, 13);
            this.lblSelectPlaylist.TabIndex = 0;
            this.lblSelectPlaylist.Text = "Select a playlist to edit";
            // 
            // cbPlaylists
            // 
            this.cbPlaylists.FormattingEnabled = true;
            this.cbPlaylists.Location = new System.Drawing.Point(4, 67);
            this.cbPlaylists.Name = "cbPlaylists";
            this.cbPlaylists.Size = new System.Drawing.Size(222, 21);
            this.cbPlaylists.TabIndex = 1;
            // 
            // cbBusinessUserForEdit
            // 
            this.cbBusinessUserForEdit.FormattingEnabled = true;
            this.cbBusinessUserForEdit.Location = new System.Drawing.Point(4, 26);
            this.cbBusinessUserForEdit.Name = "cbBusinessUserForEdit";
            this.cbBusinessUserForEdit.Size = new System.Drawing.Size(222, 21);
            this.cbBusinessUserForEdit.TabIndex = 33;
            // 
            // lblSelectBusinessForEdit
            // 
            this.lblSelectBusinessForEdit.AutoSize = true;
            this.lblSelectBusinessForEdit.Location = new System.Drawing.Point(4, 9);
            this.lblSelectBusinessForEdit.Name = "lblSelectBusinessForEdit";
            this.lblSelectBusinessForEdit.Size = new System.Drawing.Size(113, 13);
            this.lblSelectBusinessForEdit.TabIndex = 32;
            this.lblSelectBusinessForEdit.Text = "Select a business user";
            // 
            // lbSelectedPlaylistSongs
            // 
            this.lbSelectedPlaylistSongs.FormattingEnabled = true;
            this.lbSelectedPlaylistSongs.Location = new System.Drawing.Point(4, 106);
            this.lbSelectedPlaylistSongs.Name = "lbSelectedPlaylistSongs";
            this.lbSelectedPlaylistSongs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelectedPlaylistSongs.Size = new System.Drawing.Size(442, 264);
            this.lbSelectedPlaylistSongs.TabIndex = 35;
            // 
            // lblSelectedPlaylistSongs
            // 
            this.lblSelectedPlaylistSongs.AutoSize = true;
            this.lblSelectedPlaylistSongs.Location = new System.Drawing.Point(4, 90);
            this.lblSelectedPlaylistSongs.Name = "lblSelectedPlaylistSongs";
            this.lblSelectedPlaylistSongs.Size = new System.Drawing.Size(165, 13);
            this.lblSelectedPlaylistSongs.TabIndex = 34;
            this.lblSelectedPlaylistSongs.Text = "Select songs to add to the playlist";
            // 
            // UserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 618);
            this.Controls.Add(this.tcUserManagement);
            this.Name = "UserManagement";
            this.Text = "Registration";
            this.tcUserManagement.ResumeLayout(false);
            this.tpRegistration.ResumeLayout(false);
            this.tpRegistration.PerformLayout();
            this.tpAlbumUpload.ResumeLayout(false);
            this.tpAlbumUpload.PerformLayout();
            this.tpSongUpload.ResumeLayout(false);
            this.tpSongUpload.PerformLayout();
            this.tpCreatePlaylist.ResumeLayout(false);
            this.tpCreatePlaylist.PerformLayout();
            this.tpEditPlaylist.ResumeLayout(false);
            this.tpEditPlaylist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TabControl tcUserManagement;
        private System.Windows.Forms.TabPage tpRegistration;
        private System.Windows.Forms.ListBox lbGenres;
        private System.Windows.Forms.ComboBox cbBusinessType;
        private System.Windows.Forms.Button btnGetUser;
        private System.Windows.Forms.Label lblWordPressId;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.ComboBox cbUserType;
        private System.Windows.Forms.Label lblUserType;
        private System.Windows.Forms.TextBox txtWordpressId;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TabPage tpAlbumUpload;
        private System.Windows.Forms.TextBox txtAlbumResult;
        private System.Windows.Forms.Button btnUploadAlbum;
        private System.Windows.Forms.TextBox txtAlbumLabel;
        private System.Windows.Forms.TextBox txtAlbumProducer;
        private System.Windows.Forms.TextBox txtAlbumTitle;
        private System.Windows.Forms.DateTimePicker dtpAlbumReleaseDate;
        private System.Windows.Forms.TextBox TxtAlbumWordpressId;
        private System.Windows.Forms.Label lblAlbumLabel;
        private System.Windows.Forms.Label lblAlbumProducer;
        private System.Windows.Forms.Label lblAlbumTitle;
        private System.Windows.Forms.Label lblAlbumReleaseDate;
        private System.Windows.Forms.Label lblAlbumWordpressId;
        private System.Windows.Forms.Label lblAlbumGenres;
        private System.Windows.Forms.ListBox lbAlbumGenres;
        private System.Windows.Forms.ListBox lbArtistAlbums;
        private System.Windows.Forms.Label lblArtistAlbums;
        private System.Windows.Forms.Button btnGetAlbums;
        private System.Windows.Forms.Button btnDeleteAlbums;
        private System.Windows.Forms.Button btnGetAlbumDetails;
        private System.Windows.Forms.TextBox txtAlbumDetails;
        private System.Windows.Forms.Label lblAlbumDetails;
        private System.Windows.Forms.TabPage tpSongUpload;
        private System.Windows.Forms.ListBox lbSongAlbums;
        private System.Windows.Forms.Label lblSongArtistAlbums;
        private System.Windows.Forms.Button btnGetSongAlbums;
        private System.Windows.Forms.Label lblSongGenres;
        private System.Windows.Forms.ListBox lbSongGenres;
        private System.Windows.Forms.TextBox txtSongsResults;
        private System.Windows.Forms.Button btnSaveSong;
        private System.Windows.Forms.TextBox txtSongComposer;
        private System.Windows.Forms.TextBox txtSongTitle;
        private System.Windows.Forms.DateTimePicker dtpSongReleaseDate;
        private System.Windows.Forms.TextBox txtSongWordpressId;
        private System.Windows.Forms.Label lblSongComposer;
        private System.Windows.Forms.Label lblSongTitle;
        private System.Windows.Forms.Label lblSongReleaseDate;
        private System.Windows.Forms.Label lblSongWordpressId;
        private System.Windows.Forms.Button btnSelectSongFile;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.TabPage tpCreatePlaylist;
        private System.Windows.Forms.TextBox txtPlaylistResult;
        private System.Windows.Forms.Button btnSavePlaylist;
        private System.Windows.Forms.TextBox txtPlaylistName;
        private System.Windows.Forms.Label lblPlaylistName;
        private System.Windows.Forms.Label lblSelectSongs;
        private System.Windows.Forms.ComboBox cbBusinessUsers;
        private System.Windows.Forms.Label lblSelectBusiness;
        private System.Windows.Forms.ListBox lbSongsForPlaylist;
        private System.Windows.Forms.TabPage tpEditPlaylist;
        private System.Windows.Forms.ListBox lbSelectedPlaylistSongs;
        private System.Windows.Forms.Label lblSelectedPlaylistSongs;
        private System.Windows.Forms.ComboBox cbBusinessUserForEdit;
        private System.Windows.Forms.Label lblSelectBusinessForEdit;
        private System.Windows.Forms.ComboBox cbPlaylists;
        private System.Windows.Forms.Label lblSelectPlaylist;
    }
}

