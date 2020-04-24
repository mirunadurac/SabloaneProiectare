using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.ChangePassword
{
    public class SafeUserProxy
    {
        IUser RealSubject = null;
        
        public User ChangePassword(User user)
        {
            RealSubject = new UserProtected(user);
            Console.WriteLine("Old Password:");
            string oldPassword = Console.ReadLine();
            Console.WriteLine("New Passord:");
            string newPasword = Console.ReadLine();
            if(oldPassword.Equals(user.Password))
            {
                return RealSubject.ChengePassword(newPasword);
            }
            return null;

        }
        

    }
}
