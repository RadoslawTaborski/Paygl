using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Interfaces.Dal
{
    public interface IMapper<TBusinessEntity,TDalEntity>
    {
        TBusinessEntity ConvertToBusinessLogicEntity(TDalEntity dataEntity);
        IEnumerable<TBusinessEntity> ConvertToBusinessLogicEntitiesCollection(IEnumerable<TDalEntity> dataEntities);
        TDalEntity ConvertToDALEntity(TBusinessEntity businessEntity);
        IEnumerable<TDalEntity> ConvertToDALEntitiesCollection(IEnumerable<TBusinessEntity> dataEntities);
    }
}
