using DomainModel.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> User { get; }

        int SaveUser(User user);
        bool DeleteUser(User user);
        //List<User> GetUsersByType(User.UserTypeList userType);
        //User GetUserByWordpressId(int wordpressUserId);
        //User GetUserById(int userId);
    }
}
