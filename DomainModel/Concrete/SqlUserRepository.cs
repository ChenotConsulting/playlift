using DomainModel.Abstract;
using DomainModel.Entities;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace DomainModel.Concrete
{
    public class SqlUserRepository : IUserRepository
    {
        private Table<User> UserTable;
        public SqlUserRepository(string connString)
        {
            UserTable = (new DataContext(connString)).GetTable<User>();
        }

        public IQueryable<User> User
        {
            get { return UserTable; }
        }

        public int SaveUser(User user)
        {
            try
            {
                if (user.UserId == 0)
                {
                    UserTable.InsertOnSubmit(user);
                }
                else
                {
                    UserTable.Context.Refresh(RefreshMode.KeepCurrentValues, user);
                }

                UserTable.Context.SubmitChanges();

                return user.UserId; 
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteUser(User user)
        {
            try
            {
                UserTable.DeleteOnSubmit(user);
                UserTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public List<User> GetUsersByType(User.UserTypeList userType)
        //{
        //    return UserTable.Where(x => x.UserTypeId == (int)userType).ToList();
        //}

        //public User GetUserByWordpressId(int wordpressUserId)
        //{
        //    return UserTable.FirstOrDefault(x => x.WordpressUserId == wordpressUserId);
        //}

        //public User GetUserById(int userId)
        //{
        //    return UserTable.FirstOrDefault(x => x.UserId == userId);
        //}
    }
}
