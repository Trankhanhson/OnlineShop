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
using System.Data.Entity;

namespace Models.DAO
{
    public class UserDAO
    {
        ClothesShopEntities _dbContext = null;
        public UserDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<UserGroup> GetUserGroups()
        {
            return _dbContext.UserGroups.Where(u=>u.GroupId!=1).ToList();
        }

        public List<User> getAll()
        {
            _dbContext.Configuration.LazyLoadingEnabled = true;
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

        public void Update(User entity)
        {
            User user = _dbContext.Users.Find(entity.UserID);
            user.Name = entity.Name;
            user.UserAdress = entity.UserAdress;
            user.UserPhone = entity.UserPhone;
            user.UserName = entity.UserName.Trim();
            user.GroupId = entity.GroupId;
            if (entity.Password.Trim() != "")
            {
                user.Password = entity.Password;
            }
            _dbContext.SaveChanges();
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

        public List<string> GetListCredential(User user)
        {
            
            var listResult = user.UserGroup.Credentials.ToList().Select(c=>c.RoleId).ToList();
            return listResult;
        }

        public int Login(string username, string password,bool isLoginAdmin = false)
        {
            User user = _dbContext.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null) 
            {
                return 0; //sai username
            }
            else
            {
                if(isLoginAdmin==true)
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

        public bool ExistUserName(string userName)
        {
            var u = _dbContext.Users.Where(us => us.UserName == userName).FirstOrDefault();
            if (u != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ExistUserNameEdit(User user)
        {
            var u = _dbContext.Users.Where(us => us.UserName == user.UserName && us.UserID != user.UserID).FirstOrDefault();
            if (u != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
