using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace LibraryManagement.Database
{
    public class DatabaseConnection
    {
        public MySqlConnection Connetion { get; private set; }

        private static object padlock = new object();
        private static DatabaseConnection instance;
        public static DatabaseConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseConnection();
                        }
                    }
                }

                return instance;
            }
        }

        private readonly string connectionString = $@"server=localhost;userid={Utils.DbConstants.UsernameDatabase};password={Utils.DbConstants.PasswordDatabase};database={Utils.DbConstants.NameDatabase}";

        private DatabaseConnection()
        {
            Connetion = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            Connetion.Open();
            Console.WriteLine($" The Connection is: {Connetion.State}");
        }

        public void CloseConnection()
        {
            Connetion.Close();
        }
    }
}
