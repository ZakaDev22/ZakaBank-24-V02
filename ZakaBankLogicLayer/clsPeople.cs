using System;
using System.Data;

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
        public DateTime DateOfBirth { get; set; }
        public short Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public int CountryID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public clsPeople()
        {
            Mode = enMode.AddNew;
        }

        public clsPeople(int personID, string firstName, string lastName, DateTime dateOfBirth, short gender,
                         string address, string phone, string email, string imagePath, int countryID, DateTime createdDate, DateTime updatedDate)
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

        private bool _AddNewPerson()
        {

            this.PersonID = clsPeopleData.AddNewPerson(FirstName, LastName, DateOfBirth, Gender,
                                                       Address, Phone, Email, ImagePath, CountryID);
            return (this.PersonID != -1);

        }

        private bool _UpdatePerson()
        {

            return clsPeopleData.UpdatePerson(PersonID, FirstName, LastName, DateOfBirth, Gender,
                                                     Address, Phone, Email, ImagePath, CountryID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewPerson())
                        {
                            Mode = enMode.Update;
                            return true;
                        }

                        break;
                    }
                case enMode.Update:
                    return _UpdatePerson();
                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsPeople FindByPersonID(int personID)
        {
            string FirstName = string.Empty;
            string LastName = string.Empty;
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string ImagePath = string.Empty;
            int countryID = 0;

            bool IsFound = clsPeopleData.FindPersonByID(personID, ref FirstName, ref LastName, ref DateOfBirth,
                                                        ref Gender, ref Address, ref Phone, ref Email, ref ImagePath, ref countryID);

            if (IsFound)
                return new clsPeople(personID, FirstName, LastName, DateOfBirth, Gender, Address, Phone, Email, ImagePath, countryID, DateTime.Now, DateTime.Now);
            else
                return null;
        }



        public static bool Delete(int personID)
        {
            return clsPeopleData.DeletePerson(personID);
        }

        public static bool ExistsByID(int personID)
        {
            return clsPeopleData.PersonExists(personID);
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
        }

        public static DataTable GetPagedPeople(int pageNumber, int pageSize, out int totalCount)
        {
            return clsPeopleData.GetPagedPeople(pageNumber, pageSize, out totalCount);
        }

    }
}
