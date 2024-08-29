using System;
using System.Data;
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

        private bool _AddNewClient()
        {
            this.ClientID = clsClientsData.AddNewClient(PersonID, AccountNumber, PinCode, Balance, AddedByUserID, AccountTypeID);
            return (this.ClientID != -1);
        }

        private bool _UpdateClient()
        {
            return clsClientsData.UpdateClient(ClientID, PersonID, AccountNumber, PinCode, Balance, AddedByUserID, AccountTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewClient())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateClient();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsClients FindByClientID(int clientID)
        {
            int personID = 0;
            string accountNumber = string.Empty;
            string pinCode = string.Empty;
            decimal balance = 0;
            int addedByUserID = 0;
            int accountTypeID = 0;
            bool isDeleted = false;

            bool isFound = clsClientsData.FindClientByID(clientID, ref personID, ref accountNumber, ref pinCode, ref balance, ref addedByUserID, ref accountTypeID, ref isDeleted);

            if (isFound)
                return new clsClients(clientID, personID, accountNumber, pinCode, balance, addedByUserID, accountTypeID, isDeleted);
            else
                return null;
        }

        public static bool Delete(int clientID)
        {
            return clsClientsData.DeleteClient(clientID);
        }

        public static bool ExistsByID(int clientID)
        {
            return clsClientsData.ClientExists(clientID);
        }

        public static DataTable GetAllClients()
        {
            return clsClientsData.GetAllClients();
        }

        public static DataTable GetPagedClients(int pageNumber, int pageSize, out int totalCount)
        {
            return clsClientsData.GetPagedClients(pageNumber, pageSize, out totalCount);
        }
    }
}


