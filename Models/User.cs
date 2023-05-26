using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteelGames.Models
{
    public class User
    {
        protected static User instance;

        public int UserID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Administrator { get; set; }
        public bool Logged { get; set; }
        protected User() { }
        protected User(int userID, string email, string phoneNumber, bool logged)
        {
                    UserID = userID;
        Email = email;
        PhoneNumber = phoneNumber;
        Logged = logged;
        }

        public static User getInstance()
        {
            if(instance == null)
            {
                instance = new User();
                instance.Logged = false;
            }

            return instance;
        }

        public void Logout()
        {
            Logged = false;
            UserID = 0;
            Email = "";
            PhoneNumber = "";
            Administrator = false;
        }

        public static void SetClient(int clientID, string email, string phoneNumber, DateTime registrationDate)
        {
            instance = new Client(clientID, email, phoneNumber, true, registrationDate);
        }

        public static void SetAdmin(int adminID, string email, string phoneNumber, string position)
        {
            instance = new Admin(adminID, email, phoneNumber, true, position);
        }

    }

    public class Client : User
    {
        public DateTime RegistrationDate { get; set; }
        public Client(int userID, string email, string phoneNumber, bool logged, DateTime registrationDate) :
                base(userID, email, phoneNumber, logged)
        {
            RegistrationDate = registrationDate;
            Administrator = false;
        }
    }

    public class Admin : User
    {
        public string Position { get; set; }
        public Admin(int userID, string email, string phoneNumber, bool logged, string position) :
                base(userID, email, phoneNumber, logged)
        {
            Position = position;
            Administrator = true;
        }
    }
}
