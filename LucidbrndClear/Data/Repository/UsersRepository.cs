using LucidbrndClear.Data.Database;
using LucidbrndClear.Data.Interfaces;
using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Repository
{
    public class UsersRepository : IAllUsers
    {
        private AppDbContext appDbContext;
        public UsersRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public void AddNewUser(User user)
        {
            appDbContext.Users.Add(user);
            appDbContext.SaveChanges();
        }

        public User GetUser(string Id) => appDbContext.Users.FirstOrDefault(u => string.Equals(u.Id.ToString(), Id));
    }
}
