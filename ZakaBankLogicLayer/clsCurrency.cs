using System;
using System.Data;
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

        private bool _AddNewCurrency()
        {
            this.CurrencyID = clsCurrencyData.AddNewCurrency(CurrencyCode, CurrencyName, ExchangeRate);
            return (this.CurrencyID != -1);
        }

        private bool _UpdateCurrency()
        {
            return clsCurrencyData.UpdateCurrency(CurrencyID, CurrencyCode, CurrencyName, ExchangeRate);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCurrency())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;

                case enMode.Update:
                    return _UpdateCurrency();

                default:
                    throw new InvalidOperationException("Unknown mode.");
            }

            return false;
        }

        public static clsCurrency FindByCurrencyCode(string currencyCode)
        {
            int currencyID = -1;
            string currencyName = string.Empty;
            decimal exchangeRate = 0;

            bool isFound = clsCurrencyData.FindCurrencyByCode(currencyCode, ref currencyID, ref currencyName, ref exchangeRate);

            if (isFound)
                return new clsCurrency(currencyID, currencyCode, currencyName, exchangeRate);
            else
                return null;
        }

        public static clsCurrency FindByCurrencyName(string currencyName)
        {
            int currencyID = -1;
            string currencyCode = string.Empty;
            decimal exchangeRate = 0;

            bool isFound = clsCurrencyData.FindCurrencyByName(currencyName, ref currencyID, ref currencyCode, ref exchangeRate);

            if (isFound)
                return new clsCurrency(currencyID, currencyCode, currencyName, exchangeRate);
            else
                return null;
        }

        public static bool Delete(int currencyID)
        {
            return clsCurrencyData.DeleteCurrency(currencyID);
        }

        public static bool ExistsByID(int currencyID)
        {
            return clsCurrencyData.CurrencyExists(currencyID);
        }

        public static DataTable GetAllCurrencies()
        {
            return clsCurrencyData.GetAllCurrencies();
        }

        public static DataTable GetPagedCurrencies(int pageNumber, int pageSize, out int totalCount)
        {
            return clsCurrencyData.GetPagedCurrencies(pageNumber, pageSize, out totalCount);
        }
    }
}

