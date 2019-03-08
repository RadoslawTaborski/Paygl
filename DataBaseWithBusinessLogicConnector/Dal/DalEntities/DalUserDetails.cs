using DataBaseWithBusinessLogicConnector.Interfaces.Dal;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalUserDetails : IDalEntity
    {
        public int? Id { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }

        public DalUserDetails(int? id, string lastName, string firstName)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
        }
    }
}
