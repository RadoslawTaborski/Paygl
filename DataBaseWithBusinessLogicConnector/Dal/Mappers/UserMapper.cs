using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class UserMapper
    {
        private List<Language> _languages;

        public void Update(List<Language> languages)
        {
            _languages = languages;
        }

        public IEnumerable<User> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalUser> dataEntities)
        {
            var result = new List<User>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public User ConvertToBusinessLogicEntity(DalUser dataEntity)
        {
            var language = _languages.First(t => t.Id == dataEntity.LanguageId);
            var result = new User(dataEntity.Id, dataEntity.Login, dataEntity.Password, language, null);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalUser> ConvertToDALEntitiesCollection(IEnumerable<User> dataEntities)
        {
            var result = new List<DalUser>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalUser ConvertToDALEntity(User businessEntity)
        {
            var result = new DalUser(businessEntity.Id, businessEntity.Login, businessEntity.Password, businessEntity.Language.Id, businessEntity.Details!=null? businessEntity.Details.Id:0);
            return result;
        }
    }
}
