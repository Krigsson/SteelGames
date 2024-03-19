using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace SteelGames.Models
{
    public static class ProcessPurchase
    {
        public static void processPurchase(string cardNumber, double price, int gameID)
        {
            User currentUser = (User)HttpContext.Current.Session["LoggedInUser"];
            string transactionNumber = generateTransaction();
            string lastCardDigits = cardNumber.Substring(cardNumber.Length - 4);
            DateTime currentDate = DateTime.Now.Date;
            int keyID = getKeyID(gameID);
            int userID = currentUser.UserID;

            completePurchase(transactionNumber, lastCardDigits, price, currentDate, keyID, userID);


        }

        private static string generateTransaction()
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int length = 10;
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(chars.Length);
                stringBuilder.Append(chars[randomIndex]);
            }

            return stringBuilder.ToString();
        }

        private static int getKeyID(int gameID)
        {
            DBConnector connector = DBConnector.getInstance();
            string query = $"SELECT k.KeyID FROM keys_t k LEFT JOIN Purchase p ON k.KeyID = p.KeyID WHERE p.KeyID IS NULL AND k.GameID = {gameID} LIMIT 1;";
            int result = 0;

            MySqlCommand command = connector.ExecuteQuery(query);
            MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                result = int.Parse(reader["KeyID"].ToString());
            }

            reader.Close();
            return result;
        }

        private static void completePurchase(string transactionNumber, string lastCardDigits, double price,
                                                DateTime currentDate, int keyID, int userID)
        {
            string query = "INSERT INTO Purchase (TransactionNumber, LastCardDigits, Price, Date, KeyID, UserID) VALUES " +
                "(@value1, @value2, @value3, @value4, @value5, @value6)";

            using(MySqlCommand command = new MySqlCommand(query, DBConnector.getInstance().databaseConnection))
            {
                command.Parameters.AddWithValue("@value1", transactionNumber);
                command.Parameters.AddWithValue("@value2", lastCardDigits);
                command.Parameters.AddWithValue("@value3", price);
                command.Parameters.AddWithValue("@value4", currentDate);
                command.Parameters.AddWithValue("@value5", keyID);
                command.Parameters.AddWithValue("@value6", userID);

                command.ExecuteNonQuery();
            }
        }
    }
}