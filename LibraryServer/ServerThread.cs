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
        Library Library = Library.Instance;
        List<User> users = new List<User>();
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
        private User LoginRegister(KeyValuePair<int,User> pair, UserRepository<User> userRepository)
        {
            int op = pair.Key;
            User userNew = pair.Value;
            switch (op)
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
            for(int index=0;index<Library.Books.Count;index++)
            {
                if (Library.Books[index].Id == id)
                    return index;
            }
            return -1;
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
            var databaseConnection = DatabaseConnection.Instance;
            databaseConnection.OpenConnection();
            var userRepository = new UserRepository<User>(databaseConnection.Connetion);
            
            // Incoming data from the client. 
            try
            {
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    var fooReceived = JToken.Parse(data).ToObject<KeyValuePair<int,User>>();
                    Console.WriteLine("Text received : {0}", fooReceived);
                    User message = LoginRegister(fooReceived, userRepository);
                    if (message != null)
                    {
                        string serializedObject = JToken.FromObject(message).ToString();
                        byte[] msg = Encoding.ASCII.GetBytes(serializedObject);
                        clientSocket.Send(msg);
                        users.Add(message);
                        break;
                    }
                    else
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("Wrong user name or password");
                        clientSocket.Send(msg);
                    }
                    

                    if (data.IndexOf("Stop") > -1)
                    {
                        break;
                    }
                   
                }
               
                while(true)
                {
                    bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    int op = Convert.ToInt32(data);
                    Command(op);
                    
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
