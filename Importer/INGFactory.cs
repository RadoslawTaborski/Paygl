namespace Importer
{
    internal class INGFactory : ImportFactory
    {
        public override IImporter CreateImporter()
        {
            return new INGImporter();
        }
    }
}