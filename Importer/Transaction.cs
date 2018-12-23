using System;

namespace Importer
{
    public class Transaction
    {
        public DateTime DateTime { get; private set; }
        public string ContractorData { get; private set; }
        public string Title { get; private set; }
        public string AccountNumber { get; private set; }
        public string BankName { get; private set; }
        public string Details { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Transaction(DateTime dateTime, string contractorData, string title, string accountNumber, string bankName, string details, decimal amount, string currency)
        {
            DateTime = dateTime;
            ContractorData = contractorData;
            Title = title;
            AccountNumber = accountNumber;
            BankName = bankName;
            Details = details;
            Amount = amount;
            Currency = currency;
        }
    }
}