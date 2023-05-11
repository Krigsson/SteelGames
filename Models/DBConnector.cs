using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SteelGames.Models
{
    public class DBConnector
    {
        private static DBConnector dbConnector;
        private MySqlConnection databaseConnection;
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=SteelGamesKey;";

        private DBConnector()
        {
            databaseConnection = new MySqlConnection(connectionString);
            databaseConnection.Open();
        }

        ~DBConnector()
        {
            databaseConnection.Close();
        }

        public static DBConnector getInstance()
        {
            if (DBConnector.dbConnector == null)
            {
                DBConnector.dbConnector = new DBConnector();
            }

            return dbConnector;
        }

        public MySqlCommand ExecuteQuery(string querySTR)
        {
            MySqlCommand command = new MySqlCommand(querySTR, databaseConnection);
            command.CommandTimeout = 60;

            return command;
        }

        //public List<dynamic> getDBResponseByQuery(string querySTR)
        //{
        //    List<dynamic> data = new List<dynamic>();

        //    MySqlCommand command = ExecuteQuery(querySTR);
        //    MySqlDataReader reader = command.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            dynamic row = new System.Dynamic.ExpandoObject();
        //            row.Email = reader["Email"];
        //            row.Password = reader["Password"];
        //            row.PhoneNumber = reader["PhoneNumber"];
        //            data.Add(row);
        //        }
        //    }
        //    reader.Close();

        //    return data;
        //}

        public List<Game> getGamesByQuery(string query_s)
        {
            List<Game> data = new List<Game>();

            MySqlCommand command = ExecuteQuery(query_s);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Game game = new Game();
                    game.GameID = int.Parse(reader["GameID"].ToString());
                    game.Name = reader["Name"].ToString();
                    game.Platform = reader["Platform"].ToString();
                    game.Description = reader["Description"].ToString();
                    game.Price = double.Parse(reader["Price"].ToString());
                    game.CategoryName = reader["CategoryName"].ToString();
                    game.PreviewImageName = reader["PreviewImageName"].ToString();
                    game.ImageFolderName = reader["ImageFolderName"].ToString();
                    game.SystemReqID = int.Parse(reader["SystemRequirementsID"].ToString());
                    game.SysReq = new SystemRequirements();
                    game.SysReq.SystemReqID = int.Parse(reader["SystemRequirementsID"].ToString());
                    game.SysReq.OS = reader["OS"].ToString();
                    game.SysReq.Processor = reader["Processor"].ToString();
                    game.SysReq.Memory = reader["Memory"].ToString();
                    game.SysReq.Graphics = reader["Graphics"].ToString();
                    game.SysReq.DirectX = reader["DirectX"].ToString();
                    game.SysReq.Storage = reader["Storage"].ToString();
                    game.SysReq.SoundCard = reader["SoundCard"].ToString();
                    data.Add(game);
                }
            }
            reader.Close();

            return data;
        }

        public void registerNewUser(string email, string password, string phone)
        {
            int gamesOwned = 0;
            DateTime currentDate = DateTime.Now;
            int newUserID = 0;

            MySqlCommand response = ExecuteQuery("SELECT MAX(UserID) FROM User");
            MySqlDataReader reader = response.ExecuteReader();

            if(reader.Read())
            {
                newUserID = reader.GetInt32(0);
            }

            newUserID++;

            reader.Close();
            password = GeneralUtils.PasswordHasher(password);

            string sqlQueryInsertUser = "INSERT INTO User (UserID, Email, Password, PhoneNumber) VALUES " +
                "(@value1, @value2, @value3, @value4)";
            string sqlQueryInsertClient = "INSERT INTO Client (ClientID, RegistrationDate, GamesOwned) VALUES " +
                "(@value1, @value2, @value3)";

            using (MySqlCommand command = new MySqlCommand(sqlQueryInsertUser, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", newUserID);
                command.Parameters.AddWithValue("@value2", email);
                command.Parameters.AddWithValue("@value3", password);
                command.Parameters.AddWithValue("@value4", phone);

                command.ExecuteNonQuery();
            }

            using(MySqlCommand command = new MySqlCommand(sqlQueryInsertClient, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", newUserID);
                command.Parameters.AddWithValue("@value2", currentDate);
                command.Parameters.AddWithValue("@value3", gamesOwned);

                command.ExecuteNonQuery();
            }

        }

        public bool loginUser(string email, string password)
        {
            string query_s = $"SELECT * FROM User WHERE Email = \"{email}\"";

            MySqlCommand command = ExecuteQuery(query_s);

            MySqlDataReader reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                if(reader.Read())
                {
                    if(GeneralUtils.VerifyPassword(password, reader["Password"].ToString()))
                    {
                        User currentUser = User.getInstance();
                        currentUser.UserID = int.Parse(reader["UserID"].ToString());
                        currentUser.Email = email;
                        currentUser.PhoneNumber = reader["PhoneNumber"].ToString();
                        currentUser.Administrator = false;
                        currentUser.Logged = true;
                        reader.Close();
                        return true;
                    }
                }
            }

            reader.Close();
            return false;
        }
    }
}