﻿using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

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
