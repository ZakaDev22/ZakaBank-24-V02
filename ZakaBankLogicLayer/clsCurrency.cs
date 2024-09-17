using System;
using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsCurrency
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public decimal ExchangeRate { get; set; }

        public clsCurrency()
        {
            Mode = enMode.AddNew;
        }

        public clsCurrency(int currencyID, string currencyCode, string currencyName, decimal exchangeRate)
        {
            CurrencyID = currencyID;
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
            ExchangeRate = exchangeRate;
            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewCurrencyAsync()
        {
            this.CurrencyID = await clsCurrencyData.AddNewCurrencyAsync(CurrencyCode, CurrencyName, ExchangeRate);
            return (this.CurrencyID != -1);
        }

        private async Task<bool> _UpdateCurrencyAsync()
        {
            return await clsCurrencyData.UpdateCurrencyAsync(CurrencyID, CurrencyCode, CurrencyName, ExchangeRate);
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewCurrencyAsync())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return await _UpdateCurrencyAsync();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static async Task<clsCurrency> FindByCurrencyCode(string currencyCode)
        {
            var dt = await clsCurrencyData.FindCurrencyByCodeAsync(currencyCode);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsCurrency(
                                           Convert.ToInt32(row["CurrencyID"]),
                                           Convert.ToString(row["CurrencyCode"]),
                                           Convert.ToString(row["CurrencyName"]),
                                           Convert.ToDecimal(row["ExchangeRate"])


                                          );
            }
            return null;
        }

        public static async Task<clsCurrency> FindByCurrencyName(string currencyName)
        {
            var dt = await clsCurrencyData.FindCurrencyByNameAsync(currencyName);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new clsCurrency(
                                           Convert.ToInt32(row["CurrencyID"]),
                                           Convert.ToString(row["CurrencyCode"]),
                                           Convert.ToString(row["CurrencyName"]),
                                           Convert.ToDecimal(row["ExchangeRate"])


                                          );
            }
            return null;
        }

        public static async Task<bool> DeleteAsync(int currencyID)
        {
            return await clsCurrencyData.DeleteCurrencyAsync(currencyID);
        }

        public static async Task<bool> ExistsByIDAsync(int currencyID)
        {
            return await clsCurrencyData.CurrencyExistsAsync(currencyID);
        }

        public static async Task<DataTable> GetAllCurrenciesAsync()
        {
            return await clsCurrencyData.GetAllCurrenciesAsync();
        }

        public static async Task<(DataTable dataTable, int totalCount)> GetPagedCurrenciesAsync(int pageNumber, int pageSize)
        {
            return await clsCurrencyData.GetPagedCurrencies(pageNumber, pageSize);
        }
    }
}

