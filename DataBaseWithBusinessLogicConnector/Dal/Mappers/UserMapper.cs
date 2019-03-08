using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class UserMapper
    {
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
            var result = new User(dataEntity.Id, dataEntity.Login, dataEntity.Password, null);
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
            var result = new DalUser(businessEntity.Id, businessEntity.Login, businessEntity.Password, businessEntity.Details!=null? businessEntity.Details.Id:0);
            return result;
        }
    }
}
