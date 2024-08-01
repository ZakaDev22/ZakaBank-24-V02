using System;
using System.Data;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsAccountTypes
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int AccountTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public clsAccountTypes()
        {
            Mode = enMode.AddNew;
        }

        public clsAccountTypes(int accountTypeID, string name, string description)
        {
            AccountTypeID = accountTypeID;
            Name = name;
            Description = description;
            Mode = enMode.Update;
        }

        private bool _AddNewAccountType()
        {
            this.AccountTypeID = clsAccountTypeData.AddNewAccountType(Name, Description);
            return (this.AccountTypeID != -1);
        }

        private bool _UpdateAccountType()
        {
            return clsAccountTypeData.UpdateAccountType(AccountTypeID, Name, Description);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAccountType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateAccountType();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsAccountTypes FindByAccountTypeID(int accountTypeID)
        {
            string name = string.Empty;
            string description = string.Empty;

            bool isFound = clsAccountTypeData.FindAccountTypeByID(accountTypeID, ref name, ref description);

            if (isFound)
                return new clsAccountTypes(accountTypeID, name, description);
            else
                return null;
        }

        public static bool Delete(int accountTypeID)
        {
            return clsAccountTypeData.DeleteAccountType(accountTypeID);
        }

        public static bool ExistsByID(int accountTypeID)
        {
            return clsAccountTypeData.AccountTypeExists(accountTypeID);
        }

        public static DataTable GetAllAccountTypes()
        {
            return clsAccountTypeData.GetAllAccountTypes();
        }

        public static DataTable GetPagedAccountTypes(int pageNumber, int pageSize, out int totalCount)
        {
            return clsAccountTypeData.GetPagedAccountTypes(pageNumber, pageSize, out totalCount);
        }
    }
}

