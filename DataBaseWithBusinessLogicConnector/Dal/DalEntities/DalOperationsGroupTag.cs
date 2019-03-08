namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperationsGroupTag
    {
        public int? Id { get; private set; }
        public int? OperationsGroupId { get; private set; }
        public int? TagId { get; private set; }

        public DalOperationsGroupTag(int? id, int? operationsGroupId, int? tagId)
        {
            Id = id;
            OperationsGroupId = operationsGroupId;
            TagId = tagId;
        }
    }
}
