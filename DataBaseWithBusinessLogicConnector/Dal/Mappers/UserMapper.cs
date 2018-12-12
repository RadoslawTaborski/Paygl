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
        public IEnumerable<User> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalUser> dataEntities)
        {
            throw new NotImplementedException();
        }

        public User ConvertToBusinessLogicEntity(DalUser dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalUser> ConvertToDALEntitiesCollection(IEnumerable<User> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalUser ConvertToDALEntity(User businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}
