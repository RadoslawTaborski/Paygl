namespace DataBaseWithBusinessLogicConnector.Interfaces
{
    public interface IEntity
    {
        int? Id { get; }
        bool IsDirty { get; set; }

        void UpdateId(int? id);
    }
}
