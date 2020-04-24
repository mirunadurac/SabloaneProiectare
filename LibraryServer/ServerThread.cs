using LibraryManagement.Database;
using LibraryManagement.Database.Repository;
using LibraryManagement.FactoryMethod;
using LibraryManagement.Models;
using LibraryManagement.Singleton;
using LibraryManagement.State;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
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

        private int LookForBook(int id)
        {
            for (int index = 0; index < Library.Books.Count; index++)
            {
                if (Library.Books[index].Id == id)
                    return index;
            }
            return -1;
        }

        public ServerThread(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
            userRepository = new UserRepository<User>(databaseConnection.Connetion);

        }
        private void Command(int op)
        {
            byte[] bytes = null;


            bytes = new byte[1024];
            switch (op)
            {

                case 2:
                    int bytesRec = clientSocket.Receive(bytes);
                    int index = LookForBook(Convert.ToInt32(bytesRec));
                    if (index != -1)
                    {
                        Book book = Library.Books[index];
                        Library.Books.RemoveAt(index);
                        string serializedObject = JToken.FromObject(book).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        clientSocket.Send(msg);
                    }
                    break;
                case 3:
                    Console.Clear();

                    break;
            }
        }

        private void Execute()
        {
            try
            {
                string receivedOption = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    var userToVerify = JToken.Parse(receivedOption).ToObject<KeyValuePair<int, User>>();
                    Console.WriteLine("User to verify: ", userToVerify);
                    User currentUser = LoginRegister(userToVerify, userRepository);
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

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    receivedOption = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    int userOption = Convert.ToInt32(receivedOption);
                    Command(userOption);

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
