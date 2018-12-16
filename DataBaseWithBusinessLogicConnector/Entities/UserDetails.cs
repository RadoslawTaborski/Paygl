using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class UserDetails : IEntity
    {
        public int Id { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public bool IsDirty { get; private set; }

        public UserDetails(int id, string lastName, string firstName)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
        }
    }
}
