using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Proxy.ChangePassword
{
    interface IUser
    {
        User ChengePassword(String newPassword);
    }
}
