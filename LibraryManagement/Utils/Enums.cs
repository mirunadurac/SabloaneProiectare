using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Utils
{
    public enum Gender
    {
        Male,
        Female,
        Unknown
    }

    public enum Role
    {
        Admin,
        User
    }

    public enum AuthenticationOption
    {
        Invalid,
        Register,
        Login
    }

    public enum AdminMenuOptions
    {
        Exit,     
        SeeBooks,
        AdBook,
        SeeReport,
        Invalid
    }

    public enum UserMenuOptions
    {
        Exit,
        SeeBooks,
        ChooseBook,
        BorrowBook,
        ReturnBook,
        BorrowedBooks,
        ChangePassword,
        Invalid
    }

}
