using System;
using System.Data.Linq.Mapping;

namespace DomainModel.Entities
{
    [Table(Name="UserProfile")]
    public class User
    {
        public enum UserTypeList
        {
            Admin = 1,
            Artist = 2,
            Business = 3,
            Customer = 4
        };

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserTypeList UserType { get; set; }
    }
}
