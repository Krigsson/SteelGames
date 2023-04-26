using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public MySqlCommand query(string querySTR)
        {
            MySqlCommand command = new MySqlCommand(querySTR, databaseConnection);
            command.CommandTimeout = 60;

            return command;
        }

        public List<dynamic> getDBResponseByQuery(string querySTR)
        {
            List<dynamic> data = new List<dynamic>();

            MySqlCommand command = query(querySTR);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    dynamic row = new System.Dynamic.ExpandoObject();
                    row.Email = reader["Email"];
                    row.Password = reader["Password"];
                    row.PhoneNumber = reader["PhoneNumber"];
                    data.Add(row);
                }
            }
            reader.Close();

            return data;
        }

        public List<Game> getGamesByQuery(string query_s)
        {
            List<Game> data = new List<Game>();

            MySqlCommand command = query(query_s);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Game game = new Game();
                    game.GameID = int.Parse(reader["GameID"].ToString());
                    game.Name = reader["Name"].ToString();
                    game.Description = reader["Description"].ToString();
                    game.Price = double.Parse(reader["Price"].ToString());
                    game.CategoryName = reader["CategoryName"].ToString();
                    game.SystemReqID = int.Parse(reader["SystemRequirementsID"].ToString());
                    data.Add(game);
                }
            }
            reader.Close();

            getSystemRequirements(data);

            return data;
        }

        public void getSystemRequirements(List<Game> games)
        {
            foreach (Game game in games)
            {
                MySqlCommand systemQuery = query($"SELECT * FROM SystemRequirements WHERE " +
                            $"SystemRequirementsID = {game.SystemReqID}");
                MySqlDataReader systemReader = systemQuery.ExecuteReader();

                if (systemReader.HasRows)
                {
                    while (systemReader.Read())
                    {
                        game.SysReq = new SystemRequirements();
                        game.SysReq.SystemReqID = int.Parse(systemReader["SystemRequirementsID"].ToString());
                        game.SysReq.OS = systemReader["OS"].ToString();
                        game.SysReq.Processor = systemReader["Processor"].ToString();
                        game.SysReq.Memory = systemReader["Memory"].ToString();
                        game.SysReq.Graphics = systemReader["Graphics"].ToString();
                        game.SysReq.DirectX = systemReader["DirectX"].ToString();
                        game.SysReq.Storage = systemReader["Storage"].ToString();
                        game.SysReq.SoundCard = systemReader["SoundCard"].ToString();
                    }
                }

                systemReader.Close();
            }

        }

        public void registerNewUser(string email, string password, string phone)
        {
            int gamesOwned = 0;
            DateTime currentDate = DateTime.Now;
            int newUserID = 0;

            MySqlCommand response = query("SELECT MAX(UserID) FROM User");
            MySqlDataReader reader = response.ExecuteReader();

            if(reader.Read())
            {
                newUserID = reader.GetInt32(0);
            }

            newUserID++;

            reader.Close();

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

    }
}