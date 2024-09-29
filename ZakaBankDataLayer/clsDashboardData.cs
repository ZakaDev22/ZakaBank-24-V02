using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsDashboardData
    {
        public static async Task<DataTable> GetTotalTransactionsAsync()
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_GetTotalTransactions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
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

        public static async Task<DataTable> GetTotalTransfersAsync()
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetTotalTransfers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
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

        public static async Task<DataTable> GetTransactionTypesAsync()
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_GetTransactionTypes", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
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

        public static async Task<DataTable> GetClientBalanceOverviewAsync()
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_GetClientBalanceOverview", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
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

    }
}
