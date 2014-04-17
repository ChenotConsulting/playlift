using System;
using System.IO;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using DomainModel.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;

namespace TRMWebService
{
    [ServiceContract]
    public interface ITRMWCFWebServiceJson
    {
        #region Account operations

        /// <summary>
        /// Creates a user account. Can also be used to update, including add credits or activate/deactivate an account
        /// </summary>
        /// <param name="user">The user to create an account for</param>
        /// <param name="accountType">The type of the account</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool SaveUserAccount(User user, Account.AccountTypeList accountType);

        /// <summary>
        /// Retrieves the account details for a user. Can also be used to query the credits available
        /// </summary>
        /// <param name="user">The user who owns this account</param>
        /// <returns>An account instance</returns>
        [OperationContract]
        Account GetUserAccount(User user);

        /// <summary>
        /// Deletes a user account
        /// </summary>
        /// <param name="user">The user who owns the account</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool DeleteAccount(User user);

        #endregion

        #region Artist operations

        /// <summary>
        /// Gets all the artists
        /// </summary>
        /// <returns>A list of artists</returns>
        [OperationContract]
        List<Artist> GetAllArtists();

        /// <summary>
        /// Get all events related to an artist
        /// </summary>
        /// <userId>The ID of the user to get events for</userId>
        /// <returns>A list of events</returns>
        [OperationContract]
        List<Event> GetArtistEvents(int userId);

        #region Album section

        /// <summary>
        /// Inserts or updates an album for an artist. Note that the list of genres is always deleted before inserting the updated list.
        /// </summary>
        /// <param name="album">The album to save</param>
        /// <param name="artist">The artist of the album</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool SaveArtistAlbum(Album album, Artist artist, string sourceFile);

        /// <summary>
        /// Returns the collection of albums for a given artist
        /// </summary>
        /// <param name="artist">The artist whose collection must be returned</param>
        /// <returns>A list of albums</returns>
        [OperationContract]
        List<Album> GetArtistAlbums(Artist artist);

        /// <summary>
        /// Gets a single album's details
        /// </summary>
        /// <param name="albumId">The ID of the album to return</param>
        /// <returns>The details of an album</returns>
        [OperationContract]
        Album GetArtistAlbumDetails(int albumId);

        /// <summary>
        /// Delete an album from the artist's albums collection
        /// </summary>
        /// <param name="album">The album to delete</param>
        /// <param name="artist">The artist to delete the album for</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool DeleteArtistAlbum(Album album, Artist artist);

        /// <summary>
        /// Update the genres for an album. Note that all genres stored against this album are deleted first.
        /// </summary>
        /// <param name="albumId">The album to update</param>
        /// <param name="genreCollection">The genres for the album</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateAlbumGenreCollection(int albumId, List<Genre> genreCollection);

        #endregion

        #region Song section

        /// <summary>
        /// This method uploads a song, and converts it to a 128Kbps aac audio file
        /// </summary>
        /// <param name="song">The song details</param>
        /// <param name="filePath">The path to the file</param>
        /// <param name="ext">The extension of the converted file</param>
        /// <returns></returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(RequestFormat = WebMessageFormat.Json)]
        bool UploadSong(Song song, string filePath, string ext, string folderName);

        /// <summary>
        /// Downloads a song to a local folder on the app server
        /// </summary>
        /// <param name="songId">The ID of the song</param>
        /// <returns>A string with the file location</returns>
        [OperationContract]
        string DownloadSongOnServer(int songId);

        /// <summary>
        /// Downloads a file as a stream from the S3 bucket
        /// </summary>
        /// <param name="filePath">The file path to download to</param>
        /// <returns>A file stream</returns>
        [OperationContract]
        Stream DownloadFile(string filePath);

        /// <summary>
        /// Get all the songs in the database
        /// </summary>
        /// <returns>A list of songs</returns>
        [OperationContract]
        List<Song> GetAllSongs();

        /// <summary>
        /// Returns all the songs for a particular album
        /// </summary>
        /// <param name="albumId">The album Id of the album</param>
        /// <returns>A list of songs</returns>
        [OperationContract]
        List<Song> GetAlbumSongs(int albumId);

        /// <summary>
        /// Returns all the songs for a particular artist
        /// </summary>
        /// <param name="wordpressUserId">The userId of the artist</param>
        /// <returns>A list of songs</returns>
        [OperationContract]
        List<Song> GetArtistSongs(int wordpressUserId);

        /// <summary>
        /// Returns a Song
        /// </summary>
        /// <param name="songId">The Id of the song to return</param>
        /// <returns>A song</returns>
        [OperationContract]
        Song GetArtistSongDetails(int songId);

        /// <summary>
        /// Returns the media assets for a given song
        /// </summary>
        /// <param name="songId">The ID of the song</param>
        /// <returns>A list of media assets</returns>
        [OperationContract]
        List<MediaAsset> GetSongMediaAssets(int songId);

        /// <summary>
        /// Returns a list of media assets to download from
        /// </summary>
        /// <param name="songId">The ID of the song to get the download for</param>
        /// <returns>A list of media assets</returns>
        [OperationContract]
        List<MediaAsset> GetSongMediaAssetsForDownload(int songId);

        /// <summary>
        /// Deletes a song
        /// </summary>
        /// <param name="song">The song to delete</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool DeleteSong(Song song, Album album);

        /// <summary>
        /// Removes a song from an album
        /// </summary>
        /// <param name="song">The song to be removed</param>
        /// <param name="album">The album to remove the song from</param>
        /// <returns>True if successful, false if it failed</returns>
        [OperationContract]
        bool DeleteAlbumSong(Song song, Album album);

        #endregion

        #region Event section



        #endregion

        #endregion

        #region Business operations

