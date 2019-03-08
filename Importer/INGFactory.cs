namespace Importer
{
    internal class IngFactory : ImportFactory
    {
        public override IImporter CreateImporter()
        {
            return new IngImporter();
        }
    }
}