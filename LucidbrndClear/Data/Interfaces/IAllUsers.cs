using LucidbrndClear.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Interfaces
{
    public interface IAllUsers
    {
        public User GetUser(string Id);
        public void AddNewUser(User user);
    }
}
