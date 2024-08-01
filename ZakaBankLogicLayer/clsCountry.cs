using System;
using System.Data;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsCountry
    {


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int CurrencyID { get; set; }

        public clsCountry()
        {
            Mode = enMode.AddNew;
        }

        public clsCountry(int countryID, string countryName, string countryCode, int currencyID)
        {
            CountryID = countryID;
            CountryName = countryName;
            CountryCode = countryCode;
            CurrencyID = currencyID;
            Mode = enMode.Update;
        }

        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryData.AddNewCountry(CountryName, CountryCode, CurrencyID);
            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {
            return clsCountryData.UpdateCountry(CountryID, CountryName, CountryCode, CurrencyID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateCountry();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsCountry FindByCountryID(int countryID)
        {
            string countryName = string.Empty;
            string countryCode = string.Empty;
            int currencyID = 0;

            bool isFound = clsCountryData.FindCountryByID(countryID, ref countryName, ref countryCode, ref currencyID);

            if (isFound)
                return new clsCountry(countryID, countryName, countryCode, currencyID);
            else
                return null;
        }

        public static bool Delete(int countryID)
        {
            return clsCountryData.DeleteCountry(countryID);
        }

        public static bool ExistsByID(int countryID)
        {
            return clsCountryData.CountryExists(countryID);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        public static DataTable GetPagedCountries(int pageNumber, int pageSize, out int totalCount)
        {
            return clsCountryData.GetPagedCountries(pageNumber, pageSize, out totalCount);
        }
    }
}
