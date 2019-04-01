using DataBaseWithBusinessLogicConnector.Interfaces.Dal;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalUser : IDalEntity
    {
        public int? Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public int? LanguageId { get; private set; }
        public int? DetailsId { get; private set; }

        public DalUser(int? id, string login, string password, int? languageId, int? detailsId)
        {
            Id = id;
            Login = login;
            Password = password;
            LanguageId = languageId;
            DetailsId = detailsId;
        }
    }
}
