using System;
using System.Data;
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

        private bool _AddNewLoginRegister()
        {
            this.ID = clsLoginRegistersData.InsertLoginRegister(UserID, LoginDateTime);

            return (this.ID != -1);
        }

        // i will call this method the moment The Current User Will LogOut From The System to Save The Logout Time
        private bool _UpdateLoginRegister()
        {
            return clsLoginRegistersData.UpdateLoginRegister(UserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLoginRegister())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateLoginRegister();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsLoginRegisters FindByID(int id)
        {
            DataTable dt = clsLoginRegistersData.FindByID(id);
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

        public static DataTable FindByUserID(int userID)
        {
            return clsLoginRegistersData.FindByUserID(userID);
        }

        public static bool Delete(int id)
        {
            return clsLoginRegistersData.DeleteLoginRegister(id);
        }

        public static bool ExistsByID(int id)
        {
            DataTable dt = clsLoginRegistersData.FindByID(id);
            return dt.Rows.Count > 0;
        }

        public static DataTable GetAll()
        {
            return clsLoginRegistersData.GetAllLoginRegisters();
        }

        public static DataTable GetPaged(int pageNumber, int pageSize, out int totalCount)
        {
            return clsLoginRegistersData.GetPagedLoginRegisters(pageNumber, pageSize, out totalCount);
        }
    }
}
