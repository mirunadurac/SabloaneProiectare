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
                    this.SeeBooks();
                    break;
                case EUserOption.ChooseBooks:
                    this.ChooseBook();
                    break;
                case EUserOption.BorrowBooks:
                    this.BorrowBook();
                    break;
                case EUserOption.Leave:
                    this.Leave();
                    break;
                case EUserOption.ReturnBook:
                    this.ReturnBook();
                    break;
            }
        }

        void Login()
        {
            CurrentState.Login();
        }

        void SeeBooks()
        {
            CurrentState.SeeBooks();
        }

        void ChooseBook()
        {
            CurrentState.ChooseBook();
        }

        void BorrowBook()
        {
            Console.WriteLine("The number of days for borrow the book");
            int days = Convert.ToInt32(Console.ReadLine());
            CurrentState.BorrowBook(days);
        }
        void Leave()
        {
            CurrentState.Leave();
        }
        void ReturnBook()
        {
            CurrentState.Return();
        }

        public void SetMenuState(State state)
        {
            CurrentState = state;
        }
    }
}
