using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainModel.Entities;
//using TRMTestRig.TRMWebServiceJson;
using TRMWebService;
using TRMInfrastructure.Utilities;

namespace TRMTestRig
{
    public partial class UserManagement : Form
    {
        private readonly TRMWCFWebServiceJson TRMWCFWebServiceJson;
        //private readonly TRMWCFWebServiceJsonClient TRMWCFWebServiceJson;

        public UserManagement()
        {
            TRMWCFWebServiceJson = new TRMWCFWebServiceJson();
            //TRMWCFWebServiceJson = new TRMWCFWebServiceJsonClient();
            InitializeComponent();
            InitializeBusinessTypes();
            InitializeGenres();
        }

        #region Registration section

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // the user instance will be used accross all registration types
            var user = new User()
            {
                //WordpressUserId = Convert.ToInt32(txtWordpressId.Text),
                //CreatedDate = DateTime.Now,
                //UserTypeId = (int)GetUserType()
            };

            switch (GetUserType())
            {
                case User.UserTypeList.Admin:
                    TRMWCFWebServiceJson.RegisterAdmin(user);
                    break;
                case User.UserTypeList.Artist:
                    var artist = new Artist();
                    var artistGenreCollection = (from ListItem listItem in lbGenres.SelectedItems select TRMWCFWebServiceJson.GetGenre(listItem.SelectedValue())).ToList();

                    TRMWCFWebServiceJson.RegisterArtist(artist, artistGenreCollection, null);
                    break;
                case User.UserTypeList.Business:
                    var businessTypeListItem = (ListItem)cbBusinessType.SelectedItem;
                    var businessType = new BusinessType()
                    {
                        BusinessTypeName = businessTypeListItem.ToString(),
                        BusinessTypeId = businessTypeListItem.SelectedValue()
                    };
                    //TRMWCFWebServiceJson.RegisterBusiness(user, businessType, "0.0345749857", "-52.489082309");
                    break;
                default:
                    TRMWCFWebServiceJson.RegisterCustomer(user);
                    break;
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            switch (GetUserType())
            {
                case User.UserTypeList.Artist:
                    var artist = TRMWCFWebServiceJson.GetArtist(Convert.ToInt32(txtWordpressId.Text));

                    var businessTypeCollection = (from ListItem listItem in lbGenres.SelectedItems select TRMWCFWebServiceJson.GetGenre(listItem.SelectedValue())).ToList();

                    TRMWCFWebServiceJson.UpdateArtistGenreCollection(artist.UserId, businessTypeCollection);
                    break;
                case User.UserTypeList.Business:
                    var business = TRMWCFWebServiceJson.GetArtist(Convert.ToInt32(txtWordpressId.Text));

                    var businessTypeListItem = (ListItem)cbBusinessType.SelectedItem;
                    var businessType = new BusinessType()
                    {
                        BusinessTypeName = businessTypeListItem.ToString(),
                        BusinessTypeId = businessTypeListItem.SelectedValue()
                    };

                    TRMWCFWebServiceJson.UpdateBusinessUser(business, businessType, "0.0345749857", "-52.489082309");
                    break;
            }
        }

        private void btnGetUser_Click(object sender, EventArgs e)
        {
            StringBuilder sb;

            User user;
            switch (GetUserType())
            {
                case User.UserTypeList.Admin:
                    user = TRMWCFWebServiceJson.GetAdmin(Convert.ToInt32(txtWordpressId.Text));
                    sb = GetUserDetailsForDisplay(user);
                    break;
                case User.UserTypeList.Artist:
                    user = TRMWCFWebServiceJson.GetArtist(Convert.ToInt32(txtWordpressId.Text));
                    sb = GetArtistDetailsForDisplay((Artist)user);
                    break;
                case User.UserTypeList.Business:
                    //user = TRMWCFWebServiceJson.GetBusiness(Convert.ToInt32(txtWordpressId.Text));
                    //sb = GetBusinessDetailsForDisplay((BusinessUser)user);
                    sb = null;
                    break;
                default:
                    user = TRMWCFWebServiceJson.GetCustomer(Convert.ToInt32(txtWordpressId.Text));
                    sb = GetUserDetailsForDisplay(user);
                    break;
            }

            txtResult.Text = sb.ToString();
        }

        private void cbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;

            var userType = comboBox.SelectedItem.ToString();

