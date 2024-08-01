using System;
using System.Data;
using System.Data.SqlClient;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsCurrencyData
    {
        ////////////////


        public static int AddNewCurrency(string currencyName, string currencyCode, decimal exchangeRate)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Currencies_AddNewCurrency", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrencyName", currencyName);
                    cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                    cmd.Parameters.AddWithValue("@ExchangeRate", exchangeRate);

                    SqlParameter outParameter = new SqlParameter("@CurrencyID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParameter);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return (int)outParameter.Value;
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return -1;
                    }
                }
            }
        }

        public static bool UpdateCurrency(int currencyId, string currencyName, string currencyCode, decimal exchangeRate)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Currencies_UpdateCurrency", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrencyID", currencyId);
                    cmd.Parameters.AddWithValue("@CurrencyName", currencyName);
                    cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                    cmd.Parameters.AddWithValue("@ExchangeRate", exchangeRate);

                    try
                    {
                        conn.Open();
                        return (Convert.ToByte(cmd.ExecuteNonQuery()) > 0);
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static bool FindCurrencyByCode(string currencyCode, ref int CurrencyID, ref string currencyName, ref decimal exchangeRate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_FindByCode", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.HasRows)
                            {
                                CurrencyID = Convert.ToInt32(da["CurrencyID"]);
                                currencyName = da["CurrencyName"].ToString();
                                exchangeRate = Convert.ToDecimal(da["ExchangeRate"]);

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
            return false;
        }

        public static bool FindCurrencyByName(string currencyName, ref int CurrencyID, ref string currencyCode, ref decimal exchangeRate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_FindCurrencyByName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyName", currencyName);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.HasRows)
                            {
                                CurrencyID = Convert.ToInt32(da["CurrencyID"]);
                                currencyCode = da["CurrencyCode"].ToString();
                                exchangeRate = Convert.ToDecimal(da["ExchangeRate"]);

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
            return false;
        }


        public static bool DeleteCurrency(int currencyId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Currencies_DeleteCurrency", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrencyID", currencyId);

                    try
                    {
                        conn.Open();
                        return Convert.ToBoolean(cmd.ExecuteNonQuery());
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static bool CurrencyExists(int currencyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_ExistsByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyID", currencyId);

                        object result = cmd.ExecuteScalar();

                        return Convert.ToBoolean(result);
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
        }

        public static DataTable GetAllCurrencies()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_GetAllCurrencies", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.HasRows)
                                dt.Load(da);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }

            return dt;
        }

        public static DataTable GetPagedCurrencies(int pageNumber, int pageSize, out int totalCount)
        {
            DataTable dataTable = new DataTable();
            totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_GetAllCurrenciesByPages", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        SqlParameter totalParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(totalParam);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dataTable.Load(reader);
                        }

                        totalCount = (int)totalParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }

            return dataTable;
        }


        //////////////
    }
}
