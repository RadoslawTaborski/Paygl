using System;
using System.Collections.Generic;
using System.Text;

namespace Importer
{
    public abstract class ImportFactory
    {
        public static ImportFactory GetFactory(string type)
        {
            switch (type)
            {
                case "ING":
                    return new INGFactory();
                default:
                    throw new NotImplementedException();
            }
        }

        public abstract IImporter CreateImporter();
    }
}
