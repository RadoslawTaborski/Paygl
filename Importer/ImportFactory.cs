using System;

namespace Importer
{
    public abstract class ImportFactory
    {
        public static ImportFactory GetFactory(string type)
        {
            switch (type)
            {
                case "ING":
                    return new IngFactory();
                default:
                    throw new NotImplementedException();
            }
        }

        public abstract IImporter CreateImporter();
    }
}
