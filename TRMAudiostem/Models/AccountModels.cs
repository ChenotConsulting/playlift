using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace TRMAudiostem.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }

        [Display(Name = "Terms and Conditions*")]
        public bool TermsAndConditions { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name*")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class BusinessRegisterModel: RegisterModel
    {
        [Required]
        [Display(Name = "User name*")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "BusinessType*")]
        public int BusinessType { get; set; }

        [Required]
        [Display(Name = "Address1*")]
        public string Address1 { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City*")]
        public string City { get; set; }

        [Required]
        [Display(Name = "PostCode*")]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Country*")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Logo*")]
        public string Logo { get; set; }

        [Display(Name = "Terms and Conditions*")]
        public bool TermsAndConditions { get; set; }
    }

    public class ArtistRegisterModel : RegisterModel
    {
        [Required]
        [Display(Name = "Artist/Band name*")]
        public string ArtistName { get; set; }

        [EmailAddress(ErrorMessage="Your email address must be in the format name@domain.com")]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Please enter a valid Facebook URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Facebook URL")]
        public string Facebook { get; set; }

        [Url(ErrorMessage = "Please enter a valid Website URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Website URL")]
        public string Website { get; set; }

        [Url(ErrorMessage = "Please enter a valid SoundCloud URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "SoundCloud URL")]
        public string SoundCloud { get; set; }

        [Url(ErrorMessage = "Please enter a valid Twitter URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Twitter URL")]
        public string Twitter { get; set; }

        [Url(ErrorMessage = "Please enter a valid MySpace URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "MySpace URL")]
        public string MySpace { get; set; }

        [Display(Name = "Terms and Conditions*")]
        public bool TermsAndConditions { get; set; }

        [Display(Name = "Profile Image*")]
        public HttpPostedFileBase ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }

        [Display(Name = "Are you PRS registered?")]
        public bool PRS { get; set; }

        [Display(Name = "Do you have a Creative Commons Licence?")]
        public bool CreativeCommonsLicence { get; set; }

        [Required]
        [Display(Name = "Tell us about yourself*")]
        public string Bio { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<Genre> GenreCollection { get; set; }

        public IEnumerable<Genre> GenreList
        {
            get
            {
                var TRMWCFWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
                return TRMWCFWebServiceJson.GetAllGenres();
            }
        }
    }

    public class ManageArtistModel
    {
        public int ArtistId { get; set; }

        [Required]
        [Display(Name = "Artist/Band name*")]
        public string ArtistName { get; set; }

        [EmailAddress(ErrorMessage = "Your email address must be in the format name@domain.com")]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Please enter a valid Facebook URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Facebook URL")]
        public string Facebook { get; set; }

        [Url(ErrorMessage = "Please enter a valid Website URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Website URL")]
        public string Website { get; set; }

        [Url(ErrorMessage = "Please enter a valid SoundCloud URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "SoundCloud URL")]
        public string SoundCloud { get; set; }

        [Url(ErrorMessage = "Please enter a valid Twitter URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "Twitter URL")]
        public string Twitter { get; set; }

        [Url(ErrorMessage = "Please enter a valid MySpace URL e.g. http://www.myurl.com/myname")]
        [Display(Name = "MySpace URL")]
        public string MySpace { get; set; }

        [Display(Name = "Profile Image")]
        public HttpPostedFileBase ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }

        [Display(Name = "Are you PRS registered?")]
        public bool PRS { get; set; }

        [Display(Name = "Do you have a Creative Commons Licence?")]
        public bool CreativeCommonsLicence { get; set; }

        [Required]
        [Display(Name = "Tell us about yourself*")]
        public string Bio { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<Genre> GenreCollection { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class LostPasswordModel
    {
        [Required(ErrorMessage = "Please enter your user name to send you a reset link!")]
        [Display(Name = "User name")]
        public string Username { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "Please enter a new Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Please confirm your new Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnToken { get; set; }
    }
}
