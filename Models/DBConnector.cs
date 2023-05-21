using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SteelGames.Models
{
    public class DBConnector
    {
        private static DBConnector dbConnector;
        public MySqlConnection databaseConnection;
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
                        int tempID = int.Parse(reader["UserID"].ToString());
                        string phoneNumber = reader["PhoneNumber"].ToString();
                        reader.Close();

                        MySqlCommand getUserInfo;
                        MySqlDataReader userInfoReader;

                        if(VerifyAdmin(tempID))
                        {
                            getUserInfo = ExecuteQuery($"SELECT Position FROM Administrator WHERE " +
                                $"AdministratorID = {tempID}");
                            userInfoReader = getUserInfo.ExecuteReader();
                            string position = "";

                            if(userInfoReader.HasRows && userInfoReader.Read())
                            {
                                position = userInfoReader["Position"].ToString();
                            }

                            User.SetAdmin(tempID, email, phoneNumber, position);
                        }
                        else
                        {
                            getUserInfo = ExecuteQuery($"SELECT RegistrationDate FROM Client WHERE " +
                                $"ClientID = {tempID}");
                            userInfoReader = getUserInfo.ExecuteReader();

                            DateTime registrationDate = DateTime.Now;

                            if(userInfoReader.HasRows && userInfoReader.Read())
                            {
                                registrationDate = DateTime.Parse(userInfoReader["RegistrationDate"].ToString());
                            }
                            User.SetClient(tempID, email, phoneNumber, registrationDate);
                        }

                        userInfoReader.Close();

                        return true;
                    }
                }
            }

            reader.Close();
            return false;
        }

        public int getAvailableKeysForCurrentGame(int gameID)
        {
            string query_s = $"SELECT COUNT(*) as key_count FROM keys_t k " +
                $"LEFT JOIN Purchase p ON k.KeyID = p.KeyID " +
                $"WHERE p.KeyID IS NULL AND k.GameID = {gameID};";
            int result = 0;
            MySqlCommand command = ExecuteQuery(query_s);
            MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                result = int.Parse(reader["key_count"].ToString());
            }

            reader.Close();
            return result;
        }

        public void AddNewGameToDB(Game newGame)
        {
            string addGameQuery = "INSERT INTO Game (Name, Description, Price, CategoryName, " +
                "SystemRequirementsID, Platform, PreviewImageName, ImageFolderName) VALUES " +
                "(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)";
            string addSystemReqQuery = "INSERT INTO SystemRequirements (OS, Processor, Memory, " +
                "Graphics, DirectX, Storage, SoundCard) VALUES " +
                "(@value1, @value2, @value3, @value4, @value5, @value6, @value7)";

            using (MySqlCommand command = new MySqlCommand(addSystemReqQuery, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", newGame.SysReq.OS);
                command.Parameters.AddWithValue("@value2", newGame.SysReq.Processor);
                command.Parameters.AddWithValue("@value3", newGame.SysReq.Memory);
                command.Parameters.AddWithValue("@value4", newGame.SysReq.Graphics);
                command.Parameters.AddWithValue("@value5", newGame.SysReq.DirectX);
                command.Parameters.AddWithValue("@value6", newGame.SysReq.Storage);
                command.Parameters.AddWithValue("@value7", newGame.SysReq.SoundCard);

                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = new MySqlCommand(addGameQuery, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", newGame.Name);
                command.Parameters.AddWithValue("@value2", newGame.Description);
                command.Parameters.AddWithValue("@value3", newGame.Price);
                command.Parameters.AddWithValue("@value4", newGame.CategoryName);
                command.Parameters.AddWithValue("@value5", GameList.getInstance()[GameList.getInstance().Count - 1].SystemReqID + 1);
                command.Parameters.AddWithValue("@value6", newGame.Platform);
                command.Parameters.AddWithValue("@value7", newGame.PreviewImageName);
                command.Parameters.AddWithValue("@value8", newGame.ImageFolderName);

                command.ExecuteNonQuery();
            }


            GameList.UpdateGames();
        }

        public void AddKeysToGame(int gameID, string keys)
        {
            string[] listKeys = keys.Split('\n');
            string addKeyToGameQuery = "INSERT INTO keys_t (KeyValue, GameID) VALUES " +
                "(@value1, @value2)";

            foreach (string key in listKeys)
            {

                using (MySqlCommand command = new MySqlCommand(addKeyToGameQuery, databaseConnection))
                {
                    command.Parameters.AddWithValue("@value1", key);
                    command.Parameters.AddWithValue("@value2", gameID);

                    command.ExecuteNonQuery();
                }
            }

        }

        public void EditGameAttributes(int gameID, Game game, bool newPreview)
        {
            string editGameQuery = "UPDATE Game " +
                "SET Name = @value1, Description = @value2, Price = @value3, CategoryName = @value4, " +
                "Platform = @value5, PreviewImageName = @value6, ImageFolderName = @value7 " +
                "WHERE GameID = @value8";

            string editGameReqQuery = "Update SystemRequirements " +
                "SET OS = @value1, Processor = @value2, Memory = @value3, Graphics = @value4, " +
                "DirectX = @value5, Storage = @value6, SoundCard = @value7 " +
                "WHERE SystemRequirementsID = @value8";

            string oldPreview = SteelGames.Models.GameList.getInstance()[gameID - 1].PreviewImageName;

            using (MySqlCommand command = new MySqlCommand(editGameQuery, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1",game.Name);
                command.Parameters.AddWithValue("@value2",game.Description);
                command.Parameters.AddWithValue("@value3",game.Price);
                command.Parameters.AddWithValue("@value4",game.CategoryName);
                command.Parameters.AddWithValue("@value5",game.Platform);
                if(newPreview)
                {
                    command.Parameters.AddWithValue("@value6",game.PreviewImageName);

                }
                else
                {
                    command.Parameters.AddWithValue("@value6", oldPreview);
                }
                command.Parameters.AddWithValue("@value7",game.ImageFolderName);
                command.Parameters.AddWithValue("@value8", gameID);

                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = new MySqlCommand(editGameReqQuery, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", game.SysReq.OS);
                command.Parameters.AddWithValue("@value2", game.SysReq.Processor);
                command.Parameters.AddWithValue("@value3", game.SysReq.Memory);
                command.Parameters.AddWithValue("@value4", game.SysReq.Graphics);
                command.Parameters.AddWithValue("@value5", game.SysReq.DirectX);
                command.Parameters.AddWithValue("@value6", game.SysReq.Storage);
                command.Parameters.AddWithValue("@value7", game.SysReq.SoundCard);
                command.Parameters.AddWithValue("@value8", gameID);

                command.ExecuteNonQuery();
            }

            GameList.UpdateGames();
        }

        public bool VerifyAdmin(int userID)
        {
            string checkAdminQuery = "SELECT COUNT(*) FROM Administrator WHERE AdministratorID = @value1";
            bool result = false;

            using (MySqlCommand command = new MySqlCommand(checkAdminQuery, databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", userID);

                int count = int.Parse(command.ExecuteScalar().ToString());
                result = count > 0;
            }

            return result;
        }
    }
}