            if (userType.Equals("Artist"))
            {
                lbGenres.Visible = true;
                cbBusinessType.Visible = false;
            }
            else if (userType.Equals("Business"))
            {
                cbBusinessType.Visible = true;
                lbGenres.Visible = false;
            }
            else
            {
                lbGenres.Visible = false;
                cbBusinessType.Visible = false;
            }
        }

        #region private methods

        private void InitializeBusinessTypes()
        {
            cbBusinessType.Items.Clear();

            var businessTypeCollection = TRMWCFWebServiceJson.GetAllBusinessTypes();

            object[] businessTypes = new ListItem[businessTypeCollection.Count()];
            var i = 0;

            foreach (var businessType in businessTypeCollection)
            {
                businessTypes[i] = new ListItem(businessType.BusinessTypeName, businessType.BusinessTypeId);
                i++;
            }

            cbBusinessType.Items.AddRange(businessTypes);
        }

        private User.UserTypeList GetUserType()
        {
            switch (cbUserType.SelectedItem.ToString())
            {
                case "Admin":
                    return User.UserTypeList.Admin;
                case "Artist":
                    return User.UserTypeList.Artist;
                case "Business":
                    return User.UserTypeList.Business;
                default: // customer
                    return User.UserTypeList.Customer;
            }
        }

