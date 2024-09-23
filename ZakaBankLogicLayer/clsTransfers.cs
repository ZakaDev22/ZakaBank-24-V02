using System;
using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsTransfers
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TransferID { get; set; }
        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public string Description { get; set; }
        public int AddedByUserID { get; set; }

        public clsTransfers()
        {
            Mode = enMode.AddNew;
        }

        public clsTransfers(int transferID, int fromAccountID, int toAccountID, decimal amount, DateTime transferDate, string description, int addedByUserID)
        {
            TransferID = transferID;
            FromAccountID = fromAccountID;
            ToAccountID = toAccountID;
            Amount = amount;
            TransferDate = transferDate;
            Description = description;
            AddedByUserID = addedByUserID;
            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewTransferAsync()
        {
            this.TransferID = await clsTransferData.AddNewTransferAsync(FromAccountID, ToAccountID, Amount, Description, AddedByUserID);
            return (this.TransferID != -1);
        }

        private async Task<bool> _UpdateTransferAsync()
        {
            return await clsTransferData.UpdateTransferAsync(TransferID, FromAccountID, ToAccountID, Amount, Description, AddedByUserID);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewTransferAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateTransferAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsTransfers> FindByTransferIDAsync(int transferID)
        {
            var dt = await clsTransferData.FindTransferByIDAsync(transferID);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                return new clsTransfers(Convert.ToInt32(row["TransferID"]),
                                        Convert.ToInt32(row["FromAccountID"]),
                                        Convert.ToInt32(row["ToAccountID"]),
                                        Convert.ToDecimal(row["Amount"]),
                                        Convert.ToDateTime(row["TransferDate"]),
                                        Convert.ToString(row["Description"]),
                                        Convert.ToInt32(row["AddedByUserID"])
                                          );
            }
            else
                return null;
        }

        public static async Task<bool> DeleteAsync(int transferID)
        {
            return await clsTransferData.DeleteTransferAsync(transferID);
        }

        public static async Task<bool> ExistsByIDAsync(int transferID)
        {
            return await clsTransferData.TransferExistsAsync(transferID);
        }

        public static async Task<DataTable> GetAllTransfersAsync()
        {
            return await clsTransferData.GetAllTransfersAsync();
        }

        public static async Task<(DataTable dtaTable, int totalCount)> GetPagedTransfersAsync(int pageNumber, int pageSize)
        {
            return await clsTransferData.GetPagedTransfersAsync(pageNumber, pageSize);
        }
    }
}
