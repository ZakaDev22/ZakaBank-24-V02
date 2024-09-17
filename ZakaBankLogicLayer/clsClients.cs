using System;
using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsClients
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ClientID { get; set; }
        public int PersonID { get; set; }
        public string AccountNumber { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; set; }
        public int AddedByUserID { get; set; }
        public bool IsDeleted { get; set; }
        public int AccountTypeID { get; set; }

        public clsClients()
        {
            Mode = enMode.AddNew;
        }

        public clsClients(int clientID, int personID, string accountNumber, string pinCode, decimal balance, int addedByUserID, int accountTypeID, bool isDeleted)
        {
            ClientID = clientID;
            PersonID = personID;
            AccountNumber = accountNumber;
            PinCode = pinCode;
            Balance = balance;
            AddedByUserID = addedByUserID;
            AccountTypeID = accountTypeID;
            IsDeleted = isDeleted;
            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewClientAsync()
        {
            this.ClientID = await clsClientsData.AddNewClientAsync(PersonID, AccountNumber, PinCode, Balance, AddedByUserID, AccountTypeID);
            return (this.ClientID != -1);
        }

        private async Task<bool> _UpdateClientAsync()
        {
            return await clsClientsData.UpdateClientAsync(ClientID, PersonID, AccountNumber, PinCode, Balance, AddedByUserID, AccountTypeID);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewClientAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateClientAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsClients> FindByClientIDAsync(int clientID)
        {
            DataTable dt = await clsClientsData.FindClientByIDAsync(clientID);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return new clsClients(Convert.ToInt32(row["ClientID"]),
                                      Convert.ToInt32(row["PersonID"]),
                                      row["AccountNumber"].ToString(),
                                      row["PinCode"].ToString(),
                                      Convert.ToDecimal(row["Balance"]),
                                      Convert.ToInt32(row["AddedByUserID"]),
                                      Convert.ToInt32(row["AccountTypeID"]),
                                      Convert.ToBoolean(row["IsDeleted"]));
            }
            else
                return null;
        }

        public static async Task<bool> DeleteAsync(int clientID)
        {
            return await clsClientsData.DeleteClientAsync(clientID);
        }

        public static async Task<bool> ExistsByIDAsync(int clientID)
        {
            return await clsClientsData.ClientExistsAsync(clientID);
        }

        public static async Task<DataTable> GetAllClientsAsync()
        {
            return await clsClientsData.GetAllClientsAsync();
        }

        public static async Task<(DataTable dataTable, int totalCount)> GetPagedClientsAsync(int pageNumber, int pageSize)
        {
            var result = await clsClientsData.GetPagedClientsAsync(pageNumber, pageSize);
            return (result.Item1, result.Item2);
        }
    }
}


