using LibraryManagement.FactoryMethod;
using LibraryManagement.Flyweight;
using LibraryManagement.Models;
using LibraryManagement.Models.DatabaseModels;
using LibraryManagement.Repository;
using LibraryManagement.Singleton;
using LibraryManagement.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibraryServer
{
    public class ServerThread
    {
        private Socket clientSocket;
        private UnitOfWork unitOfWork;

        static Report Report { get; set; } = new Report();
        public void StartServerThread(Socket inClientSocket)
        {
            clientSocket = inClientSocket;
            Thread ctThread = new Thread(Execute);
            ctThread.Start();
        }
        private User Login(User user)
        {
            var userCheck = unitOfWork.UserRepository.DbSet.FirstOrDefault(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password));
            if (userCheck != null)
            {
                return new User(userCheck);
            }
            return null;
        }

        private User LoginRegister(KeyValuePair<int, User> pair)
        {
            var option = (AuthenticationOption)pair.Key;
            var newUser = pair.Value;
            switch (option)
            {
                //register
                case AuthenticationOption.Register:
                    var databaseUser = new UserDatabase
                    {
                        Username = newUser.Username,
                        Password = newUser.Password,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Validity = newUser.LibraryMembership.EndDateValidity,
                        Gender = newUser.Gender,
                        Role = Role.User
                    };
                    unitOfWork.UserRepository.Insert(databaseUser);
                    unitOfWork.Save();

                    break;
                //login
                case AuthenticationOption.Login:
                    newUser = Login(newUser);

                    break;
            }

            return newUser;
        }

        public ServerThread()
        {
            unitOfWork = new UnitOfWork();

        }

        private void CommandUser(int option)
        {
            byte[] bytes = null;


            bytes = new byte[1024];

            var enumOption = (UserMenuOptions)option;
            switch (enumOption)
            {
                case UserMenuOptions.SeeBooks:
                    SeeBooks();
                    break;
                case UserMenuOptions.ChooseBook:
                    ChooseBook(bytes);
                    break;
                case UserMenuOptions.ReturnBook:
                    ReturnBook();

                    break;
                case UserMenuOptions.ChangePassword:
                    //int bytesRec2 = clientSocket.Receive(bytes);
                    //string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec2);
                    //var user = JToken.Parse(receivedMessage).ToObject<User>();
                    ////userRepository.Update(user);
                    break;
            }
        }

        private void ReturnBook()
        {
            var bookBytes = new byte[1024];
            int bytesRec = clientSocket.Receive(bookBytes);

            string receivedMessage = Encoding.ASCII.GetString(bookBytes, 0, bytesRec);
            var bookToAdd = JToken.Parse(receivedMessage).ToObject<BookDatabase>();

            unitOfWork.BookRepository.Insert(bookToAdd);
            unitOfWork.Save();
        }

        private void ChooseBook(byte[] bytes)
        {
            try
            {
                int bytesRec = clientSocket.Receive(bytes);
                var receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                int id = Convert.ToInt32(receivedOption);

                var requestedBook = unitOfWork.BookRepository.GetById(id);

                string serializedObject = JToken.FromObject(requestedBook).ToString();
                byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                if (requestedBook != null)
                {
                    clientSocket.Send(msg);
                    unitOfWork.BookRepository.Delete(requestedBook);
                    unitOfWork.Save();
                    Report.AddNewBook(requestedBook.Type);
                    Report.SeeReport();
                }
                else
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes("Book not found"));
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid book ID");
                clientSocket.Send(Encoding.ASCII.GetBytes("Book not found"));
            }
        }

        private void SeeBooks()
        {
            List<BookDatabase> books = unitOfWork.BookRepository.DbSet.ToList();
            var serializedBooks = JToken.FromObject(books).ToString();
            byte[] serializedBooksByte = Encoding.ASCII.GetBytes(serializedBooks);
            int bytesSent = clientSocket.Send(serializedBooksByte);
        }

        private void CommandAdmin(int op)
        {
            var option = (AdminMenuOptions)op;
            switch (option)
            {
                case AdminMenuOptions.SeeBooks:
                    break;

                case AdminMenuOptions.AddBook:
                    var bookBytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bookBytes);

                    string receivedMessage = Encoding.ASCII.GetString(bookBytes, 0, bytesRec);
                    var pair = JToken.Parse(receivedMessage).ToObject<KeyValuePair<Book, EBookType>>();
                    var book = pair.Key;
                    var dbBook = new BookDatabase
                    {
                        Author = book.Author,
                        PublicationDate = new DateTime(book.PublicationDate, 1, 1),
                        Title = book.Title,
                        Type = pair.Value

                    };

                    unitOfWork.BookRepository.Insert(dbBook);
                    unitOfWork.Save();

                    break;
                case AdminMenuOptions.SeeReport:
                    byte[] bytes = null;
                    bytes = new byte[1024];
                    string serializedObject = JToken.FromObject(Report).ToString();
                    byte[] msg = Encoding.ASCII.GetBytes(Report.SeeReport());
                    clientSocket.Send(msg);
                    msg = Encoding.ASCII.GetBytes(Report.TotalBorrowBook());
                    clientSocket.Send(msg);
                    Report.SeeReport();
                    break;
                default:
                    break;
            }

        }
        private void Execute()
        {
            try
            {
                string receivedOption = null;
                byte[] bytes = null;
                User currentUser = null;
                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    var userToVerify = JToken.Parse(receivedOption).ToObject<KeyValuePair<int, User>>();
                    Console.WriteLine("User to verify: ", userToVerify.Value);
                    currentUser = LoginRegister(userToVerify);
                    if (currentUser != null)
                    {
                        string serializedObject = JToken.FromObject(currentUser).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        clientSocket.Send(msg);
                        break;
                    }
                    else
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("Wrong user name or password");
                        clientSocket.Send(msg);
                    }
                }

                if (currentUser.Role == Role.User)
                {
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = clientSocket.Receive(bytes);
                        receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        int userOption = Convert.ToInt32(receivedOption);
                        CommandUser(userOption);

                    }
                }
                else
                {
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = clientSocket.Receive(bytes);
                        receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        int userOption = Convert.ToInt32(receivedOption);
                        CommandAdmin(userOption);
                    }
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
