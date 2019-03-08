using DataBaseWithBusinessLogicConnector.Interfaces;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class OperationDetails : IEntity
    {
        public int? Id { get; private set; }
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsDirty { get; set; }

        public OperationDetails(int? id, string name, double quantity, decimal amount)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Amount = amount;
            IsDirty = true;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }
    }
}
