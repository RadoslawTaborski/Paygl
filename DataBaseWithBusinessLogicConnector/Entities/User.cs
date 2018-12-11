using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public UserDetails Details { get; private set; }

        public User(int id, string login, string password, UserDetails details)
        {
            Id = id;
            Login = login;
            Password = password;
            Details = details;
        }
    }
}
