using System;
using System.Data;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsTransactions
    {


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TransactionID { get; set; }
        public int ClientID { get; set; }
        public decimal Amount { get; set; }
        public int TransactionTypeID { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AddedByUserID { get; set; }

        public clsTransactions()
        {
            Mode = enMode.AddNew;
        }

        public clsTransactions(int transactionID, int clientID, decimal amount, int transactionTypeID, string description, DateTime transactionDate, int addedByUserID)
        {
            TransactionID = transactionID;
            ClientID = clientID;
            Amount = amount;
            TransactionTypeID = transactionTypeID;
            Description = description;
            TransactionDate = transactionDate;
            AddedByUserID = addedByUserID;
            Mode = enMode.Update;
        }

        private bool _AddNewTransaction()
        {
            this.TransactionID = clsTransactionData.AddNewTransaction(ClientID, Amount, TransactionTypeID, Description, TransactionDate, AddedByUserID);
            return (this.TransactionID != -1);
        }

        private bool _UpdateTransaction()
        {
            return clsTransactionData.UpdateTransaction(TransactionID, ClientID, Amount, TransactionTypeID, Description, TransactionDate, AddedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTransaction())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateTransaction();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsTransactions FindByTransactionID(int transactionID)
        {
            int clientID = 0;
            decimal amount = 0;
            int transactionTypeID = 0;
            string description = string.Empty;
            DateTime transactionDate = DateTime.MinValue;
            int addedByUserID = 0;

            bool isFound = clsTransactionData.FindTransactionByID(transactionID, ref clientID, ref amount, ref transactionTypeID, ref description, ref transactionDate, ref addedByUserID);

            if (isFound)
                return new clsTransactions(transactionID, clientID, amount, transactionTypeID, description, transactionDate, addedByUserID);
            else
                return null;
        }

        public static bool Delete(int transactionID)
        {
            return clsTransactionData.DeleteTransaction(transactionID);
        }

        public static bool ExistsByID(int transactionID)
        {
            return clsTransactionData.TransactionExists(transactionID);
        }

        public static DataTable GetAllTransactions()
        {
            return clsTransactionData.GetAllTransactions();
        }

        public static DataTable GetPagedTransactions(int pageNumber, int pageSize, out int totalCount)
        {
            return clsTransactionData.GetPagedTransactions(pageNumber, pageSize, out totalCount);
        }
    }
}
