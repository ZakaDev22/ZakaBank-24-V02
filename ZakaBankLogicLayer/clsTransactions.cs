using System;
using System.Data;
using System.Threading.Tasks;
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

        private async Task<bool> _AddNewTransactionAsync()
        {
            this.TransactionID = await clsTransactionData.AddNewTransactionAsync(ClientID, Amount, TransactionTypeID, Description, TransactionDate, AddedByUserID);
            return (this.TransactionID != -1);
        }

        private async Task<bool> _UpdateTransactionAsync()
        {
            return await clsTransactionData.UpdateTransactionAsync(TransactionID, ClientID, Amount, TransactionTypeID, Description, TransactionDate, AddedByUserID);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewTransactionAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateTransactionAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsTransactions> FindByTransactionIDAsync(int transactionID)
        {
            var dt = await clsTransactionData.FindTransactionByIDAsync(transactionID);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                return new clsTransactions(Convert.ToInt32(row["TransactionID"]),
                                             Convert.ToInt32(row["ClientID"]),
                                             Convert.ToInt32(row["Amount"]),
                                             Convert.ToInt32(row["TransactionTypeID"]),
                                             Convert.ToString(row["Description"]),
                                             Convert.ToDateTime(row["TransactionDate"]),
                                             Convert.ToInt32(row["AddedByUser"])
                                          );
            }
            else
                return null;
        }

        public static async Task<DataTable> FindByTransactionTypeByNameAsync(string transactionTypeName)
        {
            return await clsTransactionData.FindByTransactionTypeByNameAsync(transactionTypeName);

        }


        public static async Task<bool> DeleteAsync(int transactionID)
        {
            return await clsTransactionData.DeleteTransactionAsync(transactionID);
        }

        public static async Task<bool> ExistsByIDAsync(int transactionID)
        {
            return await clsTransactionData.TransactionExistsAsync(transactionID);
        }

        public static async Task<DataTable> GetAllTransactionsAsync()
        {
            return await clsTransactionData.GetAllTransactionsAsync();
        }

        public static async Task<DataTable> GetAllTransactionsTypesAsync()
        {
            return await clsTransactionData.GetAllTransactionsTypesAsync();
        }

        public static async Task<(DataTable dataTable, int totalCount)> GetPagedTransactionsAsync(int pageNumber, int pageSize)
        {
            return await clsTransactionData.GetPagedTransactionsAsync(pageNumber, pageSize);
        }
    }
}
