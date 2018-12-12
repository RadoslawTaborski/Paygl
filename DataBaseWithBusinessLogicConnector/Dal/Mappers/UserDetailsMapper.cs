using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class UserDetailsMapper : IMapper<UserDetails, DalUserDetails>
    {
        public IEnumerable<UserDetails> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalUserDetails> dataEntities)
        {
            throw new NotImplementedException();
        }

        public UserDetails ConvertToBusinessLogicEntity(DalUserDetails dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalUserDetails> ConvertToDALEntitiesCollection(IEnumerable<UserDetails> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalUserDetails ConvertToDALEntity(UserDetails businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}
