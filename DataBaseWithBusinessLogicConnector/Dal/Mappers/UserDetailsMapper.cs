using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class UserDetailsMapper
    {
        public IEnumerable<UserDetails> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalUserDetails> dataEntities)
        {
            var result = new List<UserDetails>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public UserDetails ConvertToBusinessLogicEntity(DalUserDetails dataEntity)
        {
            var result = new UserDetails(dataEntity.Id, dataEntity.LastName, dataEntity.FirstName);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalUserDetails> ConvertToDALEntitiesCollection(IEnumerable<UserDetails> dataEntities)
        {
            var result = new List<DalUserDetails>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalUserDetails ConvertToDALEntity(UserDetails businessEntity)
        {
            var result = new DalUserDetails(businessEntity.Id, businessEntity.LastName, businessEntity.FirstName);
            return result;
        }
    }
}
