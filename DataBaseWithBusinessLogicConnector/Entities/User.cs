﻿using DataBaseWithBusinessLogicConnector.Interfaces;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class User : IEntity
    {
        public int? Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public UserDetails Details { get; private set; }
        public bool IsDirty { get; set; }

        public User(int? id, string login, string password, UserDetails details)
        {
            Id = id;
            Login = login;
            Password = password;
            Details = details;
            IsDirty = true;
        }

        public void SetDetails(UserDetails userDetails)
        {
            Details = userDetails;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }
    }
}
