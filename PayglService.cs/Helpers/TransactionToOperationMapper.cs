using DataBaseWithBusinessLogicConnector.Entities;
using Importer;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayglService.cs.Helpers
{
    internal class TransactionToOperationMapper
    {
        public IEnumerable<Operation> ConvertToEntitiesCollection(IEnumerable<Transaction> transactions, User user, List<TransactionType> transactionsType, List<TransferType> transfersType)
        {
            var result = new List<Operation>();
            foreach (var item in transactions)
            {
                result.Add(Convert(item,user,transactionsType,transfersType));
            }

            return result;
        }

        public Operation Convert(Transaction transaction, User user, List<TransactionType> transactionsType, List<TransferType> transfersType)
        {
            if (transaction.Amount < 0)
            {
                return new Operation(null, null, user, transaction.ContractorData+Environment.NewLine+transaction.Title, -1* transaction.Amount, transactionsType[1], transfersType[1], null,null,transaction.DateTime,"");
            }
            else
            {
                return new Operation(null, null, user, transaction.ContractorData + Environment.NewLine + transaction.Title, transaction.Amount, transactionsType[0], transfersType[1], null, null, transaction.DateTime, "");
            }
        }
    }
}
