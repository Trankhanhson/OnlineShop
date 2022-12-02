using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class PositionDAO
    {
        ClothesShopEntities _dbContext = null;
        public PositionDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<UserGroup> getAll()
        {
            return _dbContext.UserGroups.ToList();
        }
        public UserGroup Insert(UserGroup group, List<Credential> credentials)
        {
            var newGroup = _dbContext.UserGroups.Add(group);
            foreach(var item in credentials)
            {
                item.GroupId = newGroup.GroupId;
            }
            _dbContext.Credentials.AddRange(credentials);
            _dbContext.SaveChanges();
            return newGroup;
        }


        public void Update(UserGroup userGroup, List<Credential> credentials)
        {
            var g = _dbContext.UserGroups.Find(userGroup.GroupId);
            g.Name = userGroup.Name;

            var OldCredential = _dbContext.Credentials.Where(c=>c.GroupId == userGroup.GroupId).ToList();
            _dbContext.Credentials.RemoveRange(OldCredential);
            _dbContext.Credentials.AddRange(credentials);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var u = _dbContext.UserGroups.Find(id);
            _dbContext.Credentials.RemoveRange(u.Credentials.ToList());
            _dbContext.UserGroups.Remove(u);
            _dbContext.SaveChanges();
        }
    }
}
