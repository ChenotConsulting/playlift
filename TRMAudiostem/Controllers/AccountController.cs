using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DomainModel.Entities;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TRMAudiostem;
using TRMAudiostem.Filters;
using TRMAudiostem.Models;
using System.Web;
using TRMInfrastructure.Utilities;
using System.IO;
using System.Drawing;

namespace TRMAudiostem.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [ErrorNotification]
    public class AccountController : Controller
    {
        private Utilities util = new Utilities();

        public ActionResult Index()
        {
            return RedirectToAction("ManageArtist");
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";
            ViewBag.Message = "Artist.";

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewBag.Register = "Register as a personal user";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Personal user.";

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public PartialViewResult ArtistTermsAndConditions()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult BusinessTermsAndConditions()
        {
            return PartialView();
        }

        // Artist registration
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult RegisterArtist()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Artist.";

            return View();
        }

        //[AllowAnonymous]
        //public ActionResult RegisterArtistMobile()
        //{
        //    ViewBag.Register = "Register as an artist or band";
        //    ViewBag.RegisterAction = "Register";
        //    ViewBag.Message = "Artist.";

        //    return View();
        //}

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterArtist(ArtistRegisterModel model, FormCollection form)
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Artist.";

            if (!model.TermsAndConditions)
            {
                ModelState.AddModelError("TermsAndConditions", "You must agree to the terms and conditions to register.");
            }

            if (ModelState.IsValid)
            {
                if (model.ProfileImage != null)
                {
                    // Attempt to register the user
                    try
                    {
                        var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                        var util = new Utilities();

                        var artist = new Artist
                            {
                                ProfileImage = util.RemoveSpaces(model.ArtistName) + "/" + model.ProfileImage.FileName,
                                ArtistName = model.ArtistName,
                                UserName = model.UserName,
                                Password = model.Password,
                                UserType = DomainModel.Entities.User.UserTypeList.Artist,
                                Email = model.Email,
                                Facebook = model.Facebook,
                                MySpace = model.MySpace,
                                SoundCloud = model.SoundCloud,
                                Twitter = model.Twitter,
                                Website = model.Website,
                                TermsAndConditionsAccepted = model.TermsAndConditions,
                                PRS = model.PRS,
                                CreativeCommonsLicence = model.CreativeCommonsLicence,
                                Bio = model.Bio
                            };

                        var artistGenreList = new List<Genre>();

                        foreach (var formItem in form)
                        {
                            if (formItem.ToString().StartsWith("genre"))
                            {
                                artistGenreList.Add(new Genre
                                {
                                    GenreId = GetGenreId(formItem.ToString()),
                                    GenreName = GetGenreName(formItem.ToString())
                                });
                            }
                        }

                        if (trmservice.RegisterArtist(artist, artistGenreList, model.ProfileImage))
                        {
                            WebSecurity.Login(model.UserName, model.Password);
                            return RedirectToAction("RegisterSuccess", "Account");
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("Error registering artist", ErrorCodeToString(e.StatusCode));
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Generic Error", e.ToString());
                    }
                }
                else
                {
                    ModelState.AddModelError("MissingProfileImage", "Please select a profile image.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegisterArtistMobile(ArtistRegisterModel model, FormCollection form)
        //{
        //    // redirect to the action with the code
        //    return RedirectToAction("RegisterArtist", new { model = model, form = form });
        //}

        // Business registration
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult RegisterBusiness()
        {
            ViewBag.Register = "Register as a business";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Business.";

            return View();
        }

        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterBusiness(BusinessRegisterModel model)
        {
            ViewBag.Register = "Register as a business";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Business.";

            if (!model.TermsAndConditions)
            {
                ModelState.AddModelError("TermsAndConditions", "You must agree to the terms and conditions to register.");
            }

            //if (ModelState.IsValid)
            //{
            //    if (model.ProfileImage != null)
            //    {
            //        // Attempt to register the user
            //        try
            //        {
            //            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            //            var util = new Utilities();

            //            var artist = new Artist
            //            {
            //                ProfileImage = util.RemoveSpaces(model.ArtistName) + "/" + model.ProfileImage.FileName,
            //                ArtistName = model.ArtistName,
            //                UserName = model.UserName,
            //                Password = model.Password,
            //                UserType = DomainModel.Entities.User.UserTypeList.Artist,
            //                Email = model.Email,
            //                Facebook = model.Facebook,
            //                MySpace = model.MySpace,
            //                SoundCloud = model.SoundCloud,
            //                Twitter = model.Twitter,
            //                Website = model.Website,
            //                TermsAndConditionsAccepted = model.TermsAndConditions
            //            };

            //            var artistGenreList = new List<Genre>();

            //            foreach (var formItem in form)
            //            {
            //                if (formItem.ToString().StartsWith("genre"))
            //                {
            //                    artistGenreList.Add(new Genre
            //                    {
            //                        GenreId = GetGenreId(formItem.ToString()),
            //                        GenreName = GetGenreName(formItem.ToString())
            //                    });
            //                }
            //            }

            //            if (trmservice.RegisterArtist(artist, artistGenreList, model.ProfileImage))
            //            {
            //                WebSecurity.Login(model.UserName, model.Password);
            //                return RedirectToAction("RegisterSuccess", "Account");
            //            }
            //        }
            //        catch (MembershipCreateUserException e)
            //        {
            //            ModelState.AddModelError("Error registering business", ErrorCodeToString(e.StatusCode));
            //        }
            //        catch (Exception e)
            //        {
            //            ModelState.AddModelError("Generic Error", e.ToString());
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("MissingProfileImage", "Please select a profile image.");
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // Personal user registration
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Register = "Register as a personal user";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Personal user.";

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            ViewBag.Register = "Register as a personal user";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Personal user.";

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    Roles.AddUserToRole(model.UserName, "Customer");
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RegisterSuccess()
        {
            Response.AddHeader("REFRESH", "5;URL=" + Url.Action("ManageArtist"));
            return View();
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        // Artist management

        public ActionResult ManageArtist(ManageMessageId? message)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);

            ViewBag.ImagePath = AudiostemBase.StreamingUrl + artist.ProfileImage;

            return View(artist);
        }

        // Artist management
        // GET: /Account/ArtistAlbums

        public PartialViewResult ArtistAlbums()
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);

            return PartialView();
        }

        // Artist management
        // POST: /Account/ArtistAlbums

        [HttpPost]
        public PartialViewResult ArtistAlbums(ArtistAlbumModel model, FormCollection form)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var util = new Utilities();

            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);

            if (ModelState.IsValid)
            {
                if (model.AlbumCover != null)
                {
                    // Attempt to save the album
                    try
                    {
                        var albumGenreList = new List<Genre>();

                        foreach (var formItem in form)
                        {
                            if (formItem.ToString().StartsWith("genre"))
                            {
                                albumGenreList.Add(new Genre
                                {
                                    GenreId = GetGenreId(formItem.ToString()),
                                    GenreName = GetGenreName(formItem.ToString())
                                });
                            }
                        }

                        var album = new Album
                        {
                            AlbumTitle = model.AlbumTitle,
                            AlbumProducer = model.AlbumProducer,
                            AlbumLabel = model.AlbumLabel,
                            AlbumReleaseDate = model.AlbumReleaseDate,
                            AlbumCover = util.RemoveSpaces(artist.ArtistName) + "/" + util.RemoveSpaces(model.AlbumTitle) + "/" + model.AlbumCover.FileName,
                            GenreCollection = albumGenreList,
                            CreatedDate = DateTime.Now
                        };

                        // link the album to the artist and save it
                        trmservice.SaveArtistAlbum(album, artist, album.AlbumCover);
                        ViewBag.StatusMessage = "The album \"" + album.AlbumTitle + "\" has been uploaded successfully.";
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
                else
                {
                    ModelState.AddModelError("MissingAlbumCover", "Please select an album cover");
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView(model);
        }

        public PartialViewResult ViewAlbum(int albumId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var album = trmservice.GetAllAlbums().FirstOrDefault(a => a.AlbumId == albumId);
            ViewBag.AlbumCoverPath = AudiostemBase.StreamingUrl + album.AlbumCover;
            return PartialView(album);
        }

        public ActionResult EditAlbum(int albumId)
        {
            var artistAlbumModel = new ArtistAlbumModel();

            if (albumId > 0)
            {
                var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
                var albumDetails = trmservice.GetArtistAlbumDetails(albumId);

                artistAlbumModel.AlbumId = albumDetails.AlbumId;
                artistAlbumModel.AlbumCoverPath = albumDetails.AlbumCover;
                artistAlbumModel.AlbumLabel = albumDetails.AlbumLabel;
                artistAlbumModel.AlbumProducer = albumDetails.AlbumProducer;
                artistAlbumModel.AlbumReleaseDate = albumDetails.AlbumReleaseDate;
                artistAlbumModel.AlbumTitle = albumDetails.AlbumTitle;
                artistAlbumModel.GenreCollection = albumDetails.GenreCollection;
            }

            return View(artistAlbumModel);
        }

        [HttpPost]
        public string EditAlbum(string form, string fileStream, string fileName, string fileType)
        {
            var result = "The album was not created. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";
            var formCollection = form.Split('&');
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var util = new Utilities();
            var localFile = Path.Combine(AudiostemBase.LocalTempDestinationPath, fileName);

            byte[] bytes = Convert.FromBase64String(fileStream);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image image = Image.FromStream(ms);
                image.Save(localFile);
            }

            // populate the list of albums for this artist to be displayed in the management section
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);

            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumCoverPath")))
            {
                // Attempt to save the album
                try
                {
                    var albumGenreList = new List<Genre>();

                    foreach (var formItem in formCollection)
                    {
                        if (formItem.ToString().StartsWith("genre"))
                        {
                            albumGenreList.Add(new Genre
                            {
                                GenreId = GetGenreId(formItem.ToString()),
                                GenreName = GetGenreName(formItem.ToString())
                            });
                        }
                    }

                    var albumId = 0;
                    if (!string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId")) && Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId")) > 0)
                    {
                        albumId = Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId"));
                        trmservice.UpdateAlbumGenreCollection(albumId, albumGenreList);
                    }

                    // if editing, if no image is uploaded, use the existing path
                    var albumCover = string.Empty;

                    if (string.IsNullOrEmpty(fileName))
                    {
                        albumCover = AudiostemBase.ReturnFormItemValue(formCollection, "AlbumCoverPath");
                    }
                    else
                    {
                        albumCover = util.RemoveSpaces(artist.ArtistName) + "/" + util.RemoveSpaces(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumTitle")) + "/" + fileName;
                    }

                    var releaseDate = AudiostemBase.ReturnFormItemValue(formCollection, "AlbumReleaseDate").ToString().Replace("%2F", "/");

                    var album = new Album
                    {
                        AlbumId = albumId,
                        AlbumTitle = AudiostemBase.ReturnFormItemValue(formCollection, "AlbumTitle"),
                        AlbumProducer = AudiostemBase.ReturnFormItemValue(formCollection, "AlbumProducer"),
                        AlbumLabel = AudiostemBase.ReturnFormItemValue(formCollection, "AlbumLabel"),
                        AlbumReleaseDate = Convert.ToDateTime(releaseDate),
                        AlbumCover = albumCover,
                        GenreCollection = albumGenreList,
                        CreatedDate = DateTime.Now
                    };

                    // link the album to the artist and save it
                    trmservice.SaveArtistAlbum(album, artist, localFile);

                    result = "You have successfully uploaded the album " + album.AlbumTitle;
                }
                catch (MembershipCreateUserException e)
                {
                    result = ErrorCodeToString(e.StatusCode);
                }
            }
            else
            {
                result = "Please select an album cover";
            }

            // delete the temp file
            if (System.IO.File.Exists(localFile))
            {
                System.IO.File.Delete(localFile);
            }
            return result;
        }

        public PartialViewResult DeleteAlbum(int albumId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var album = trmservice.GetArtistAlbumDetails(albumId);

            trmservice.DeleteArtistAlbum(album, artist);

            return PartialView();
        }

        // Artist management
        // GET: /Account/ArtistSongs

        public ActionResult ArtistSongs()
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);

            return View();
        }

        // Artist management
        // POST: /Account/ArtistSongs

        [HttpPost]
        public ActionResult ArtistSongs(ArtistSongModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (model.MediaAsset != null || !string.IsNullOrEmpty(model.MediaAssetPath))
                {
                    // Attempt to save the songs
                    try
                    {
                        var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                        var util = new Utilities();
                        var albumCollection = new List<Album>();
                        var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);

                        var songGenreList = new List<Genre>();

                        foreach (var formItem in form)
                        {
                            if (formItem.ToString().StartsWith("genre"))
                            {
                                songGenreList.Add(new Genre
                                {
                                    GenreId = GetGenreId(formItem.ToString()),
                                    GenreName = GetGenreName(formItem.ToString())
                                });
                            }
                        }

                        var albumId = Convert.ToInt32(form["songAlbum"]);
                        var album = trmservice.GetArtistAlbumDetails(albumId);
                        albumCollection.Add(album);

                        // if editing, if no media file is uploaded, use the existing path
                        var mediaFileFolder = string.Empty;
                        var mediaFileLocation = string.Empty;

                        if (model.MediaAsset != null)
                        {
                            // save the file to a temp location
                            if (!Directory.Exists(AudiostemBase.LocalTempDestinationPath))
                            {
                                Directory.CreateDirectory(AudiostemBase.LocalTempDestinationPath);
                            }

                            var tempLocation = Path.Combine(AudiostemBase.LocalTempDestinationPath, model.MediaAsset.FileName);
                            model.MediaAsset.SaveAs(tempLocation);

                            mediaFileLocation = tempLocation;
                            mediaFileFolder = util.RemoveSpaces(artist.ArtistName) + "/" + util.RemoveSpaces(album.AlbumTitle) + "/";
                        }

                        var song = new Song()
                        {
                            AlbumCollection = albumCollection,
                            CreatedDate = DateTime.Now,
                            GenreCollection = songGenreList,
                            SongComposer = model.SongComposer,
                            SongId = model.SongId,
                            SongReleaseDate = model.SongReleaseDate,
                            SongTitle = model.SongTitle
                        };

                        // upload and save the song
                        if (trmservice.UploadSong(song, mediaFileLocation, "mp3", mediaFileFolder))
                        {
                            ViewBag.StatusMessage = "The song \"" + song.SongTitle + "\" has been uploaded successfully.";
                        }
                        else
                        {
                            ModelState.AddModelError("Song upload error.", "There was an issue uploading your song. Please try again. If the problem persists, please contact us at info@totalresolutiomusic.com");
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                    catch (TimeoutException exc)
                    {
                        util.ErrorNotification(exc);
                        ModelState.AddModelError("Upload Time Out.", "The upload of your song has timed out. Your file may be too big for our system. We have been informed and will work on fixing this issue. You may want to try uploading a shorter version of this file.");
                    }
                    catch (Exception ex)
                    {
                        util.ErrorNotification(ex);
                        ModelState.AddModelError("General issue.", ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("MissingMediaFile", "Please select a media file to upload");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult DeleteSong(int songId, int albumId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var album = trmservice.GetArtistAlbumDetails(albumId);
            var song = trmservice.GetArtistSongDetails(songId);

            trmservice.DeleteSong(song, album);

            return RedirectToAction("ArtistSongs");
        }

        // Artist management
        // GET: /Account/ManageArtist

        public ActionResult ArtistDetails()
        {
            var TrmWcfWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
            var artist = TrmWcfWebServiceJson.GetArtist(WebSecurity.CurrentUserId);

            var artistRegisterModel = new ManageArtistModel
            {
                ArtistName = artist.ArtistName,
                Email = artist.Email,
                Facebook = artist.Facebook,
                GenreCollection = artist.GenreCollection,
                MySpace = artist.MySpace,
                ProfileImagePath = artist.ProfileImage,
                SoundCloud = artist.SoundCloud,
                Twitter = artist.Twitter,
                Website = artist.Website,
                PRS = artist.PRS,
                CreativeCommonsLicence = artist.CreativeCommonsLicence,
                Bio = artist.Bio
            };

            return View(artistRegisterModel);
        }

        // Artist management
        // POST: /Account/ManageArtist

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArtistDetails(ManageArtistModel model, FormCollection form)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var util = new Utilities();
            var profileImage = string.Empty;

            if (ModelState.IsValid)
            {
                if (model.ProfileImage != null || !string.IsNullOrEmpty(model.ProfileImagePath))
                {
                    // Attempt to upate the user
                    try
                    {
                        if (model.ProfileImage == null)
                        {
                            profileImage = model.ProfileImagePath;
                        }
                        else
                        {
                            profileImage = util.RemoveSpaces(model.ArtistName) + "/" + model.ProfileImage.FileName;
                        }

                        artist.ProfileImage = profileImage;
                        artist.ArtistName = model.ArtistName;
                        artist.Email = model.Email;
                        artist.Facebook = model.Facebook;
                        artist.MySpace = model.MySpace;
                        artist.SoundCloud = model.SoundCloud;
                        artist.Twitter = model.Twitter;
                        artist.Website = model.Website;
                        artist.PRS = model.PRS;
                        artist.CreativeCommonsLicence = model.CreativeCommonsLicence;
                        artist.Bio = model.Bio;

                        var artistGenreList = new List<Genre>();

                        foreach (var formItem in form)
                        {
                            if (formItem.ToString().StartsWith("genre"))
                            {
                                artistGenreList.Add(new Genre
                                {
                                    GenreId = GetGenreId(formItem.ToString()),
                                    GenreName = GetGenreName(formItem.ToString())
                                });
                            }
                        }

                        if (trmservice.UpdateArtist(artist, artistGenreList, artist.ProfileImage))
                        {
                            ViewBag.StatusMessage = "You have successfully updated your details";

                            return RedirectToAction("ManageArtist");
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
                else
                {
                    ModelState.AddModelError("MissingProfileImage", "Please select a profile image");
                }
            }

            // If we got this far, something failed, redisplay form
            model.GenreCollection = artist.GenreCollection;
            return View(model);
        }

        // Password management

        public ActionResult PasswordChange(ManageMessageId? message)
        {
            LoadPasswordChangeDefaults(message);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordChange(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("PasswordChange");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("PasswordChange", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("PasswordChange", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // Customer management
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        // Customer management
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (!model.TermsAndConditions)
            {
                ModelState.AddModelError("TermsAndConditions", "You must agree to the terms and conditions to register.");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        var artist = new Artist
                        {
                            UserName = model.UserName,
                            UserType = DomainModel.Entities.User.UserTypeList.Artist,
                            TermsAndConditionsAccepted = model.TermsAndConditions,
                        };

                        var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
                        if (trmwebservice.RegisterArtistSocial(artist, provider, providerUserId))
                        {
                            OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);
                            return RedirectToAction("RegisterSuccess", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError("ArtistRegistrationError", "There was an issue registering you. If the problemt persists, please contact us at info@totalresolutionmusic.com");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        private string GetGenreName(string genreName)
        {
            var genreList = genreName.Split('_');
            return genreList[2];
        }

        private int GetGenreId(string genreName)
        {
            var genreList = genreName.Split('_');
            return Convert.ToInt32(genreList[1]);
        }

        private void LoadPasswordChangeDefaults(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ManageArtist");
        }
        #endregion
    }
}
