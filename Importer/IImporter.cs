using System.Collections.Generic;

namespace Importer
{
    public interface IImporter
    {
        IEnumerable<Transaction> ReadTransactions();
    }
}