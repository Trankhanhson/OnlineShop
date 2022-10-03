using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
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
            List<User> users = _dbContext.Users.ToList();
            return users;
        }

        public User getByUserName(string username)
        {
            return _dbContext.Users.SingleOrDefault(x => x.UserName == username);
        }

        public User getById(long id)
        {
            return _dbContext.Users.Find(id);
        }
        public void Insert(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
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

        public bool ChangeSattus(int id)
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
