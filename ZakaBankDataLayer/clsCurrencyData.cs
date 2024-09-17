using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsCurrencyData
    {
        ////////////////


        public static async Task<int> AddNewCurrencyAsync(string currencyName, string currencyCode, decimal exchangeRate)
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
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
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

        public static async Task<bool> UpdateCurrencyAsync(int currencyId, string currencyName, string currencyCode, decimal exchangeRate)
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
                        await conn.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static async Task<DataTable> FindCurrencyByCodeAsync(string currencyCode)
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_FindByCode", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
                        {
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

        public static async Task<DataTable> FindCurrencyByNameAsync(string currencyName)
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_FindCurrencyByName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyName", currencyName);

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
                        {
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


        public static async Task<bool> DeleteCurrencyAsync(int currencyId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Currencies_DeleteCurrency", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrencyID", currencyId);

                    try
                    {
                        await conn.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static async Task<bool> CurrencyExistsAsync(int currencyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_ExistsByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrencyID", currencyId);

                        await conn.OpenAsync();
                        object result = await cmd.ExecuteScalarAsync();

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

        public static async Task<DataTable> GetAllCurrenciesAsync()
        {
            var dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Currencies_GetAllCurrencies", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
                        {
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

        public static async Task<(DataTable, int)> GetPagedCurrencies(int pageNumber, int pageSize)
        {
            var dataTable = new DataTable();
            int totalCount = 0;

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

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
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

            return (dataTable, totalCount);
        }
    }
}
