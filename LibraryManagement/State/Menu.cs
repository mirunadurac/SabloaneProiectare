using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    public class Menu
    {
        State CurrentState { get; set;}

        NoLoginState NoLoginState { get; set; }
        LoginState LoginState { get; set; }
        SeeBooksState SeeBooksState { get; set; }
        ChooseBookState ChooseBookState { get; set; }
        BorrowBookState BorrowBookState { get; set; }
        LeaveState LeaveState { get; set; }
        ReturnState ReturnState { get; set; }

        public Menu (User user)
        {
            CurrentState = new NoLoginState(user, this);
            NoLoginState = new NoLoginState(user, this);
            LoginState = new LoginState(user, this);
            SeeBooksState = new SeeBooksState(user, this);
            ChooseBookState = new ChooseBookState(user, this);
            BorrowBookState = new BorrowBookState(user, this);
            LeaveState = new LeaveState(user, this);
            ReturnState = new ReturnState(user, this);
        }

        public void UpdateState(EUserOption option)
        {
            switch (option)
            {
                case EUserOption.Login:
                    
                    break;
                case EUserOption.SeeBooks:
                    
                    break;
                case EUserOption.ChooseBooks:
                    
                    break;
                case EUserOption.BorrowBooks:
                   
                    break;
                case EUserOption.Leave:
                    
                    break;
                case EUserOption.ReturnBook:

                    break;
            }
        }

        public void SetMenuState(State state)
        {
            CurrentState = state;
        }
    }
}
