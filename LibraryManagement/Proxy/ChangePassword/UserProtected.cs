using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.ChangePassword
{
    class UserProtected : IUser
    {
        User User;

        public UserProtected (User user)
        {
            User = user;
        }

        public User ChengePassword(String newPassword)
        {
            User.Password = newPassword;
            return User;
        }
    }
}
