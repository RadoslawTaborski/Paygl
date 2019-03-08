using DataBaseWithBusinessLogicConnector.Entities;
using Importer;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PayglService.cs.Helpers
{
    internal class TransactionToOperationMapper
    {
        public IEnumerable<Operation> ConvertToEntitiesCollection(IEnumerable<Transaction> transactions, User user, List<Importance> importances, List<Frequency> frequencies, List<Tag> tags, List<TransactionType> transactionsType, List<TransferType> transfersType)
        {
            var result = new List<Operation>();
            foreach (var item in transactions)
            {
                result.Add(Convert(item,user,importances, frequencies, tags, transactionsType,transfersType));
            }

            return result;
        }

        public Operation Convert(Transaction transaction, User user, List<Importance> importances, List<Frequency> frequencies, List<Tag> tags, List<TransactionType> transactionsType, List<TransferType> transfersType)
        {
            Operation result;
            var schematic = FindSchematicInPattern(transaction);

            if (transaction.Amount < 0)
            {
                result = new Operation(null, null, user, transaction.ContractorData + Environment.NewLine + transaction.Title, -1 * transaction.Amount, transactionsType[1], transfersType[1], null, null, transaction.DateTime, "");
            }
            else
            {
                result = new Operation(null, null, user, transaction.ContractorData + Environment.NewLine + transaction.Title, transaction.Amount, transactionsType[0], transfersType[1], null, null, transaction.DateTime, "");
            }

            if (schematic != null)
            {
                result.SetShortDescription(schematic.Description);
                result.SetFrequency(ConvertStringHelper.ConvertToFrequency(schematic.Frequency, frequencies));
                result.SetImportance(ConvertStringHelper.ConvertToImportance(schematic.Importance, importances));
                var tagsList = new List<RelTag>();
                foreach(var tagString in schematic.Tags)
                {
                    var tag = ConvertStringHelper.ConvertToTag(tagString, tags);
                    if ( tag != null)
                    {
                        tagsList.Add(new RelTag(null, tag, result.Id));
                    }
                }
                result.SetTags(tagsList);
            }

            return result;
        }


        private Schematic FindSchematicInPattern(Transaction transaction)
        {
            var patterns = ConfigurationManager.SchematicTransaction();

            foreach (var pattern in patterns)
            {
                if (pattern.DescriptionRegex == "")
                {
                    pattern.DescriptionRegex = ".*";
                }

                if (pattern.TitleRegex == "")
                {
                    pattern.TitleRegex = ".*";
                }

                if (Regex.Match(transaction.ContractorData, pattern.DescriptionRegex).Success && Regex.Match(transaction.Title, pattern.TitleRegex).Success)
                {
                    return pattern;
                }
            }
            return null;
        }
    }
}
