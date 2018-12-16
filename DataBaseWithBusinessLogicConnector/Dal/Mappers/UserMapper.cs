using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class UserMapper : IMapper<User, DalUser>
    {
        private UserDetails _details;
        public UserMapper(UserDetails details)
        {
            _details = details;
        }

        public void Update(UserDetails details)
        {
            _details = details;
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
            var result = new User(dataEntity.Id, dataEntity.Login, dataEntity.Password, _details);
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
            var result = new DalUser(businessEntity.Id, businessEntity.Login, businessEntity.Password, businessEntity.Details.Id);
            return result;
        }
    }
}
