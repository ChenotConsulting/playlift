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
using System.Net.Mail;
using System.Text;
using System.Net;

namespace TRMAudiostem.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [ErrorNotification]
    public class AccountController : Controller
    {
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
                if (Roles.IsUserInRole(model.UserName, "Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else if (Roles.IsUserInRole(model.UserName, "Business"))
                {
                    return RedirectToAction("Index", "CloudPlayer", null);
                }
                else if (Roles.IsUserInRole(model.UserName, "Artist"))
                {
                    return RedirectToAction("ManageArtist", "Account", null);
                }

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
                            Bio = model.Bio,
                            CountyCityId = model.CountyCityId
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

            if (ModelState.IsValid)
            {
                if (model.Logo != null)
                {
                    // Attempt to register the user
                    try
                    {
                        var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                        var util = new Utilities();

                        var business = new BusinessUser
                        {
                            Logo= util.RemoveSpaces(model.BusinessName) + "/" + model.Logo.FileName,
                            UserName = model.UserName,
                            Password = model.Password,
                            UserType = DomainModel.Entities.User.UserTypeList.Business,
                            BusinessName = model.BusinessName,
                            BusinessType = trmservice.GetAllBusinessTypes().Where(x => x.BusinessTypeId == model.BusinessType).FirstOrDefault(),
                            BusinessTypeId = model.BusinessType,
                            Address1 = model.Address1,
                            Address2 = model.Address2,
                            City = model.City,
                            PostCode = model.PostCode,
                            Country = model.Country,
                            TermsAndConditionsAccepted = model.TermsAndConditions,
                            CreatedDate = DateTime.Now
                        };

                        if (trmservice.RegisterBusiness(business, model.Logo))
                        {
                            WebSecurity.Login(model.UserName, model.Password);
                            return RedirectToAction("RegisterBusinessSuccess", "Account");
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("Error registering business", ErrorCodeToString(e.StatusCode));
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

        public ActionResult RegisterBusinessSuccess()
        {
            Response.AddHeader("REFRESH", "5;URL=" + Url.Action("ManageBusiness"));
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

            return View(artist);
        }

        // Artist management
        // GET: /Account/ArtistAlbums

        public PartialViewResult ArtistProfile()
        {
            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmwebservice.GetArtist(WebSecurity.CurrentUserId);

            ViewBag.ImagePath = AudiostemBase.StreamingUrl + artist.ProfileImage;

            return PartialView(artist);
        }

        public PartialViewResult ArtistAlbums()
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);
            ViewBag.ArtistName = artist.ArtistName;

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
            ViewBag.ArtistName = trmservice.GetArtist(WebSecurity.CurrentUserId).ArtistName;

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

            if (!string.IsNullOrEmpty(fileStream))
            {
                byte[] bytes = Convert.FromBase64String(fileStream);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image image = Image.FromStream(ms);
                    image.Save(localFile);
                }
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

        [HttpPost]
        public string DeleteAlbum(int albumId)
        {
            var result = "There was a problem deleting this album. Please try again. If the problem persists, please contact support@totalresolutionmusic.com";

            try
            {
                var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
                var album = trmservice.GetArtistAlbumDetails(albumId);

                trmservice.DeleteArtistAlbum(album, artist);
                result = "The album " + album.AlbumTitle + " has been successfully removed.";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        // Artist management
        // GET: /Account/ArtistSongs

        public PartialViewResult ArtistSongs(int albumId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);
            ViewBag.AlbumId = albumId;

            return PartialView();
        }

        // Artist management
        // POST: /Account/ArtistSongs

        [HttpPost]
        public string EditSong(string form, string fileStream, string fileName, string fileType)
        {
            var result = "The song was not uploaded. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";
            var formCollection = form.Split('&');
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var util = new Utilities();
            var localFile = Path.Combine(AudiostemBase.LocalTempDestinationPath, fileName);

            if (!string.IsNullOrEmpty(fileStream))
            {
                byte[] bytes = Convert.FromBase64String(fileStream);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (FileStream file = new FileStream(localFile, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }
            }

            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "MediaAssetPath")))
            {
                // Attempt to save the song
                try
                {
                    var albumCollection = new List<Album>();
                    var songGenreList = new List<Genre>();

                    foreach (var formItem in formCollection)
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

                    var albumId = 0;
                    if (!string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId")) && Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId")) > 0)
                    {
                        albumId = Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "AlbumId"));
                    }

                    var songId = 0;
                    if (!string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "SongId")) && Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "SongId")) > 0)
                    {
                        songId = Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "SongId"));
                    }

                    var album = trmservice.GetArtistAlbumDetails(albumId);
                    albumCollection.Add(album);

                    var mediaFileFolder = string.Empty;
                    var mediaFileLocation = string.Empty;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        mediaFileLocation = localFile;
                        mediaFileFolder = util.RemoveSpaces(artist.ArtistName) + "/" + util.RemoveSpaces(album.AlbumTitle) + "/";
                    }

                    var releaseDate = AudiostemBase.ReturnFormItemValue(formCollection, "SongReleaseDate").ToString().Replace("%2F", "/");

                    var song = new Song()
                    {
                        AlbumCollection = albumCollection,
                        CreatedDate = DateTime.Now,
                        GenreCollection = songGenreList,
                        SongComposer = AudiostemBase.ReturnFormItemValue(formCollection, "SongComposer"),
                        SongId = songId,
                        SongReleaseDate = Convert.ToDateTime(releaseDate),
                        SongTitle = AudiostemBase.ReturnFormItemValue(formCollection, "SongTitle"),
                        PRS = AudiostemBase.ReturnFormBooleanValue(formCollection, "PRS")
                    };

                    // upload and save the song
                    if (trmservice.UploadSong(song, mediaFileLocation, "mp3", mediaFileFolder))
                    {
                        result = "The song \"" + song.SongTitle + "\" has been uploaded successfully.";
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    result = ErrorCodeToString(e.StatusCode);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            else
            {
                result = "Please select a media file to upload";
            }

            return result;
        }

        public PartialViewResult AlbumSongList(int albumId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var songList = trmservice.GetAlbumSongs(albumId);
            var album = trmservice.GetAllAlbums().FirstOrDefault(a => a.AlbumId == albumId);

            ViewBag.AlbumId = albumId;
            ViewBag.AlbumTitle = album.AlbumTitle;

            return PartialView(songList);
        }

        public string DeleteSong(int songId, int albumId)
        {
            var result = "There was a problem deleting this song. Please try again. If the problem persists, please contact support@totalresolutionmusic.com";

            try
            {
                var trmservice = new TRMWebService.TRMWCFWebServiceJson();
                var album = trmservice.GetArtistAlbumDetails(albumId);
                var song = trmservice.GetArtistSongDetails(songId);

                trmservice.DeleteSong(song, album);
                result = "The song " + song.SongTitle + "has been successfully removed from the album " + album.AlbumTitle + ".";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        // Artist management
        // GET: /Account/ManageArtist

        public PartialViewResult ArtistDetails()
        {
            var TrmWcfWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
            var artist = TrmWcfWebServiceJson.GetArtist(WebSecurity.CurrentUserId);
            var countyCity = TrmWcfWebServiceJson.GetAllCountyCities();

            ViewBag.Counties = countyCity.Select(x => x.County).ToList();
            ViewBag.Cities = countyCity.Select(x => x.City).ToList();

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
                Bio = artist.Bio,
                CountyCityId = artist.CountyCityId
            };

            return PartialView(artistRegisterModel);
        }

        // Artist management
        // POST: /Account/ManageArtist

        [HttpPost]
        public string ArtistDetails(string form, string fileStream, string fileName, string fileType)
        {
            var result = "Your details have not been updated. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";
            var formCollection = form.Split('&');
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(WebSecurity.CurrentUserId);
            var util = new Utilities();
            var profileImage = string.Empty;
            var localFile = Path.Combine(AudiostemBase.LocalTempDestinationPath, fileName);

            if (!string.IsNullOrEmpty(fileStream))
            {
                byte[] bytes = Convert.FromBase64String(fileStream);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (FileStream file = new FileStream(localFile, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }
            }

            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "ProfileImagePath")))
            {
                // Attempt to upate the user
                try
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        profileImage = AudiostemBase.ReturnFormItemValue(formCollection, "ProfileImagePath");
                    }
                    else
                    {
                        profileImage = util.RemoveSpaces(AudiostemBase.ReturnFormItemValue(formCollection, "ArtistName")) + "/" + fileName;
                    }

                    artist.ProfileImage = profileImage;
                    artist.ArtistName = AudiostemBase.ReturnFormItemValue(formCollection, "ArtistName");
                    artist.Email = AudiostemBase.ReturnFormItemValue(formCollection, "Email");
                    artist.Facebook = AudiostemBase.ReturnFormItemValue(formCollection, "Facebook");
                    artist.MySpace = AudiostemBase.ReturnFormItemValue(formCollection, "MySpace");
                    artist.SoundCloud = AudiostemBase.ReturnFormItemValue(formCollection, "SoundCloud");
                    artist.Twitter = AudiostemBase.ReturnFormItemValue(formCollection, "Twitter");
                    artist.Website = AudiostemBase.ReturnFormItemValue(formCollection, "Website");
                    artist.PRS = AudiostemBase.ReturnFormBooleanValue(formCollection, "PRS");
                    artist.CreativeCommonsLicence = AudiostemBase.ReturnFormBooleanValue(formCollection, "CreativeCommonsLicence");
                    artist.Bio = AudiostemBase.ReturnFormItemValue(formCollection, "Bio");

                    var artistGenreList = new List<Genre>();

                    foreach (var formItem in formCollection)
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

                    if (trmservice.UpdateArtist(artist, artistGenreList, localFile))
                    {
                        result = "You have successfully updated your details";
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    result = ErrorCodeToString(e.StatusCode);
                }
            }
            else
            {
                result = "Please select a profile image";
            }

            return result;
        }

        // Password management

        public PartialViewResult PasswordChange(ManageMessageId? message)
        {
            LoadPasswordChangeDefaults(message);

            return PartialView();
        }

        [HttpPost]
        public string PasswordChange(string form)
        {
            var result = "Your password has not been updated. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";
            var formCollection = form.Split('&');
            var model = new LocalPasswordModel();

            model.OldPassword = AudiostemBase.ReturnFormItemValue(formCollection, "OldPassword");
            model.NewPassword = AudiostemBase.ReturnFormItemValue(formCollection, "NewPassword");
            model.ConfirmPassword = AudiostemBase.ReturnFormItemValue(formCollection, "ConfirmPassword");

            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("PasswordChange");
            if (hasLocalAccount)
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
                    result = "You have successfully updated your password!";
                }
                else
                {
                    result = "The current password is incorrect or the new password is invalid.";
                }
            }

            return result;
        }

        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user;
                var artistUser = new Artist();
                using (var context = new UsersContext())
                {
                    var userProfile = context.UserProfiles.Where(u => u.UserName == model.Username).FirstOrDefault();

                    if (userProfile != null && !string.IsNullOrEmpty(userProfile.UserName))
                    {
                        user = Membership.GetUser(userProfile.UserName);

                        try
                        {
                            // get local user for their email
                            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
                            artistUser = trmwebservice.GetArtist(userProfile.UserId);
                        }
                        catch
                        {
                            ModelState.AddModelError("", "No artist found by that user name.");

                            return View(model);
                        }
                    }
                    else
                    {
                        user = null;
                    }
                }
                if (user != null && artistUser != null)
                {
                    try
                    {
                        // Generate password token that will be used in the email link to authenticate user
                        var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                        // Generate the html link sent via emailModelState.AddModelError("", "There was an issue sending email: " + e.Message);
                        StringBuilder resetLink = new StringBuilder();
                        resetLink.Append(Url.Action("ResetPassword", "Account", new { rt = token }, "http"));
                        resetLink.AppendLine(Environment.NewLine);
                        resetLink.AppendLine("If the link does not work, please copy and paste it in your browser.");
                        resetLink.AppendLine(Environment.NewLine);
                        resetLink.AppendLine("The team at Total Resolution Music Ltd.");

                        // Email stuff
                        string subject = "Audiostem - Reset your password for " + artistUser.ArtistName;
                        string body = "Reset password link: " + resetLink;
                        string from = "noreply@totalresolutionmusic.com";

                        MailMessage message = new MailMessage(from, artistUser.Email);
                        message.Subject = subject;
                        message.Body = body;
                        SmtpClient client = new SmtpClient("auth.smtp.1and1.co.uk");
                        client.Credentials = new NetworkCredential("info@totalresolutionmusic.com", "trm_info");

                        // Attempt to send the email
                        try
                        {
                            client.Send(message);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "There was an issue sending email: " + e.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "We cannot reset your password because: " + ex.Message + " If you have registered with a social network, please reset your password with the provider.");
                    }

                }
                else // Email not found
                {
                    /* Note: You may not want to provide the following information
                    * since it gives an intruder information as to whether a
                    * certain email address is registered with this website or not.
                    * If you're really concerned about privacy, you may want to
                    * forward to the same "Success" page regardless whether an
                    * user was found or not. This is only for illustration purposes.
                    */
                    ModelState.AddModelError("", "No user found by that user name.");

                    return View(model);
                }
            }

            /* You may want to send the user to a "Success" page upon the successful
            * sending of the reset email link. Right now, if we are 100% successful
            * nothing happens on the page. :P
            */
            return RedirectToAction("ResetLinkSent");
        }

        [AllowAnonymous]
        public ActionResult ResetLinkSent()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
                if (resetResponse)
                {
                    ViewBag.Message = "Your password has been changed successfully!";
                }
                else
                {
                    ViewBag.Message = "Your password has not been changed. Please try again! If the problem persists, please contact support.";
                }
            }
            return View(model);
        }

        // Business management
        // GET: /Account/ManageBusiness

        public ActionResult ManageBusiness(ManageMessageId? message)
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

        // Business management
        // POST: /Account/ManageBusiness

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageBusiness(LocalPasswordModel model)
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
                if (Roles.IsUserInRole(result.UserName, "Admin"))
                {
                    return RedirectToAction("Index", "Admin", null);
                }
                else if (Roles.IsUserInRole(result.UserName, "Business"))
                {
                    return RedirectToAction("Index", "CloudPlayer", null);
                }
                else if (Roles.IsUserInRole(result.UserName, "Artist"))
                {
                    return RedirectToAction("ManageArtist", "Account", null);
                }

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
