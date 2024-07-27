using System;
using System.Data;

using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsPeople
    {

        ///

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode { get; set; } = enMode.AddNew;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public clsPeople()
        {
            Mode = enMode.AddNew;
        }

        public clsPeople(int personID, string firstName, string lastName, string nationalNo, DateTime dateOfBirth, short gender,
                         string address, string phone, string email, string imagePath)
        {
            PersonID = personID;
            FirstName = firstName;
            LastName = lastName;
            NationalNo = nationalNo;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            ImagePath = imagePath;

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            try
            {
                if (clsPeopleData.PersonExistsByNationalNo(NationalNo))
                {
                    throw new InvalidOperationException("A person with this National Number already exists.");
                }

                this.PersonID = clsPeopleData.AddNewPerson(FirstName, LastName, NationalNo, DateOfBirth, Gender,
                                                           Address, Phone, Email, ImagePath);
                return (this.PersonID != -1);
            }
            catch (Exception ex)
            {
                // Log exception here (e.g., using Event Viewer or other logging mechanism)
                throw new ApplicationException("An error occurred while adding a new person.", ex);
            }
        }

        private bool _UpdatePerson()
        {
            try
            {
                if (!clsPeopleData.PersonExists(PersonID))
                {
                    throw new InvalidOperationException("Person with the provided ID does not exist.");
                }

                return clsPeopleData.UpdatePerson(PersonID, FirstName, LastName, NationalNo, DateOfBirth, Gender,
                                                   Address, Phone, Email, ImagePath);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while updating the person.", ex);
            }
        }

        public bool Save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                {
                    throw new ArgumentException("First Name and Last Name are required.");
                }

                switch (Mode)
                {
                    case enMode.AddNew:
                        return _AddNewPerson();
                    case enMode.Update:
                        return _UpdatePerson();
                    default:
                        throw new InvalidOperationException("Unknown mode.");
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while saving the person.", ex);
            }
        }

        public static clsPeople FindByID(int personID)
        {
            try
            {
                DataTable dt = clsPeopleData.GetAllPeople(); // Adjusted to a more appropriate method if needed.
                foreach (DataRow row in dt.Rows)
                {
                    if ((int)row["PersonID"] == personID)
                    {
                        return new clsPeople(
                            (int)row["PersonID"],
                            (string)row["FirstName"],
                            (string)row["LastName"],
                            (string)row["NationalNo"],
                            (DateTime)row["DateOfBirth"],
                            (short)row["Gender"],
                            (string)row["Address"],
                            (string)row["Phone"],
                            (string)row["Email"],
                            (string)row["ImagePath"]
                        );
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while finding the person by ID.", ex);
            }
        }

        public static clsPeople FindByNationalNo(string nationalNo)
        {
            try
            {
                DataTable dt = clsPeopleData.GetAllPeople(); // Adjusted to a more appropriate method if needed.
                foreach (DataRow row in dt.Rows)
                {
                    if ((string)row["NationalNo"] == nationalNo)
                    {
                        return new clsPeople(
                            (int)row["PersonID"],
                            (string)row["FirstName"],
                            (string)row["LastName"],
                            (string)row["NationalNo"],
                            (DateTime)row["DateOfBirth"],
                            (short)row["Gender"],
                            (string)row["Address"],
                            (string)row["Phone"],
                            (string)row["Email"],
                            (string)row["ImagePath"]
                        );
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while finding the person by National Number.", ex);
            }
        }

        public static bool Delete(int personID)
        {
            try
            {
                return clsPeopleData.DeletePerson(personID);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while deleting the person.", ex);
            }
        }

        public static bool ExistsByID(int personID)
        {
            try
            {
                return clsPeopleData.PersonExists(personID);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while checking if the person exists by ID.", ex);
            }
        }

        public static bool ExistsByNationalNo(string nationalNo)
        {
            try
            {
                return clsPeopleData.PersonExistsByNationalNo(nationalNo);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while checking if the person exists by National Number.", ex);
            }
        }

        public static DataTable GetAllPeople()
        {
            try
            {
                return clsPeopleData.GetAllPeople();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving all people.", ex);
            }
        }

        public static DataTable GetPagedPeople(int pageNumber, int pageSize, out int totalCount)
        {
            try
            {
                return clsPeopleData.GetPagedPeople(pageNumber, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving paged people.", ex);
            }
        }



        ///
    }
}