        private StringBuilder GetUserDetailsForDisplay(User user)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"User ID: " + user.UserId.ToString(CultureInfo.InvariantCulture));
            //sb.AppendLine(@"Wordpress ID: " + user.WordpressUserId.ToString(CultureInfo.InvariantCulture));
            //sb.AppendLine(@"Created Date: " + user.CreatedDate.ToString(CultureInfo.InvariantCulture));

            return sb;
        }

        private StringBuilder GetArtistDetailsForDisplay(Artist artist)
        {
            var sb = GetUserDetailsForDisplay(artist);

            return sb;
        }

        private StringBuilder GetBusinessDetailsForDisplay(BusinessUser business)
        {
            //var sb = GetUserDetailsForDisplay(business);
            var sb = new StringBuilder();

            return sb;
        }

        #endregion

        #endregion

        #region Album section

        private void btnUploadAlbum_Click(object sender, EventArgs e)
        {
            ClearAlbumsResultTextBoxes();

            var artist = TRMWCFWebServiceJson.GetArtist(Convert.ToInt32(TxtAlbumWordpressId.Text));
            var albumGenreCollection = (from ListItem listItem in lbAlbumGenres.SelectedItems select TRMWCFWebServiceJson.GetGenre(listItem.SelectedValue())).ToList();

            var album = new Album()
            {
                AlbumLabel = txtAlbumLabel.Text,
                AlbumProducer = txtAlbumProducer.Text,
                AlbumReleaseDate = Convert.ToDateTime(dtpAlbumReleaseDate.Text),
                AlbumTitle = txtAlbumTitle.Text,
                CreatedDate = DateTime.Now,
                GenreCollection = albumGenreCollection
            };

            if (TRMWCFWebServiceJson.SaveArtistAlbum(album, artist, null))
            {
                txtAlbumResult.Text = album.AlbumTitle + @" has been saved successfully!";
            }
            else
            {
                txtAlbumResult.Text = @"There was a problem saving the album: " + album.AlbumTitle;
            }
        }

        private void btnGetAlbums_Click(object sender, EventArgs e)
        {
            ClearAlbumsResultTextBoxes();
            InitializeAlbums(Convert.ToInt32(TxtAlbumWordpressId.Text));
        }

        private void btnGetAlbumDetails_Click(object sender, EventArgs e)
        {
            ClearAlbumsResultTextBoxes();

            var sb = new StringBuilder();
            var album = (from ListItem listItem in lbArtistAlbums.SelectedItems select TRMWCFWebServiceJson.GetArtistAlbumDetails(listItem.SelectedValue())).FirstOrDefault();

            sb.AppendLine("Title: " + album.AlbumTitle);
            sb.AppendLine("Producer: " + album.AlbumProducer);
            sb.AppendLine("Label: " + album.AlbumLabel);
            sb.AppendLine("Release Date: " + album.AlbumReleaseDate.ToShortDateString());
            sb.AppendLine("Date Added: " + album.CreatedDate.ToShortDateString());

            if (album.GenreCollection != null)
            {
                sb.AppendLine("Genres: ");

                foreach (var genre in album.GenreCollection)
                {
                    sb.AppendLine(genre.GenreName);
                }
            }

            txtAlbumDetails.Text = sb.ToString();
        }

        private void btnDeleteAlbums_Click(object sender, EventArgs e)
        {
            ClearAlbumsResultTextBoxes();

            var artist = TRMWCFWebServiceJson.GetArtist(Convert.ToInt32(TxtAlbumWordpressId.Text));
            var album = (from ListItem listItem in lbArtistAlbums.SelectedItems select TRMWCFWebServiceJson.GetArtistAlbumDetails(listItem.SelectedValue())).FirstOrDefault();

            if (TRMWCFWebServiceJson.DeleteArtistAlbum(album, artist))
            {
                txtAlbumResult.Text = album.AlbumTitle + @" has been deleted successfully!";
            }
        }

        #region private methods

        private List<Album> GetArtistAlbumCollection(int wordpressId)
        {
            var artist = TRMWCFWebServiceJson.GetArtist(wordpressId);
            var artistAlbumCollection = TRMWCFWebServiceJson.GetArtistAlbums(artist).ToList();

            return artistAlbumCollection;
        }

        private void ClearAlbumsResultTextBoxes()
        {
            txtAlbumResult.Text = string.Empty;
            txtAlbumDetails.Text = string.Empty;
        }

        #endregion

        #endregion

        #region Song section

        private void btnGetSongAlbums_Click(object sender, EventArgs e)
        {
            ClearSongsResultTextBoxes();
            InitializeAlbums(Convert.ToInt32(txtSongWordpressId.Text));
        }

        private void btnSelectSongFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            lblSelectedFile.Text = ofd.FileName;
        }

        private void btnSaveSong_Click(object sender, EventArgs e)
        {
            var util = new Utilities();
            var songGenreCollection = (from ListItem listItem in lbSongGenres.SelectedItems select TRMWCFWebServiceJson.GetGenre(listItem.SelectedValue())).ToList();
            var albumCollection = (from ListItem listItem in lbSongAlbums.SelectedItems select TRMWCFWebServiceJson.GetArtistAlbumDetails(listItem.SelectedValue())).ToList();

            var song = new Song()
            {
                AlbumCollection = albumCollection,
                CreatedDate = DateTime.Now,
                GenreCollection = songGenreCollection,
                SongComposer = txtSongComposer.Text,
                SongReleaseDate = Convert.ToDateTime(dtpSongReleaseDate.Text),
                SongTitle = txtSongTitle.Text
            };

            foreach (var album in albumCollection)
            {
                if (TRMWCFWebServiceJson.UploadSong(song, lblSelectedFile.Text, "mp3", txtSongWordpressId.Text + "/" + util.RemoveSpaces(album.AlbumTitle) + "/"))
                {
                    txtSongsResults.Text = song.SongTitle + @" has been saved successfully!";
                }
                else
                {
                    txtSongsResults.Text = @"There was a problem saving the song: " + song.SongTitle;
                }
            }
        }

        private void ClearSongsResultTextBoxes()
        {
            txtSongsResults.Text = string.Empty;
            lbSongAlbums.Items.Clear();
        }

        #endregion

        #region Private methods

        private void InitializeGenres()
        {
            lbGenres.Items.Clear();
            lbAlbumGenres.Items.Clear();

            var genreCollection = TRMWCFWebServiceJson.GetAllGenres();
            object[] genres = new ListItem[genreCollection.Count()];
            var i = 0;

            foreach (var genre in genreCollection)
            {
                genres[i] = new ListItem(genre.GenreName, genre.GenreId);
                i++;
            }

            lbGenres.Items.AddRange(genres);
            lbAlbumGenres.Items.AddRange(genres);
            lbSongGenres.Items.AddRange(genres);
        }

        private void InitializeAlbums(int wordpressId)
        {
            lbArtistAlbums.Items.Clear();

            var albumCollection = GetArtistAlbumCollection(wordpressId);
            object[] albums = new ListItem[albumCollection.Count];
            var i = 0;

            foreach (var album in albumCollection)
            {
                albums[i] = new ListItem(album.AlbumTitle, album.AlbumId);
                i++;
            }

            lbArtistAlbums.Items.AddRange(albums);
            lbSongAlbums.Items.AddRange(albums);
        }

        #endregion
    }
}