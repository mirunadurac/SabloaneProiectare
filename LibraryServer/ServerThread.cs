using LibraryManagement.Database;
using LibraryManagement.Database.Repository;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Flyweight;
using LibraryManagement.Models;
using LibraryManagement.Singleton;
using LibraryManagement.State;
using LibraryManagement.Utils;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
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

        private Library Library = Library.Instance;
        private List<User> users = new List<User>();
        private DatabaseConnection databaseConnection;
        private UserRepository<User> userRepository;

        static Report Report { get; set; } = new Report();
        public void StartServerThread(Socket inClientSocket)
        {
            clientSocket = inClientSocket;
            Thread ctThread = new Thread(Execute);
            ctThread.Start();
        }

        static User Login(UserRepository<User> userRepository, User user)
        {
            User userCheck = userRepository.FindByUsername(user.Username);
            if (userCheck != null)
                if (userCheck.Password.Equals(user.Password))
                    return userCheck;
            return null;
        }

        private User LoginRegister(KeyValuePair<int, User> pair, UserRepository<User> userRepository)
        {
            int option = pair.Key;
            User userNew = pair.Value;
            switch (option)
            {
                case 1:
                    userRepository.Add(userNew);

                    break;
                case 2:
                    userNew = Login(userRepository, userNew);

                    break;
            }

            return userNew;
        }

        public ServerThread(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
            userRepository = new UserRepository<User>(databaseConnection.Connetion);

        }
        private void Command(int option)
        {
            byte[] bytes = null;


            bytes = new byte[1024];
            switch (option)
            {

                case 2:
                    try
                    {
                        int bytesRec = clientSocket.Receive(bytes);
                        var receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        int id = Convert.ToInt32(receivedOption);
                    
                        var requestedBook = Library.Books.Where(book => book.Id == id).FirstOrDefault();

                        string serializedObject = JToken.FromObject(requestedBook).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        if (requestedBook != null)
                        {
                            Library.Books.Remove(requestedBook);                          
                            clientSocket.Send(msg);
                            Report.AddNewBook(requestedBook.Type());
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

                    break;
                case 6:
                    int bytesRec2 = clientSocket.Receive(bytes);
                    string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec2);
                    var user = JToken.Parse(receivedMessage).ToObject<User>();
                    userRepository.Update(user);
                    break;
            }
        }

        private void CommandAdmin(int op)
        {
            byte[] bytes = null;
            bytes = new byte[1024];
            //string serializedObject = JToken.FromObject(Report).ToString();
            byte[] msg = Encoding.ASCII.GetBytes(Report.SeeReport());
            clientSocket.Send(msg);
            msg = Encoding.ASCII.GetBytes(Report.TotalBorrowBook());
            clientSocket.Send(msg);
            Report.SeeReport();

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
                    Console.WriteLine("User to verify: ", userToVerify);
                    currentUser = LoginRegister(userToVerify, userRepository);
                    if (currentUser != null)
                    {
                        string serializedObject = JToken.FromObject(currentUser).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        clientSocket.Send(msg);
                        users.Add(currentUser);
                        Console.WriteLine(users.Count);
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
                        Command(userOption);

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
