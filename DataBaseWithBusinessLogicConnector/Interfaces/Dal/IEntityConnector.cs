using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Interfaces.Dal
{
    public interface IEntityConnector <DalEntity>
    {
        IEnumerable<DalEntity> GetAll(string filter);
        DalEntity GetById(int id);
        void Insert(DalEntity entity);
        void Update(int id, DalEntity entity);
        void Delete(int id);
    }
}
