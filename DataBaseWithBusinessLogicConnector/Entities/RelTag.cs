using DataBaseWithBusinessLogicConnector.Interfaces;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class RelTag: IEntity
    {
        public int? Id { get; private set; }
        public Tag Tag { get; private set; }
        public int? RelatedId { get; private set; }
        public bool IsDirty { get; set; }
        public bool IsMarkForDeletion { get; set; }

        public RelTag(int? id, Tag tag, int? operationId)
        {
            Id = id;
            Tag = tag;
            RelatedId = operationId;
            IsDirty = true;
            IsMarkForDeletion = false;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Tag.Text;
        }
    }
}
