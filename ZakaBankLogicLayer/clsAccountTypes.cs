using System;
using System.Data;
using System.Threading.Tasks;
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

        private async Task<bool> _AddNewAccountTypeAsync()
        {
            this.AccountTypeID = await clsAccountTypeData.AddNewAccountTypeAsync(Name, Description);
            return (this.AccountTypeID != -1);
        }

        private async Task<bool> _UpdateAccountTypeAsync()
        {
            return await clsAccountTypeData.UpdateAccountTypeAsync(AccountTypeID, Name, Description);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewAccountTypeAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateAccountTypeAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsAccountTypes> FindByAccountTypeIDAsync(int accountTypeID)
        {
            var dt = await clsLoginRegistersData.FindByID(accountTypeID);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsAccountTypes(
                                            Convert.ToInt32(row["AccountTypeID"]),
                                            Convert.ToString(row["Name"]),
                                            Convert.ToString(row["Description"])
                                          );
            }
            return null;
        }

        public static async Task<bool> DeleteAsync(int accountTypeID)
        {
            return await clsAccountTypeData.DeleteAccountTypeAsync(accountTypeID);
        }

        public static async Task<bool> ExistsByID(int accountTypeID)
        {
            return await clsAccountTypeData.AccountTypeExistsAsync(accountTypeID);
        }

        public static async Task<DataTable> GetAllAccountTypesAsync()
        {
            return await clsAccountTypeData.GetAllAccountTypesAsync();
        }

        public static async Task<(DataTable dataTable, int totalCount)> GetPagedAccountTypes(int pageNumber, int pageSize)
        {
            return await clsAccountTypeData.GetPagedAccountTypesAsync(pageNumber, pageSize);
        }
    }
}

