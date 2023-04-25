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

        public void printDBQueryResult(string querySTR)
        {
            MySqlCommand command = query(querySTR);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetString(i)} ");
                    }

                    Console.WriteLine();
                }
            }
            reader.Close();
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
                    data.Add(game);
                }
            }
            reader.Close();

            return data;
        }

        //public List<User> getUsersFromDB(string querySTR)
        //{
        //    List<User> data = new List<User>();

        //    MySqlCommand command = query(querySTR);
        //    MySqlDataReader reader = command.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            User user = new User();
        //            user.Password = reader["Password"].ToString();
        //            user.Email = reader["Email"].ToString();
        //            user.PhoneNumber = reader["PhoneNumber"].ToString();
        //            data.Add(user);
        //        }
        //    }
        //    reader.Close();

        //    return data;
        //}

    }
}