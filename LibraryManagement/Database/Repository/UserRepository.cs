using LibraryManagement.Models;
using LibraryManagement.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Database.Repository
{
    public class UserRepository<T> : IRepository<User>
    {
        #region Prepared Statements
        private readonly string AddUser = "INSERT INTO user(username, password, firstname, lastname, gender, validity, role) VALUES(@username, @password, @firstname, @lastname, @gender, @validity, @role)";
        private readonly string DeleteUser = "DELETE FROM user WHERE iduser=@id";
        private readonly string FindUserById = "SELECT * FROM user WHERE iduser=@id";
        private readonly string FindUserByUsername = "SELECT * FROM user WHERE username=@username";
        private readonly string UpdateUser = "UPDATE user SET username=@username, password=@password, firstname=@firstname, lastname=@lastname WHERE iduser=@id";
        private readonly string SelectAllUsers = "SELECT * FROM user";
        #endregion

        private MySqlCommand command;
        private MySqlDataReader reader;

        private MySqlConnection connection;

        public UserRepository(MySqlConnection connection)
        {
            this.connection = connection;
           
        }

        public void Add(User entity)
        {
            if (connection == null) return;
            command = new MySqlCommand(AddUser, connection);
            

            try
            {
                if (command != null)
                {
                    command.Parameters.AddWithValue("@username", entity.Username);
                    command.Parameters.AddWithValue("@password", entity.Password);
                    command.Parameters.AddWithValue("@firstname", entity.FirstName);
                    command.Parameters.AddWithValue("@lastname", entity.LastName);
                    command.Parameters.AddWithValue("@gender", entity.Gender.ToString());
                    command.Parameters.AddWithValue("@validity", entity.LibraryMembership.EndDateValidity);
                    command.Parameters.AddWithValue("@role", entity.Role);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
           
        }

        public void Delete(User entity)
        {
            if (connection == null) return;
            command = new MySqlCommand(DeleteUser, connection);
           
            try
            {
                if (command != null)
                {
                    command.Parameters.AddWithValue("@id", entity.IdUser);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
           
        }

        public User FindById(int Id)
        {
            if (connection == null) return null;
            command = new MySqlCommand(FindUserById, connection);

            try
            {
                if (command != null)
                {
                    command.Parameters.AddWithValue("@id", Id);
                    command.Prepare();
                    reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Enum.TryParse(reader.GetString(5), out Gender gender);

                        //TO DO:
                        //modify ID in user and here 
                        User user = new User(reader.GetString(3), reader.GetString(4), reader.GetDateTime(6), null, gender);
                        reader.Close();
                        return user;
                    }
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }

        public User FindByUsername(string username)
        {

           
            
            if (connection == null) return null;
            command = new MySqlCommand(FindUserByUsername, connection);

            try
            {
                if (command != null)
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Prepare();
                    reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Enum.TryParse(reader.GetString(5), out Gender gender);
                        int id = reader.GetInt16(0);
                        User user= new User(id, reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(6), gender, reader.GetString(7));
                    reader.Close();
                    return user;
                    }
                
            }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }

        public void Update(User entity)
        {
           //TO DO
        }

        public List<User> SelectAll()
        {
            List<User> users = new List<User>();

            if (connection == null) return null;
            command = new MySqlCommand(SelectAllUsers, connection);
            
            try
            {
                if (command != null)
                {
                    command.Prepare();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Enum.TryParse(reader.GetString(5), out Gender gender);
                        users.Add(new User(reader.GetString(3), reader.GetString(4), reader.GetDateTime(6), null, gender));
                    }
                    reader.Close();
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
                            }

            return users; 
        }
    }
}
