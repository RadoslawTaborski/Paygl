using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseWithBusinessLogicConnector.Interfaces.Dal
{
    public interface IMapper<BusinesEntity,DalEntity>
    {
        BusinesEntity ConvertToBusinessLogicEntity(DalEntity dataEntity);
        IEnumerable<BusinesEntity> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalEntity> dataEntities);
        DalEntity ConvertToDALEntity(BusinesEntity businessEntity);
        IEnumerable<DalEntity> ConvertToDALEntitiesCollection(IEnumerable<BusinesEntity> dataEntities);
    }
}
