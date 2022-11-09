using Models.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserDAO
    {
        ClothesShopEntities _dbContext = null;
        public UserDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<User> getAll()
        {
            _dbContext.Configuration.LazyLoadingEnabled = false;
            List<User> users = _dbContext.Users.ToList();
            return users;
        }

        public IEnumerable<User> getPage(string searchResult,int page,int pageSize)
        {
            IQueryable<User> model = _dbContext.Users;
            if (!string.IsNullOrEmpty(searchResult))
            {
                model = model.Where(x=>x.UserName.Contains(searchResult) || x.Name.Contains(searchResult));
            }
            return model.OrderByDescending(x => x.Name).ToPagedList(page,pageSize);
        }

        public User getByUserName(string username)
        {
            return _dbContext.Users.SingleOrDefault(x => x.UserName == username);
        }

        public User getById(long id)
        {
            return _dbContext.Users.Find(id);
        }
        public User Insert(User user)
        {
            _dbContext.Configuration.LazyLoadingEnabled = false;
            var u = _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return u;
        }

        public bool Update(User entity)
        {
            try
            {
                User user = _dbContext.Users.Find(entity.UserID);
                user.Name = entity.Name;
                user.UserAdress = entity.UserAdress;
                user.UserPhone=entity.UserPhone;
                user.UserName = entity.UserName;
                user.Password = entity.Password;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool? ChangeSattus(int id)
        {
            User user = _dbContext.Users.Find(id);
            user.Status=!user.Status;
            _dbContext.SaveChanges();
            return user.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                User user = _dbContext.Users.Find(id);
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch { 
                return false;
            }
        }

        public int Login(string username, string password)
        {
            User user = _dbContext.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return 0; //sai username
            }
            else
            {
                if (user.Status == false)
                {
                    return -1; //account is locked
                }
                else
                {
                    if (user.Password == password)
                    {
                        return 1;
                    }
                    else
                    {
                        //pass sai
                        return -2;
                    }
                }
            }
        }
    }
}
