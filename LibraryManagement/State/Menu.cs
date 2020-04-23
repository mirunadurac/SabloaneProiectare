using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.State
{
    public class Menu
    {
        State CurrentState { get; set;}

        public NoLoginState NoLoginState { get; set; }
        public LoginState LoginState { get; set; }
        public SeeBooksState SeeBooksState { get; set; }
        public ChooseBookState ChooseBookState { get; set; }
        public BorrowBookState BorrowBookState { get; set; }
        public LeaveState LeaveState { get; set; }
        public ReturnState ReturnState { get; set; }

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
                    this.Login();
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

        void Login()
        {
            CurrentState.Login();
        }

        public void SetMenuState(State state)
        {
            CurrentState = state;
        }
    }
}
