using System;
using System.Data;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsUsers
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string PassWordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Permissions { get; set; }
        public int AddedByUserID { get; set; }

        public bool IsActive { get; set; }

        public clsPeople _PersonInfo;

        public clsUsers()
        {
            Mode = enMode.AddNew;
        }

        public clsUsers(int id, int personID, string userName, string passwordHash, DateTime createdDate, DateTime updatedDate, int permissions, int addedByUserID, bool isActive)
        {
            ID = id;
            PersonID = personID;
            UserName = userName;
            PassWordHash = passwordHash;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            Permissions = permissions;
            AddedByUserID = addedByUserID;
            Mode = enMode.Update;
            IsActive = isActive;

            _PersonInfo = clsPeople.FindByPersonID(PersonID);
        }

        private bool _AddNewUser()
        {
            this.ID = clsUsersData.AddNewUser(PersonID, UserName, PassWordHash, CreatedDate, UpdatedDate, Permissions, AddedByUserID);
            return (this.ID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(ID, PersonID, UserName, PassWordHash, CreatedDate, UpdatedDate, Permissions, AddedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateUser();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsUsers FindByUserID(int id)
        {
            int personID = 0;
            string userName = string.Empty;
            string passwordHash = string.Empty;
            DateTime createdDate = DateTime.MinValue;
            DateTime updatedDate = DateTime.MinValue;
            int permissions = 0;
            int addedByUserID = 0;
            bool isActive = false;

            bool isFound = clsUsersData.FindUserByID(id, ref personID, ref userName, ref passwordHash, ref createdDate, ref updatedDate, ref permissions, ref addedByUserID, ref isActive);

            if (isFound)
                return new clsUsers(id, personID, userName, passwordHash, createdDate, updatedDate, permissions, addedByUserID, isActive);
            else
                return null;
        }

        public static clsUsers FindByUserNameAndPassword(string UserName, string Password)
        {
            int userID = 0;
            int personID = 0;
            DateTime createdDate = DateTime.MinValue;
            DateTime updatedDate = DateTime.MinValue;
            int permissions = 0;
            int addedByUserID = 0;
            bool isActive = false;

            bool isFound = clsUsersData.FindByUserNameAndPassword(UserName, Password, ref userID, ref personID, ref createdDate, ref updatedDate, ref permissions, ref addedByUserID, ref isActive);

            if (isFound)
                return new clsUsers(userID, personID, UserName, Password, createdDate, updatedDate, permissions, addedByUserID, isActive);
            else
                return null;
        }


        public static bool Delete(int id)
        {
            return clsUsersData.DeleteUser(id);
        }

        public static bool ExistsByID(int id)
        {
            return clsUsersData.UserExists(id);
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static DataTable GetPagedUsers(int pageNumber, int pageSize, out int totalCount)
        {
            return clsUsersData.GetPagedUsers(pageNumber, pageSize, out totalCount);
        }
    }
}
