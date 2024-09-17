using System;
using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsLoginRegisters
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime? LogOutDateTime { get; set; }

        public clsLoginRegisters()
        {
            Mode = enMode.AddNew;
        }

        public clsLoginRegisters(int id, int userID, DateTime loginDateTime, DateTime? logOutDateTime)
        {
            ID = id;
            UserID = userID;
            LoginDateTime = loginDateTime;
            LogOutDateTime = logOutDateTime;
            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewLoginRegisterAsync()
        {
            this.ID = await clsLoginRegistersData.InsertLoginRegister(UserID, LoginDateTime);

            return (this.ID != -1);
        }

        // i will call this method the moment The Current User Will LogOut From The System to Save The Logout Time
        private async Task<bool> _UpdateLoginRegisterAsync()
        {
            return await clsLoginRegistersData.UpdateLoginRegister(UserID);
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewLoginRegisterAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateLoginRegisterAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsLoginRegisters> FindByID(int id)
        {
            var dt = await clsLoginRegistersData.FindByID(id);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsLoginRegisters(
                    Convert.ToInt32(row["LoginRegisterID"]),
                    Convert.ToInt32(row["UserID"]),
                    Convert.ToDateTime(row["LoginDateTime"]),
                    row["LogOutDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LogOutDateTime"])
                );
            }
            return null;
        }

        public static async Task<DataTable> FindByUserIDAsync(int userID)
        {
            return await clsLoginRegistersData.FindByUserID(userID);
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            return await clsLoginRegistersData.DeleteLoginRegister(id);
        }

        public static async Task<bool> ExistsByIDAsync(int id)
        {
            var dt = await clsLoginRegistersData.FindByID(id);
            return dt.Rows.Count > 0;
        }

        public static async Task<DataTable> GetAllLoginRegisters()
        {
            return await clsLoginRegistersData.GetAllLoginRegisters();
        }

        public static async Task<(DataTable dataTable, int TotalCount)> GetPagedLoginRegisters(int pageNumber, int pageSize)
        {
            return await clsLoginRegistersData.GetPagedLoginRegisters(pageNumber, pageSize);
        }
    }
}
