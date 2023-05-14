using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteelGames.Models
{
    public class User
    {
        private static User instance;

        public int UserID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Administrator { get; set; }
        public bool Logged { get; set; }
        protected User() { }

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

    }

    public class Client : User
    {
        public DateTime registrationDate;
    }
}
