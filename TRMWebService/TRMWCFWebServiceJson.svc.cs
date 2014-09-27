using System.Web.Security;
using DomainModel.Concrete;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Transactions;
using TRMInfrastructure.Delivery;
using TRMInfrastructure.Media.FFMpeg;
using TRMInfrastructure.Media.MediaInfo;
using TRMInfrastructure.Utilities;
using TRMWebService.Classes;
using WebMatrix.WebData;
using System.Web;
using Microsoft.Web.WebPages.OAuth;

namespace TRMWebService
{
    [ServiceBehavior(TransactionIsolationLevel = System.Transactions.IsolationLevel.Serializable, TransactionTimeout = "00:02:00")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TRMWCFWebServiceJson : TRMBaseClass, ITRMWCFWebServiceJson
    {
        #region Private members
        private readonly SqlAccountRepository SqlAccountRepository;
        private readonly SqlAlbumRepository SqlAlbumRepository;
        private readonly SqlAlbumGenreRepository SqlAlbumGenreRepository;
        private readonly SqlAlbumSongRepository SqlAlbumSongRepository;
        private readonly SqlArtistRepository SqlArtistRepository;
        private readonly SqlArtistAlbumRepository SqlArtistAlbumRepository;
        private readonly SqlArtistGenreRepository SqlArtistGenreRepository;
        private readonly SqlBusinessTypeRepository SqlBusinessTypeRepository;
        private readonly SqlBusinessUserRepository SqlBusinessUserRepository;
        private readonly SqlGenreRepository SqlGenreRepository;
        private readonly SqlMediaAssetFormatRepository SqlMediaAssetFormatRepository;
        private readonly SqlMediaAssetLocationRepository SqlMediaAssetLocationRepository;
        private readonly SqlMediaAssetRepository SqlMediaAssetRepository;
        private readonly SqlMediaAssetTypeRepository SqlMediaAssetTypeRepository;
        private readonly SqlPlaylistRepository SqlPlaylistRepository;
        private readonly SqlPlaylistSongRepository SqlPlaylistSongRepository;
        private readonly SqlProtocolRepository SqlProtocolRepository;
        private readonly SqlPurchasedSongRepository SqlPurchasedSongRepository;
        private readonly SqlSongRepository SqlSongRepository;
        private readonly SqlSongGenreRepository SqlSongGenreRepository;
        private readonly SqlSongMediaAssetRepository SqlSongMediaAssetRepository;
        private readonly SqlUserRepository SqlUserRepository;
        private readonly SqlUserPlaylistRepository SqlUserPlaylistRepository;

        private string DestinationFile;
        private Utilities util = new Utilities();

        private enum ProtocolList
        {
            HTTP = 1,
            FTP = 2,
            UNC = 3,
            FileSystem = 4,
            S3 = 5
        }
        #endregion

        #region Constructors
        public TRMWCFWebServiceJson()
        {
            SqlAccountRepository = new SqlAccountRepository(ConnectionString);
            SqlAlbumRepository = new SqlAlbumRepository(ConnectionString);
            SqlAlbumGenreRepository = new SqlAlbumGenreRepository(ConnectionString);
            SqlAlbumSongRepository = new SqlAlbumSongRepository(ConnectionString);
            SqlArtistRepository = new SqlArtistRepository(ConnectionString);
            SqlArtistAlbumRepository = new SqlArtistAlbumRepository(ConnectionString);
            SqlArtistGenreRepository = new SqlArtistGenreRepository(ConnectionString);
            SqlBusinessTypeRepository = new SqlBusinessTypeRepository(ConnectionString);
            SqlBusinessUserRepository = new SqlBusinessUserRepository(ConnectionString);
            SqlGenreRepository = new SqlGenreRepository(ConnectionString);
            SqlMediaAssetFormatRepository = new SqlMediaAssetFormatRepository(ConnectionString);
            SqlMediaAssetLocationRepository = new SqlMediaAssetLocationRepository(ConnectionString);
            SqlMediaAssetRepository = new SqlMediaAssetRepository(ConnectionString);
            SqlMediaAssetTypeRepository = new SqlMediaAssetTypeRepository(ConnectionString);
            SqlPlaylistRepository = new SqlPlaylistRepository(ConnectionString);
            SqlPlaylistSongRepository = new SqlPlaylistSongRepository(ConnectionString);
            SqlProtocolRepository = new SqlProtocolRepository(ConnectionString);
            SqlPurchasedSongRepository = new SqlPurchasedSongRepository(ConnectionString);
            SqlSongRepository = new SqlSongRepository(ConnectionString);
            SqlSongGenreRepository = new SqlSongGenreRepository(ConnectionString);
            SqlSongMediaAssetRepository = new SqlSongMediaAssetRepository(ConnectionString);
            SqlUserRepository = new SqlUserRepository(ConnectionString);
            SqlUserPlaylistRepository = new SqlUserPlaylistRepository(ConnectionString);
        }
        #endregion

        #region Operations

        #region Account operations

        public bool SaveUserAccount(User user, Account.AccountTypeList accountType)
        {
            var account = new Account()
            {
                AccountType = accountType,
                AccountTypeId = (int)accountType,
                Active = true,
                CreatedDate = DateTime.Now,
                Credits = (user.UserType == User.UserTypeList.Admin) ? 9999 : 0,
                UserId = user.UserId
            };

            try
            {
                return SqlAccountRepository.SaveAccount(account);
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
                throw;
            }
        }

        public Account GetUserAccount(User user)
        {
            return SqlAccountRepository.Account.FirstOrDefault(account => account.UserId == user.UserId);
        }

        public bool DeleteAccount(User user)
        {
            return SqlAccountRepository.DeleteAccount(GetUserAccount(user));
        }

        #endregion

        #region Artist operations

        public List<Artist> GetAllArtists()
        {
            var userCollection = SqlArtistRepository.Artist.ToList();

            return userCollection.Select(artist => new Artist
            {
                ArtistId = artist.ArtistId,
                ArtistName = artist.ArtistName,
                Email = artist.Email,
                Facebook = artist.Facebook,
                MySpace = artist.MySpace,
                ProfileImage = artist.ProfileImage,
                SoundCloud = artist.SoundCloud,
                Twitter = artist.Twitter,
                UserName = artist.UserName,
                UserId = artist.UserId,
                UserType = artist.UserType,
                PRS = artist.PRS,
                CreativeCommonsLicence = artist.CreativeCommonsLicence,
                Active = artist.Active,
                AlbumCollection = GetArtistAlbumCollection(artist.UserId),
                GenreCollection = GetArtistGenreCollection(artist.UserId),
                SongCollection = GetArtistSongCollection(GetArtistAlbumCollection(artist.UserId))
            }).ToList();
        }

        public Artist GetArtistFromAlbumId(int albumId)
        {
            var artistAlbum = SqlArtistAlbumRepository.ArtistAlbum.Where(x => x.AlbumId == albumId).FirstOrDefault();
            return SqlArtistRepository.GetArtistByUserId(artistAlbum.UserId);
        }

        public List<Event> GetArtistEvents(int userId)
        {
            // TODO
            return null;
        }

        #region Album section

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool SaveArtistAlbum(Album album, Artist artist, string sourceFile)
        {
            using (var tranScope = new TransactionScope())
            {
                try
                {
                    var albumId = SqlAlbumRepository.SaveAlbum(album);

                    if (albumId > 0)
                    {
                        if (SaveAlbumGenreCollection(albumId, album.GenreCollection))
                        {
                            var util = new Utilities();
                            var artistAlbum = new ArtistAlbum()
                                 {
                                     AlbumId = albumId,
                                     UserId = artist.UserId
                                 };

                            if (UploadFileToS3(sourceFile, util.RemoveSpaces(artist.ArtistName) + "/" + util.RemoveSpaces(album.AlbumTitle) + "/", "stream"))
                            {
                                if (SqlArtistAlbumRepository.SaveArtistAlbum(artistAlbum))
                                {
                                    tranScope.Complete();
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public List<Album> GetAllAlbums()
        {
            return SqlAlbumRepository.GetAllAlbums();
        }

        public List<Album> GetArtistAlbums(Artist artist)
        {
            var artistAlbumCollection = SqlArtistAlbumRepository.ArtistAlbum.Where(x => x.UserId == artist.UserId).ToList();
            var albumCollection = artistAlbumCollection.Select(album => SqlAlbumRepository.Album.FirstOrDefault(x => x.AlbumId == album.AlbumId)).ToList();

            foreach (var album in albumCollection)
            {
                var albumGenreCollection = SqlAlbumGenreRepository.AlbumGenre.Where(x => album != null && x.AlbumId == album.AlbumId);
                foreach (var genre in albumGenreCollection)
                {
                    album.GenreCollection = SqlGenreRepository.Genre.Where(x => x.GenreId == genre.GenreId).ToList();
                }
            }

            return albumCollection;
        }

        public Album GetArtistAlbumDetails(int albumId)
        {
            var album = SqlAlbumRepository.Album.FirstOrDefault(x => x.AlbumId == albumId);
            album.GenreCollection = new List<Genre>();
            var albumGenreCollection = SqlAlbumGenreRepository.AlbumGenre.Where(x => album != null && x.AlbumId == album.AlbumId).ToList();

            foreach (var genre in albumGenreCollection.Where(genre => album != null))
            {
                album.GenreCollection.Add(SqlGenreRepository.Genre.FirstOrDefault(x => x.GenreId == genre.GenreId));
            }

            return album;
        }

        public Album GetAlbumFromSongId(int songId)
        {
            var albumSong = SqlAlbumSongRepository.GetAlbumSongsBySongId(songId).FirstOrDefault();
            return SqlAlbumRepository.GetAlbumById(albumSong.AlbumId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool DeleteArtistAlbum(Album album, Artist artist)
        {
            using (var tranScope = new TransactionScope())
            {
                var artistAlbum =
                    SqlArtistAlbumRepository.ArtistAlbum.FirstOrDefault(
                        x => x.AlbumId == album.AlbumId && x.UserId == artist.UserId);

                try
                {
                    if (SqlArtistAlbumRepository.DeleteArtistAlbum(artistAlbum))
                    {
                        if (SqlAlbumRepository.DeleteAlbum(album))
                        {
                            tranScope.Complete();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool UpdateAlbumGenreCollection(int albumId, List<Genre> genreCollection)
        {
            var isUpdated = true;

            // start by deleting all the genres before inserting the new list of genres that should contain the current ones
            var deleteGenreCollection = SqlAlbumGenreRepository.DeleteAllAlbumGenreByAlbum(albumId);

            if (deleteGenreCollection)
            {
                if ((from genre in genreCollection let tempGenre = SqlAlbumGenreRepository.AlbumGenre.FirstOrDefault(x => x.GenreId == genre.GenreId) where tempGenre == null select genre).Any(genre => !SqlAlbumGenreRepository.SaveAlbumGenre(new AlbumGenre()
                {
                    AlbumId = albumId,
                    GenreId = genre.GenreId
                })))
                {
                    return false;
                }
            }
            else
            {
                isUpdated = false;
            }

            return isUpdated;
        }

        #endregion

        #region Song section
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool UploadSong(Song song, string filePath, string ext, string folderName)
        {
            var isUploaded = false;

            // save the file locally and create the aac version for streaming then upload it to the S3 bucket
            if (UploadSongToAlbumFolder(song, filePath, folderName, ext))
            {
                using (var tranScope = new TransactionScope())
                {
                    try
                    {
                        //create the song entry
                        var songId = SqlSongRepository.SaveSong(song);

                        if (songId > 0)
                        {
                            // link the song and the albums
                            if (SaveSongAlbums(song))
                            {
                                // save the song genre collection
                                if (SaveSongGenreCollection(songId, song.GenreCollection))
                                {
                                    // create the media asset entry
                                    var mediaAssetFormat = SqlMediaAssetFormatRepository.MediaAssetFormat.FirstOrDefault(x => x.MediaAssetFormatName.Equals(ext));
                                    if (mediaAssetFormat != null)
                                    {
                                        var mediaAssetId = SaveMediaAsset(mediaAssetFormat);

                                        if (mediaAssetId > 0)
                                        {
                                            // save the location of the media asset in S3
                                            if (SaveMediaAssetLocation(mediaAssetId, folderName))
                                            {
                                                // now link the song and the media file
                                                if (SaveSongMediaAsset(songId, mediaAssetId))
                                                {
                                                    tranScope.Complete();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        util.ErrorNotification(ex);
                        isUploaded = false;
                    }
                }

                // save the master file
                isUploaded = SaveMasterFile(song, filePath, folderName);
            }

            return isUploaded;
        }

        public string DownloadSongOnServer(int songId)
        {
            var isDownloaded = false;
            var localFile = LocalDownloadPath;

            var songMediaAsset = SqlSongMediaAssetRepository.SongMediaAsset.FirstOrDefault(song => song.SongId == songId);
            var mediaAsset = SqlMediaAssetRepository.MediaAsset.FirstOrDefault(x => x.MediaAssetId == songMediaAsset.MediaAssetId);
            var mediaAssetLocation = SqlMediaAssetLocationRepository.MediaAssetLocation.Where(x => x.ProtocolId == 6).FirstOrDefault(x => x.MediaAssetId == mediaAsset.MediaAssetId);

            if (mediaAssetLocation != null)
            {
                isDownloaded = File.Exists(Path.Combine(localFile, mediaAssetLocation.Path)) || DownloadAudioFileFromS3(localFile, mediaAssetLocation.Path);
            }

            return isDownloaded ? mediaAssetLocation.Path : "error";
        }

        public Stream DownloadFile(string fileName)
        {
            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
            var f = new FileStream(Path.Combine(LocalDownloadPath, fileName), FileMode.Open);

            var length = (int)f.Length;
            WebOperationContext.Current.OutgoingResponse.ContentLength = length;

            var buffer = new byte[length];
            var sum = 0;
            int count;

            while ((count = f.Read(buffer, sum, length - sum)) > 0)
            {
                sum += count;
            }

            f.Close();

            return new MemoryStream(buffer);
        }

        public List<Song> GetAllSongs()
        {
            return SqlSongRepository.Song.ToList();
        }

        public List<Song> GetAlbumSongs(int albumId)
        {
            var songCollection = new List<Song>();
            var albumSongCollection = SqlAlbumSongRepository.AlbumSong.Where(x => x.AlbumId == albumId).ToList();

            songCollection.AddRange(albumSongCollection.Select(song => SqlSongRepository.GetSongById(song.SongId)));

            foreach (var song in songCollection)
            {
                song.songMediaAsset = GetMediaAssetCollection(song.SongId);
                song.GenreCollection = GetSongGenreCollection(song.SongId);
            }

            return songCollection;
        }

        public List<Song> GetArtistSongs(int userId)
        {
            var songCollection = new List<Song>();

            var artistAlbumCollection = SqlArtistAlbumRepository.ArtistAlbum.Where(x => x.UserId == userId).ToList();
            var albumCollection = artistAlbumCollection.Select(album => SqlAlbumRepository.Album.FirstOrDefault(x => x.AlbumId == album.AlbumId)).ToList();

            foreach (var albumSongCollection in albumCollection.Select(album => SqlAlbumSongRepository.GetAlbumSongsByAlbumId(album.AlbumId).ToList()))
            {
                songCollection.AddRange(albumSongCollection.Select(song => SqlSongRepository.GetSongById(song.SongId)));
            }

            return songCollection;
        }

        public Song GetArtistSongDetails(int songId)
        {
            var artistSong = SqlSongRepository.Song.FirstOrDefault(song => song.SongId == songId);

            return artistSong;
        }

        public List<MediaAsset> GetSongMediaAssets(int songId)
        {
            return GetMediaAssetCollection(songId);
        }

        public List<MediaAsset> GetSongMediaAssetsForDownload(int songId)
        {
            var mediaAssetCollection = GetMediaAssetCollection(songId);

            foreach (var mediaAsset in mediaAssetCollection.Where(mediaAsset => mediaAsset.MediaAssetLocation != null))
            {
                mediaAsset.MediaAssetLocation.Path = DownloadSongOnServer(songId);
            }

            return mediaAssetCollection;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool DeleteSong(Song song, Album album)
        {
            using (var tranScope = new TransactionScope())
            {
                try
                {
                    if (DeleteAlbumSong(song, album))
                    {
                        var songMediaAsset = SqlSongMediaAssetRepository.SongMediaAsset.FirstOrDefault(x => x.SongId == song.SongId);
                        if (SqlSongMediaAssetRepository.DeleteSongMediaAsset(songMediaAsset))
                        {
                            if (SqlMediaAssetRepository.DeleteMediaAsset(SqlMediaAssetRepository.MediaAsset.FirstOrDefault(x => x.MediaAssetId == songMediaAsset.MediaAssetId)))
                            {
                                if (SqlSongRepository.DeleteSong(song))
                                {
                                    tranScope.Complete();
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteAlbumSong(Song song, Album album)
        {
            var albumSong =
                SqlAlbumSongRepository.AlbumSong.FirstOrDefault(
                    x => x.SongId == song.SongId && x.AlbumId == album.AlbumId);

            return SqlAlbumSongRepository.DeleteAlbumSong(albumSong);
        }

        #endregion

        #endregion

        #region Business operations

        public List<Business> GetAllBusinesses()
        {
            var userCollection = SqlUserRepository.User.ToList();

            return userCollection.Where(user => Roles.IsUserInRole(user.UserName, User.UserTypeList.Business.ToString()))
            .Select(user => new Business
                {
                    //CreatedDate = user.CreatedDate,
                    UserId = user.UserId,
                    UserType = user.UserType,
                    //UserTypeId = user.UserTypeId,
                    //WordpressUserId = user.WordpressUserId,
                    PlaylistCollection = GetBusinessPlaylists(user.UserId),
                    BusinessType = GetBusinessType(user.UserId),
                    BusinessUser = GetBusinessUserObject(user.UserId)
                }).ToList();
        }

        #region Playlist section

        public Playlist GetPlaylistByName(string playlistName)
        {
            var playlist = SqlPlaylistRepository.Playlist.Where(p => p.PlaylistName == playlistName).FirstOrDefault();

            if (playlist != null)
            {
                playlist.PlaylistSongCollection = GetPlaylistSongCollection(playlist.PlaylistId);
                return playlist;
            }

            return new Playlist();
        }

        public Playlist GetPlaylistById(int playlistId)
        {
            var playlist = SqlPlaylistRepository.Playlist.Where(p => p.PlaylistId == playlistId).FirstOrDefault();
            if (playlist != null)
            {
                playlist.PlaylistSongCollection = GetPlaylistSongCollection(playlistId);
            }
            else
            {
                playlist = new Playlist();
                playlist.PlaylistSongCollection = new List<PlaylistSong>();
            }

            return playlist;
        }

        public List<Playlist> GetPlaylistsByUserId(int userId)
        {
            var userPlaylists = SqlUserPlaylistRepository.GetUserPlaylistsByUserId(userId);
            var userPlaylistCollection = new List<Playlist>();

            foreach (var userPlaylist in userPlaylists)
            {
                userPlaylistCollection.Add(SqlPlaylistRepository.GetPlaylistById(userPlaylist.PlaylistId));
            }

            return userPlaylistCollection;
        }

        public List<UserPlaylist> GetUserPlaylistsByUserId(int userId)
        {
            return SqlUserPlaylistRepository.GetUserPlaylistsByUserId(userId);
        }

        public List<Song> GetAllPlaylistSongs(int playlistId)
        {
            var songCollection = new List<Song>();

            foreach(var playlistSong in SqlPlaylistSongRepository.GetSongsByPlaylistId(playlistId)){
                songCollection.Add(SqlSongRepository.Song.Where(x => x.SongId == playlistSong.SongId).FirstOrDefault());
            }

            return songCollection;
        }

        public List<PlaylistSong> GetPlaylistSongCollection(int playlistId)
        {
            return SqlPlaylistSongRepository.GetSongsByPlaylistId(playlistId);
        }        

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public int SavePlaylist(int userId, Playlist playlist)
        {
            using (var tranScope = new TransactionScope())
            {
                try
                {
                    var playlistId = playlist.PlaylistId;
                    var newPlaylist = (playlistId == 0);

                    if (newPlaylist)
                    {
                        playlistId = SqlPlaylistRepository.SavePlaylist(playlist);
                    }

                    if (playlist.PlaylistSongCollection.Count > 0 && SavePlaylistSongs(UpdatePlaylistSongsPlaylistId(playlist.PlaylistId, playlist.PlaylistSongCollection)))
                    {
                        if (SaveUserPlaylist(userId, playlistId, newPlaylist))
                        {
                            tranScope.Complete();

                            return playlistId;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch
                {
                    return -1;
                }
            }

            return -1;
        }

        public bool DeletePlaylist(int playlistId)
        {
            try
            {
                return SqlPlaylistRepository.DeletePlaylist(SqlPlaylistRepository.Playlist.FirstOrDefault(x => x.PlaylistId == playlistId));
            }
            catch
            {
                return false;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool DeletePlaylistSong(int songId, int playlistId, int position)
        {
            var valid = false;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    valid = SqlPlaylistSongRepository.DeletePlaylistSong(SqlPlaylistSongRepository.PlaylistSong.FirstOrDefault(x => x.PlaylistId == playlistId && x.SongId == songId));
                    valid = UpdatePlaylistSongsPosition(playlistId, position);

                    if (valid)
                    {
                        tranScope.Complete();
                    }
                }
                catch
                {
                    return false;
                }
            }

            return valid;
        }

        #endregion

        #endregion

        #region Business type operations

        public List<BusinessType> GetAllBusinessTypes()
        {
            return SqlBusinessTypeRepository.BusinessType.ToList();
        }

        #endregion

        #region Genre operations

        public List<Genre> GetAllGenres()
        {
            return SqlGenreRepository.Genre.ToList();
        }

        public Genre GetGenre(int genreId)
        {
            return SqlGenreRepository.Genre.FirstOrDefault(x => x.GenreId == genreId);
        }

        #endregion

        #region User operations
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool RegisterAdmin(User user)
        {
            bool isRegistered;
            int userId;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    // if the user is saved successfully it will return a userId which is always greater than 0
                    userId = SqlUserRepository.SaveUser(user);
                    // now create an account for this user
                    isRegistered = SaveUserAccount(user, Account.AccountTypeList.admin);

                    tranScope.Complete();
                }
                catch
                {
                    return false;
                }
            }

            return (userId > 0 && isRegistered);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool RegisterArtist(Artist artist, List<Genre> genreCollection, HttpPostedFileBase sourceFile)
        {
            var util = new Utilities();
            bool isRegistered;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    // if the user is saved successfully it will return a userId which is always greater than 0
                    WebSecurity.CreateUserAndAccount(artist.UserName, artist.Password);
                    Roles.AddUserToRole(artist.UserName, artist.UserType.ToString());
                    artist.UserId = WebSecurity.GetUserId(artist.UserName);

                    // first save an artist instance of this user
                    isRegistered = SaveArtist(artist);

                    if (isRegistered)
                    {
                        // now create an account for this user
                        isRegistered = SaveUserAccount(artist, Account.AccountTypeList.artist);

                        foreach (var genre in genreCollection)
                        {
                            isRegistered = SqlArtistGenreRepository.SaveArtistGenre(new ArtistGenre()
                                {
                                    UserId = artist.UserId,
                                    GenreId = genre.GenreId
                                });


                            if (!isRegistered)
                            {
                                return false;
                            }
                        }

                        // save file locally to upload it
                        if (!UploadFileToS3(SaveFileLocally(sourceFile), util.RemoveSpaces(artist.ArtistName) + "/", "stream"))
                        {
                            return false;
                        }

                        tranScope.Complete();
                    }
                }
                catch (MembershipCreateUserException ex)
                {
                    util.ErrorNotification(ex);
                    throw;
                }
                catch (Exception ex)
                {
                    util.ErrorNotification(ex);
                    throw;
                }
            }

            return (artist.UserId > 0 && isRegistered);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool RegisterArtistSocial(Artist artist, string provider, string providerUserId)
        {
            var util = new Utilities();
            bool isRegistered;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, artist.UserName);
                    Roles.AddUserToRole(artist.UserName, "Artist");

                    // if the user is saved successfully it will return a userId which is always greater than 0
                    artist.UserId = WebSecurity.GetUserId(artist.UserName);

                    // save an artist instance of this user
                    isRegistered = SaveArtist(artist);

                    tranScope.Complete();
                }
                catch (MembershipCreateUserException ex)
                {
                    util.ErrorNotification(ex);
                    throw;
                }
                catch (Exception ex)
                {
                    util.ErrorNotification(ex);
                    throw;
                }
            }

            return (artist.UserId > 0 && isRegistered);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool RegisterBusiness(User user, BusinessType businessType, string latitude, string longitude)
        {
            bool isRegistered;
            int userId;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    // if the user is saved successfully it will return a userId which is always greater than 0
                    userId = SqlUserRepository.SaveUser(user);
                    // now create an account for this user
                    isRegistered = SaveUserAccount(user, Account.AccountTypeList.business);

                    // create the business object and insert it
                    var businessUser = new BusinessUser()
                        {
                            BusinessTypeId = businessType.BusinessTypeId,
                            CreatedDate = DateTime.Now,
                            UserId = userId
                        };

                    if (!SqlBusinessUserRepository.SaveBusinessUser(businessUser))
                    {
                        return false;
                    }

                    tranScope.Complete();
                }
                catch
                {
                    return false;
                }
            }

            return (userId > 0 && isRegistered);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool RegisterCustomer(User user)
        {
            bool isRegistered;
            int userId;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    // if the register is saved successfully it will return the userId which is always greater than 0
                    userId = SqlUserRepository.SaveUser(user);
                    // now create an account for this user
                    isRegistered = SaveUserAccount(user, Account.AccountTypeList.customer);

                    tranScope.Complete();
                }
                catch
                {
                    return false;
                }
            }

            return (userId > 0 && isRegistered);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool UpdateArtist(Artist artist, List<Genre> genreCollection, string sourceFile)
        {
            var util = new Utilities();
            bool isRegistered;

            using (var tranScope = new TransactionScope())
            {
                try
                {
                    // if the user is saved successfully it will return a userId which is always greater than 0
                    isRegistered = SaveArtist(artist);

                    if (isRegistered)
                    {
                        if (UpdateArtistGenreCollection(artist.UserId, genreCollection))
                        {
                            // if there is a file, save it locally to upload it
                            if (sourceFile != null && !string.IsNullOrEmpty(Path.GetExtension(sourceFile)))
                            {
                                if (!UploadFileToS3(sourceFile, util.RemoveSpaces(artist.ArtistName) + "/", "stream"))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }

                        tranScope.Complete();
                    }
                }
                catch
                {
                    return false;
                }
            }

            return (artist.UserId > 0 && isRegistered);
        }

        public bool UpdateBusinessUser(User user, BusinessType businessType, string latitude, string longitude)
        {
            // create the business object and insert it
            var businessUser = SqlBusinessUserRepository.GetBusinessUserByUserId(user.UserId);
            businessUser.BusinessTypeId = businessType.BusinessTypeId;

            return SqlBusinessUserRepository.SaveBusinessUser(businessUser);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public bool UpdateArtistGenreCollection(int userId, List<Genre> genreCollection)
        {
            var isUpdated = true;

            // start by deleting all the genres before inserting the new list of genres that should contain the current ones
            var deleteGenreCollection = SqlArtistGenreRepository.DeleteAllArtistGenreByArtist(userId);

            if (deleteGenreCollection)
            {
                foreach (var genre in genreCollection)
                {
                    var artistGenre = new ArtistGenre()
                    {
                        UserId = userId,
                        GenreId = genre.GenreId
                    };

                    if (!SqlArtistGenreRepository.SaveArtistGenre(artistGenre))
                    {
                        return false;
                    }
                }
            }
            else
            {
                isUpdated = false;
            }

            return isUpdated;
        }

        //public User GetUserByWordpressUserId(int wordpressUserId)
        //{
        //    return SqlUserRepository.GetUserByWordpressId(wordpressUserId);
        //}

        public User GetUserByUserId(int userId)
        {
            return SqlUserRepository.User.FirstOrDefault(user => user.UserId == userId);
        }

        public Admin GetAdmin(int userId)
        {
            var user = GetUserByUserId(userId);

            var admin = new Admin()
            {
                //CreatedDate = user.CreatedDate,
                UserId = user.UserId,
                UserName = user.UserName,
                UserType = user.UserType,
                //UserTypeId = user.UserTypeId,
                //WordpressUserId = user.WordpressUserId
            };

            return admin;
        }

        public Artist GetArtist(int userId)
        {
            var artist = SqlArtistRepository.GetArtistByUserId(userId);
            var albumCollection = GetArtistAlbumCollection(userId);

            artist.GenreCollection = GetArtistGenreCollection(userId);
            artist.AlbumCollection = albumCollection;
            artist.SongCollection = GetArtistSongCollection(albumCollection);

            return artist;
        }

        public Business GetBusiness(int userId)
        {
            var user = GetUserByUserId(userId);

            var businessUser = new Business
            {
                //CreatedDate = user.CreatedDate;
                UserId = user.UserId,
                UserType = user.UserType,
                UserName = user.UserName,
                //UserTypeId = user.UserTypeId;
                //WordpressUserId = user.WordpressUserId;

                PlaylistCollection = GetBusinessPlaylists(user.UserId),
                BusinessType = GetBusinessType(user.UserId),
                BusinessUser = GetBusinessUserObject(user.UserId)
            };

            return businessUser;
        }

        public Customer GetCustomer(int userId)
        {
            var user = GetUserByUserId(userId);
            var customer = new Customer()
            {
                //CreatedDate = user.CreatedDate,
                UserId = user.UserId,
                UserType = user.UserType,
                UserName = user.UserName
                //UserTypeId = user.UserTypeId,
                //WordpressUserId = user.WordpressUserId
            };

            return customer;
        }

        public bool DeleteUser(User user)
        {
            return SqlUserRepository.DeleteUser(user);
        }
        #endregion

        #region Song operations

        public bool RecordSongPlay(int songId, int playlistId)
        {
            var playlistSongId = GetPlaylistSongCollection(playlistId).Where(s => s.SongId == songId).Select(p => p.PlaylistSongId).FirstOrDefault();
            var purchasedSong = new PurchasedSong()
            {
                UserId = WebSecurity.CurrentUserId,
                Cost = 0,
                DatePurchased = DateTime.Now,
                PlaylistSongId = playlistSongId
            };

            try
            {
                var success = SqlPurchasedSongRepository.SavePurchasedSong(purchasedSong);

                return success;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region private methods

        #region account private methods



        #endregion

        #region album private methods

        private bool SaveAlbumGenreCollection(int albumId, IEnumerable<Genre> genreCollection)
        {
            // always delete the genres before inserting the updated list
            var isValid = DeleteAlbumGenreCollection(albumId);

            if (isValid)
            {
                foreach (var genre in genreCollection)
                {
                    isValid = SqlAlbumGenreRepository.SaveAlbumGenre(new AlbumGenre()
                        {
                            AlbumId = albumId,
                            GenreId = genre.GenreId
                        });

                    if (!isValid) break;
                }
            }

            return isValid;
        }

        private bool DeleteAlbumGenreCollection(int albumId)
        {
            var isValid = true;
            var genreCollection = SqlAlbumGenreRepository.GetAlbumGenresByAlbumId(albumId);

            foreach (var genre in genreCollection)
            {
                isValid = SqlAlbumGenreRepository.DeleteAlbumGenre(new AlbumGenre()
                    {
                        AlbumId = albumId,
                        GenreId = genre.GenreId
                    });

                if (!isValid) break;
            }

            return isValid;
        }

        #endregion

        #region artist private methods

        private bool SaveArtist(Artist artist)
        {
            try
            {
                return SqlArtistRepository.SaveArtist(artist);
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
                throw;
            }
        }

        private List<Genre> GetArtistGenreCollection(int userId)
        {
            var artistGenreRepository = new SqlArtistGenreRepository(ConnectionString);
            var genreRepository = new SqlGenreRepository(ConnectionString);

            var userGenres = artistGenreRepository.GetArtistGenresByArtistId(userId);
            var genres = userGenres.Select(userGenre => genreRepository.Genre.FirstOrDefault(x => x.GenreId == userGenre.GenreId)).ToList();

            return genres;
        }

        private List<Album> GetArtistAlbumCollection(int userId)
        {
            var artistAlbumRepository = new SqlArtistAlbumRepository(ConnectionString);
            var albumRepository = new SqlAlbumRepository(ConnectionString);

            var userAlbums = artistAlbumRepository.GetArtistAlbumsByArtistId(userId);
            var albums = userAlbums.Select(userAlbum => albumRepository.Album.FirstOrDefault(x => x.AlbumId == userAlbum.AlbumId)).ToList();

            return albums;
        }

        private List<Song> GetArtistSongCollection(IEnumerable<Album> albumCollection)
        {
            var albumSongRepository = new SqlAlbumSongRepository(ConnectionString);
            var songRepository = new SqlSongRepository(ConnectionString);

            var songCollection = new List<Song>();

            foreach (var albumSongs in albumCollection.Select(album => albumSongRepository.GetAlbumSongsByAlbumId(album.AlbumId)))
            {
                songCollection.AddRange(
                    albumSongs.Select(albumSong => songRepository.SongTable.FirstOrDefault(x => x.SongId == albumSong.SongId))
                              .ToList());
            }

            return songCollection;
        }

        private User.UserTypeList GetUserType(int userTypeId)
        {
            switch (userTypeId)
            {
                case 1:
                    return User.UserTypeList.Admin;
                case 2:
                    return User.UserTypeList.Artist;
                case 3:
                    return User.UserTypeList.Business;
                default:
                    return User.UserTypeList.Customer;
            }
        }

        #endregion

        #region business private methods

        private List<Playlist> GetBusinessPlaylists(int userId)
        {
            var businessPlaylistRepository = new SqlUserPlaylistRepository(ConnectionString);
            var playlistRepository = new SqlPlaylistRepository(ConnectionString);

            var businessPlaylists = businessPlaylistRepository.GetUserPlaylistsByUserId(userId);
            var playlists =
                businessPlaylists.Select(
                    businessPlaylist =>
                    playlistRepository.Playlist.FirstOrDefault(x => x.PlaylistId == businessPlaylist.PlaylistId))
                                 .ToList();

            return playlists;
        }

        private BusinessUser GetBusinessUserObject(int userId)
        {
            var businessUserRepository = new SqlBusinessUserRepository(ConnectionString);
            var businessUser = businessUserRepository.GetBusinessUserByUserId(userId);

            return businessUser;
        }

        private BusinessType GetBusinessType(int userId)
        {
            var businessTypeRepository = new SqlBusinessTypeRepository(ConnectionString);
            var businessUserRepository = new SqlBusinessUserRepository(ConnectionString);
            var businessType = businessTypeRepository.GetBusinessTypeById(businessUserRepository.GetBusinessUserByUserId(userId).BusinessTypeId);

            return businessType;
        }

        private bool UpdatePlaylistSongsPosition(int playlistId, int position){
            var valid = true;

            var maxposition = SqlPlaylistSongRepository.GetSongsByPlaylistId(playlistId).Select(x => x.Position).Max();
            var playlistSongs = SqlPlaylistSongRepository.GetSongsByPlaylistId(playlistId);

            if (maxposition > position)
            {
                foreach (var playlistSong in playlistSongs)
                {
                    if (playlistSong.Position > position)
                    {
                        playlistSong.Position = playlistSong.Position - 1;
                        try
                        {
                            valid = SqlPlaylistSongRepository.SavePlaylistSong(playlistSong);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }

            return valid;
        }        

        #endregion

        #region media asset methods

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        private int SaveMediaAsset(MediaAssetFormat mediaAssetFormat)
        {
            //var mediaFile = new MediaFile(DestinationFile);

            var mediaAsset = new MediaAsset()
            {
                CreatedDate = DateTime.Now,
                MediaAssetDuration = 0, //mediaFile.GetDuration(),
                MediaAssetFileName = Path.GetFileName(DestinationFile),
                MediaAssetFileSize = 0, //mediaFile.GetFileSize(),
                MediaAssetFormatId = mediaAssetFormat.MediaAssetFormatId,
                MediaAssetTypeId = 1 // audio
            };

            return SqlMediaAssetRepository.SaveMediaAsset(mediaAsset);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        private bool SaveMediaAssetLocation(int mediaAssetId, string folderName)
        {
            bool isUploaded;

            // create the media asset location entry
            var mediaAssetLocation = new MediaAssetLocation()
            {
                MediaAssetId = mediaAssetId,
                Path = folderName + Path.GetFileName(DestinationFile),
                ProtocolId = (int)ProtocolList.S3
            };

            try
            {
                isUploaded = SqlMediaAssetLocationRepository.SaveMediaAssetLocation(mediaAssetLocation);
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
                isUploaded = false;
            }

            return isUploaded;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        private bool SaveSongMediaAsset(int songId, int mediaAssetId)
        {
            return SqlSongMediaAssetRepository.SaveSongMediaAsset(new SongMediaAsset()
                {
                    MediaAssetId = mediaAssetId,
                    SongId = songId
                });
        }

        private List<MediaAsset> GetMediaAssetCollection(int songId)
        {
            var mediaAssetCollection = new List<MediaAsset>();

            var songMediaAssetCollection = SqlSongMediaAssetRepository.SongMediaAsset.Where(x => x.SongId == songId).ToList();
            foreach (var mediaAsset in songMediaAssetCollection.Select(songMediaAsset => SqlMediaAssetRepository.MediaAsset.FirstOrDefault(x => x.MediaAssetId == songMediaAsset.MediaAssetId)).Where(mediaAsset => mediaAsset != null))
            {
                mediaAsset.MediaAssetFormat = SqlMediaAssetFormatRepository.MediaAssetFormat.FirstOrDefault(x => x.MediaAssetFormatId == mediaAsset.MediaAssetFormatId);
                mediaAsset.MediaAssetType = SqlMediaAssetTypeRepository.MediaAssetType.FirstOrDefault(x => x.MediaAssetTypeId == mediaAsset.MediaAssetTypeId);
                mediaAsset.MediaAssetLocation = SqlMediaAssetLocationRepository.MediaAssetLocation.FirstOrDefault(location => location.MediaAssetId == mediaAsset.MediaAssetId);
                mediaAsset.MediaAssetLocation.Protocol = SqlProtocolRepository.Protocol.FirstOrDefault(p => p.ProtocolId == mediaAsset.MediaAssetLocation.ProtocolId);

                mediaAssetCollection.Add(mediaAsset);
            }

            return mediaAssetCollection;
        }

        #endregion

        #region playlist methods

        private List<PlaylistSong> UpdatePlaylistSongsPlaylistId(int playlistId, IEnumerable<PlaylistSong> playlistSongCollection)
        {
            foreach (var playlistSong in playlistSongCollection)
            {
                playlistSong.PlaylistId = playlistId;
            }

            return (List<PlaylistSong>)playlistSongCollection;
        }

        private bool SavePlaylistSongs(IEnumerable<PlaylistSong> playlistSongCollection)
        {
            var isSaved = true;

            foreach (var playlistSong in playlistSongCollection)
            {
                if (playlistSong.PlaylistSongId == 0)
                {
                    isSaved = SqlPlaylistSongRepository.SavePlaylistSong(playlistSong);

                    if (!isSaved)
                    {
                        return false;
                    }
                }
            }

            return isSaved;
        }

        private bool SaveUserPlaylist(int userId, int playlistId, bool newPlaylist)
        {
            if (!newPlaylist) return true;

            var isSaved = SqlUserPlaylistRepository.SaveUserPlaylist(new UserPlaylist
            {
                PlaylistId = playlistId,
                UserId = userId
            });

            return isSaved;
        }

        #endregion

        #region song private methods

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        private bool SaveMasterFile(Song song, string filePath, string folderName)
        {
            if (song.AlbumCollection.Any())
            {
                string ext = Path.GetExtension(filePath);
                DestinationFile = Path.Combine(Path.GetDirectoryName(filePath), String.Format("{0}{1}", ReplaceIllegalCharacters(song.SongTitle.ToUpper()), ext));
                try
                {
                    if (!File.Exists(DestinationFile))
                    {
                        File.Copy(filePath, DestinationFile);
                    }

                    using (var tranScope = new TransactionScope())
                    {

                        // create the required media assets entries
                        var mediaAssetFormat = SqlMediaAssetFormatRepository.MediaAssetFormat.FirstOrDefault(x => x.MediaAssetFormatName.Equals(ext.Remove(0, 1)));
                        if (mediaAssetFormat != null)
                        {
                            var mediaAssetId = SaveMediaAsset(mediaAssetFormat);

                            if (mediaAssetId > 0)
                            {
                                // save the location of the media asset in S3
                                if (SaveMediaAssetLocation(mediaAssetId, folderName))
                                {
                                    // now link the song and the media file
                                    if (SaveSongMediaAsset(song.SongId, mediaAssetId))
                                    {
                                        // upload the file to the S3 bucket album folder
                                        if (UploadFileToS3(DestinationFile, folderName, "master"))
                                        {
                                            tranScope.Complete();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var ex = new InvalidDataException("The file extension {} is invalid. Allowed file extensions: mp3, aac, mp4, m4a, wav, aiff");
                            util.ErrorNotification(ex);
                        }
                    }

                }
                catch (Exception ex)
                {
                    util.ErrorNotification(ex);
                }
            }

            return false;
        }

        private bool UploadSongToAlbumFolder(Song song, string filePath, string folderName, string ext)
        {
            if (song.AlbumCollection.Any())
            {
                DestinationFile = String.Format("{0}{1}", ReplaceIllegalCharacters(song.SongTitle.ToUpper()), Path.GetExtension(filePath));

                try
                {
                    // save the file locally and create the aac version for streaming then upload it to the S3 bucket album folder
                    return SaveUploadedAudioFileLocally(filePath, ext) && UploadFileToS3(DestinationFile, folderName, "stream");
                }
                catch (Exception ex)
                {
                    util.ErrorNotification(ex);
                }
            }

            return false;
        }

        private bool SaveUploadedAudioFileLocally(string sourceFile, string ext)
        {
            DestinationFile = Path.Combine(AudioDestinationPath, DestinationFile);

            try
            {
                // copy the source file locally
                if (File.Exists(sourceFile) && !File.Exists(DestinationFile))
                {
                    File.Copy(sourceFile, DestinationFile, true);
                }

                // encode the file
                if (!EncodeAudioFile(DestinationFile, ext))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private Album GetSongAlbum(Song song)
        {
            var albumSong = SqlAlbumSongRepository.AlbumSong.FirstOrDefault(x => x.SongId == song.SongId);

            return (albumSong != null) ? SqlAlbumRepository.GetAlbumById(albumSong.AlbumId) : null;
        }

        private bool EncodeAudioFile(string sourceFile, string ext)
        {
            DestinationFile = FFmpegController.EncodeAudio(sourceFile, ext, DestinationFile);

            return !DestinationFile.Equals("error");
        }

        private bool SaveSongAlbums(Song song)
        {
            var isSaved = false;

            foreach (var album in song.AlbumCollection)
            {
                isSaved = SqlAlbumSongRepository.SaveAlbumSong(new AlbumSong()
                    {
                        AlbumId = album.AlbumId,
                        SongId = song.SongId
                    });

                if (!isSaved)
                {
                    return false;
                }
            }

            return isSaved;
        }

        private bool SaveSongGenreCollection(int songId, IEnumerable<Genre> genreCollection)
        {
            // always delete the genres before inserting the updated list
            var isValid = DeleteSongGenreCollection(songId);

            if (isValid)
            {
                foreach (var genre in genreCollection)
                {
                    isValid = SqlSongGenreRepository.SaveSongGenre(new SongGenre()
                    {
                        SongId = songId,
                        GenreId = genre.GenreId
                    });

                    if (!isValid) break;
                }
            }

            return isValid;
        }

        private bool DeleteSongGenreCollection(int songId)
        {
            var isValid = true;
            var genreCollection = SqlSongGenreRepository.GetSongGenresBySongId(songId);

            foreach (var genre in genreCollection)
            {
                isValid = SqlSongGenreRepository.DeleteSongGenre(new SongGenre()
                {
                    SongId = songId,
                    GenreId = genre.GenreId
                });

                if (!isValid) break;
            }

            return isValid;
        }

        private List<Genre> GetSongGenreCollection(int songId)
        {
            var genreCollection = new List<Genre>();

            var songGenreCollection = SqlSongGenreRepository.SongGenre.Where(x => x.SongId == songId).ToList();
            foreach (var genre in songGenreCollection.Select(songGenre => SqlGenreRepository.Genre.FirstOrDefault(x => x.GenreId == songGenre.GenreId)).Where(genre => genre != null))
            {
                genreCollection.Add(genre);
            }

            return genreCollection;
        }

        #endregion

        #region user private methods

        //private User GetWordpressUser(int wordpressUserId)
        //{
        //    return SqlUserRepository.GetUserByWordpressId(wordpressUserId);
        //}

        #endregion

        #region global private methods

        private string ReplaceIllegalCharacters(string input)
        {
            var util = new Utilities();
            input = util.RemoveSpaces(input);

            return input.Replace(" ", "_").Replace("/", "-").Replace(@"\", "-").Replace("&", "").Replace("'", "").Replace("@", "").Replace("*", "");
        }

        private string SaveFileLocally(HttpPostedFileBase sourceFile)
        {
            if (!Directory.Exists(LocalTempDestinationPath))
            {
                Directory.CreateDirectory(LocalTempDestinationPath);
            }

            var localFile = Path.Combine(LocalTempDestinationPath, sourceFile.FileName);

            if (!File.Exists(localFile))
            {
                sourceFile.SaveAs(localFile);
            }

            return localFile;
        }

        private bool UploadFileToS3(string filePath, string folderName, string uploadType)
        {
            try
            {
                var s3FileUpload = new S3FileDelivery(filePath, folderName);

                return s3FileUpload.UploadFile(filePath, uploadType);
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
            }

            return false;
        }

        private bool DownloadAudioFileFromS3(string filePath, string keyName)
        {
            var s3FileDownload = new S3FileDelivery();

            return s3FileDownload.DownloadFile(filePath, keyName);
        }

        #endregion

        #endregion
    }
}
