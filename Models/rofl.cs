public class GameKeyModel
{
    private static GameKeyModel instance;
    public List<KeyStruct> keys;

    private GameKeyModel()
    {
        if (keys == null)
        {
            keys = new List<KeyStruct>();
        }
    }

    public static GameKeyModel getInstance()
    {
        if (instance == null)
        {
            instance = new GameKeyModel();
        }

        return instance;
    }

    public class KeyStruct
    {
        public string Key { get; set; }
        public string GameName { get; set; }
        public string Platform { get; set; }
        public int GameID { get; set; }

        public KeyStruct(int gameID, string gameName, string key, string platform)
        {
            Key = key;
            GameName = gameName;
            GameID = gameID;
            Platform = platform;
        }
    }

    public void getKeys(int userID)
    {
        if (keys != null)
        {
            keys.Clear();
        }

        string query = "SELECT k.KeyValue, k.GameID, g.Name, g.Platform " +
            "FROM Purchase p " +
            "JOIN keys_t k ON p.KeyID = k.KeyID " +
            "JOIN game g ON k.GameID = g.GameID " +
            $"WHERE p.UserID = {userID};";

        DBConnector connector = DBConnector.getInstance();

        using (MySqlCommand command = new MySqlCommand(query, connector.databaseConnection))
        {
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int gameID = int.Parse(reader["GameID"].ToString());
                    string keyValue = reader["KeyValue"].ToString();
                    string gameName = reader["Name"].ToString();
                    string platform = reader["Platform"].ToString();

                    KeyStruct key = new KeyStruct(gameID, gameName, keyValue, platform);

                    keys.Add(key);
                }
            }

            reader.Close();
        }
    }
}