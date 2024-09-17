using System;
using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsPeople
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public short? Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public int? CountryID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public clsPeople()
        {
            Mode = enMode.AddNew;
        }

        public clsPeople(int personID, string firstName, string lastName, DateTime? dateOfBirth, short? gender,
                         string address, string phone, string email, string imagePath, int? countryID, DateTime? createdDate, DateTime? updatedDate)
        {
            PersonID = personID;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            ImagePath = imagePath;
            CountryID = countryID;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewPersonAsync()
        {
            this.PersonID = await clsPeopleData.AddNewPersonAsync(FirstName, LastName, DateOfBirth, Gender, Address, Phone, Email, ImagePath, CountryID);
            return (this.PersonID != -1);
        }

        private async Task<bool> _UpdatePersonAsync()
        {
            return await clsPeopleData.UpdatePersonAsync(PersonID, FirstName, LastName, DateOfBirth, Gender, Address, Phone, Email, ImagePath, CountryID);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (await _AddNewPersonAsync())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        break;
                    }
                case enMode.Update:
                    return await _UpdatePersonAsync();
                default:
                    throw new InvalidOperationException("Unknown mode.");
            }
            return false;
        }

        public static async Task<clsPeople> FindByPersonIDAsync(int personID)
        {
            DataTable dt = await clsPeopleData.FindPersonByIDAsync(personID);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new clsPeople(
                    Convert.ToInt32(row["PersonID"]),
                    Convert.ToString(row["FirstName"]),
                    Convert.ToString(row["LastName"]),
                    row["DateOfBirth"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["DateOfBirth"]) : null,
                    row["Gender"] != DBNull.Value ? (short?)Convert.ToInt16(row["Gender"]) : null,
                    Convert.ToString(row["Address"]),
                    Convert.ToString(row["PhoneNumber"]),
                    Convert.ToString(row["Email"]),
                    Convert.ToString(row["ImagePath"]),
                    row["CountryID"] != DBNull.Value ? (int?)Convert.ToInt32(row["CountryID"]) : null,
                    row["CreatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["CreatedDate"]) : null,
                    row["UpdatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["UpdatedDate"]) : null
                );
            }
            return null;
        }

        public static async Task<bool> DeletePersonAsync(int personID)
        {
            return await clsPeopleData.DeletePersonAsync(personID);
        }

        public static async Task<bool> ExistsByIDAsync(int personID)
        {
            return await clsPeopleData.PersonExistsAsync(personID);
        }

        public static async Task<DataTable> GetAllPeopleAsync()
        {
            return await clsPeopleData.GetAllPeopleAsync();
        }

        public static async Task<(DataTable dataTable, int TotalCount)> GetPagedPeopleAsync(int pageNumber, int pageSize)
        {
            return await clsPeopleData.GetPeopleByPageAsync(pageNumber, pageSize);
        }

    }
}
