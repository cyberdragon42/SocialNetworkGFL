using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService: IUserService
    {
        private readonly SocialNetworkContext context;
        public UserService(SocialNetworkContext context)
        {
            this.context = context;
        }

        public User GetUser(string userId)
        {
            var user = context.Users.
                Where(u => u.Id == userId)
                .Include(u=>u.Posts)
                .FirstOrDefault();
            return user;
        }
    }
}
