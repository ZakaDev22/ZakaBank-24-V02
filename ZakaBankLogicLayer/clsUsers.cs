﻿using System;
using System.Data;
using System.Threading.Tasks;
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
        public DateTime? UpdatedDate { get; set; }
        public int Permissions { get; set; }
        public int? AddedByUserID { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// If You Want to Use This Future Then Call The LoadPersonInfo Method First After You Create an Object Of Users Class.
        /// </summary>
        public clsPeople _PersonInfo;

        public clsUsers()
        {
            Mode = enMode.AddNew;
        }

        public clsUsers(int id, int personID, string userName, string passwordHash, DateTime createdDate, DateTime? updatedDate, int permissions, int? addedByUserID, bool isActive)
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

        }

        public async Task LoadPersonInfo()
        {
            _PersonInfo = await clsPeople.FindByPersonIDAsync(PersonID);
        }

        private async Task<bool> _AddNewUser()
        {
            this.ID = await clsUsersData.AddNewUser(PersonID, UserName, PassWordHash, CreatedDate, Permissions, AddedByUserID);
            return (this.ID != -1);
        }

        private async Task<bool> _UpdateUser()
        {
            return await clsUsersData.UpdateUser(ID, UserName, PassWordHash, Permissions);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateUser();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsUsers> FindByUserIDAsync(int id)
        {
            DataTable dt = await clsUsersData.FindUserByID(id);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new clsUsers(
                                     Convert.ToInt32(row["UserID"]),
                                     Convert.ToInt32(row["PersonID"]),
                                     Convert.ToString(row["UserName"]),
                                     Convert.ToString(row["PasswordHash"]),
                                     Convert.ToDateTime(row["CreatedDate"]),
                                     row["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["UpdatedDate"]),
                                     Convert.ToInt32(row["Permissions"]),
                                     row["AddedByUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["AddedByUserID"]),
                                     Convert.ToBoolean(row["IsActive"])



                );
            }
            return null;
        }

        public static async Task<clsUsers> FindByUserNameAndPassword(string UserName, string Password)
        {
            DataTable dt = await clsUsersData.FindByUserNameAndPassword(UserName, Password);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new clsUsers(
                                     Convert.ToInt32(row["UserID"]),
                                     Convert.ToInt32(row["PersonID"]),
                                     Convert.ToString(row["UserName"]),
                                     Convert.ToString(row["PasswordHash"]),
                                     Convert.ToDateTime(row["CreatedDate"]),
                                     row["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["UpdatedDate"]),
                                     Convert.ToInt32(row["Permissions"]),
                                     row["AddedByUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["AddedByUserID"]),
                                     Convert.ToBoolean(row["IsActive"])



                );
            }
            return null;
        }


        public static async Task<bool> DeleteAsync(int id)
        {
            return await clsUsersData.DeleteUser(id);
        }

        public static async Task<bool> ExistsByIDAsync(int id)
        {
            return await clsUsersData.UserExists(id);
        }

        public static async Task<DataTable> GetAllUsers()
        {
            return await clsUsersData.GetAllUsers();
        }

        public static async Task<(DataTable dataTable, int TotalCount)> GetPagedUsers(int pageNumber, int pageSize)
        {
            return await clsUsersData.GetPagedUsers(pageNumber, pageSize);
        }
    }
}