        /// <summary>
        /// Get all user records of type business
        /// </summary>
        /// <returns>A list of businesses</returns>
        [OperationContract]
        List<Business> GetAllBusinesses();

        #region Playlist section

        /// <summary>
        /// Saves a playlist for a given user
        /// </summary>
        /// <param name="wordpressUserId">The wordpress ID of the user</param>
        /// <param name="playlist">The playlist</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool SavePlaylist(int wordpressUserId, Playlist playlist);

        /// <summary>
        /// Hard delete of a playlist
        /// </summary>
        /// <param name="playlistId">The Id of the playlist to delete</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        bool DeletePlaylist(int playlistId);

        /// <summary>
        /// Removed a song from a playlist
        /// </summary>
        /// <param name="songId">The song to remove</param>
        /// <param name="playlistId">The playlist to remove the song from</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        bool DeletePlaylistSong(int songId, int playlistId);

        #endregion

        #endregion

        #region Business type operations

        /// <summary>
        /// This method gets the business types available
        /// </summary>
        /// <returns>A list of business types</returns>
        [OperationContract]
        List<BusinessType> GetAllBusinessTypes();

        #endregion

        #region Event operations



        #endregion

        #region Genre operations

        /// <summary>
        /// This methods gets all the music genres available
        /// </summary>
        /// <returns>A list of the genres</returns>
        [WebGet()]
        [OperationContract]
        List<Genre> GetAllGenres();

        /// <summary>
        /// This method returns a specific genre
        /// </summary>
        /// <param name="genreId">The ID of the genre to get</param>
        /// <returns>A music genre</returns>
        [OperationContract]
        Genre GetGenre(int genreId);

        #endregion

        #region User operations

        /// <summary>
        /// Registers an admin user
        /// </summary>
        /// <param name="user">The admin user to register</param>
        /// <returns>True if successful or false if failed</returns>
        [OperationContract]
        bool RegisterAdmin(User user);

        /// <summary>
        /// Saves the artist and the relevant tables
        /// </summary>
        /// <param name="user">The artist to be registered</param>
        /// <param name="genreCollection">The genres selected by the artist</param>
        /// <returns>True if the whole process is successful, False if it is not</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool RegisterArtist(Artist user, List<Genre> genreCollection, HttpPostedFileBase sourceFile);

        /// <summary>
        /// Saves the business and all relevant tables
        /// </summary>
        /// <param name="user">The business user to register</param>
        /// <param name="businessType">The business type</param>
        /// <param name="latitude">The latitude of the business location</param>
        /// <param name="longitude">The longitude of the business location</param>
        /// <returns>True if the whole process is successful, False if it is not</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool RegisterBusiness(User user, BusinessType businessType, string latitude, string longitude);

        /// <summary>
        /// Registers a customer user
        /// </summary>
        /// <param name="user">The customer user to register</param>
        /// <returns>True if successful or false if failed</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool RegisterCustomer(User user);

        /// <summary>
        /// Updates a business user
        /// </summary>
        /// <param name="user">The user object to update</param>
        /// <param name="businessType">The type of business (e.g. Pub, Restaurant, etc...)</param>
        /// <param name="latitude">The latitude of the business location</param>
        /// <param name="longitude">The longitude of the business location</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        bool UpdateBusinessUser(User user, BusinessType businessType, string latitude, string longitude);

        /// <summary>
        /// Updates an artist
        /// </summary>
        /// <param name="artist">The artist to update</param>
        /// <param name="genreCollection">The genres of the artist</param>
        /// <param name="sourceFile">The profile image file</param>
        /// <returns></returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateArtist(Artist artist, List<Genre> genreCollection, string sourceFile);

        /// <summary>
        /// Update the genres for an artist. Note that all genres stored against this artist are deleted first.
        /// </summary>
        /// <param name="userId">The user to update</param>
        /// <param name="genreCollection">The genres for the artist</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool UpdateArtistGenreCollection(int userId, List<Genre> genreCollection);

        /// <summary>
        /// Gets a user instance
        /// </summary>
        /// <param name="wordpressUserId">The user Id in the Wordpress database</param>
        /// <returns>An instance of the user class</returns>
        //[OperationContract]
        //User GetUserByWordpressUserId(int wordpressUserId);

        /// <summary>
        /// Gets a user by its ID
        /// </summary>
        /// <param name="userId">the User Id</param>
        /// <returns>A user</returns>
        [OperationContract]
        User GetUserByUserId(int userId);

        /// <summary>
        /// Gets an admin instance
        /// </summary>
        /// <param name="wordpressUserId">The user Id in the Wordpress database</param>
        /// <returns>An instance of the admin class</returns>
        [OperationContract]
        Admin GetAdmin(int wordpressUserId);

        /// <summary>
        /// Gets an artist instance
        /// </summary>
        /// <param name="wordpressUserId">The user Id in the Wordpress database</param>
        /// <returns>An instance of the artist class</returns>
        [OperationContract]
        Artist GetArtist(int wordpressUserId);

        /// <summary>
        /// Gets a business instance
        /// </summary>
        /// <param name="wordpressUserId">The user Id in the Wordpress database</param>
        /// <returns>An instance of the business class</returns>
        [OperationContract]
        Business GetBusiness(int wordpressUserId);

        /// <summary>
        /// Gets a customer instance
        /// </summary>
        /// <param name="wordpressUserId">The user Id in the Wordpress database</param>
        /// <returns>An instance of the customer class</returns>
        [OperationContract]
        Customer GetCustomer(int wordpressUserId);

        /// <summary>
        /// Delete a user and its related records
        /// </summary>
        /// <param name="user">The user to be deleted</param>
        /// <returns>True if successful or False if failed</returns>
        [OperationContract]
        bool DeleteUser(User user);
        #endregion
    }
}
