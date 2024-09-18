using System;
using System.Data;
using System.Threading.Tasks;
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

        private async Task<bool> _AddNewCountryAsync()
        {
            this.CountryID = await clsCountryData.AddNewCountryAsync(CountryName, CountryCode, CurrencyID);
            return (this.CountryID != -1);
        }

        private async Task<bool> _UpdateCountryAsync()
        {
            return await clsCountryData.UpdateCountryAsync(CountryID, CountryName, CountryCode, CurrencyID);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewCountryAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateCountryAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsCountry> FindByCountryID(int countryID)
        {
            var dt = await clsCountryData.FindCountryByIDAsync(countryID);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsCountry(
                                           Convert.ToInt32(row["CountryID"]),
                                           Convert.ToString(row["CountryName"]),
                                           Convert.ToString(row["CountryCode"]),
                                           Convert.ToInt32(row["CurrencyID"])

                                          );
            }
            return null;
        }

        public static async Task<clsCountry> FindByCountryName(string countryName)
        {
            var dt = await clsCountryData.FindCountryByNameAsync(countryName);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsCountry(
                                           Convert.ToInt32(row["CountryID"]),
                                           Convert.ToString(row["CountryName"]),
                                           Convert.ToString(row["CountryCode"]),
                                           Convert.ToInt32(row["CurrencyID"])

                                          );
            }
            return null;
        }

        public static async Task<bool> DeleteAsync(int countryID)
        {
            return await clsCountryData.DeleteCountryAsync(countryID);
        }

        public static async Task<bool> ExistsByIDAsync(int countryID)
        {
            return await clsCountryData.CountryExists(countryID);
        }

        public static async Task<DataTable> GetAllCountriesAsync()
        {
            return await clsCountryData.GetAllCountries();
        }

        public static async Task<(DataTable dataTable, int totalCount)> GetPagedCountries(int pageNumber, int pageSize)
        {
            return await clsCountryData.GetPagedCountries(pageNumber, pageSize);
        }
    }
}
