using System;
using System.Data;
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

        private bool _AddNewTransfer()
        {
            this.TransferID = clsTransferData.AddNewTransfer(FromAccountID, ToAccountID, Amount, TransferDate, Description, AddedByUserID);
            return (this.TransferID != -1);
        }

        private bool _UpdateTransfer()
        {
            return clsTransferData.UpdateTransfer(TransferID, FromAccountID, ToAccountID, Amount, TransferDate, Description, AddedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTransfer())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateTransfer();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsTransfers FindByTransferID(int transferID)
        {
            int fromAccountID = 0;
            int toAccountID = 0;
            decimal amount = 0;
            DateTime transferDate = DateTime.MinValue;
            string description = string.Empty;
            int addedByUserID = 0;

            bool isFound = clsTransferData.FindTransferByID(transferID, ref fromAccountID, ref toAccountID, ref amount, ref transferDate, ref description, ref addedByUserID);

            if (isFound)
                return new clsTransfers(transferID, fromAccountID, toAccountID, amount, transferDate, description, addedByUserID);
            else
                return null;
        }

        public static bool Delete(int transferID)
        {
            return clsTransferData.DeleteTransfer(transferID);
        }

        public static bool ExistsByID(int transferID)
        {
            return clsTransferData.TransferExists(transferID);
        }

        public static DataTable GetAllTransfers()
        {
            return clsTransferData.GetAllTransfers();
        }

        public static DataTable GetPagedTransfers(int pageNumber, int pageSize, out int totalCount)
        {
            return clsTransferData.GetPagedTransfers(pageNumber, pageSize, out totalCount);
        }
    }
}
