using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsClientsData
    {

        public static async Task<int> AddNewClientAsync(int personID, string accountNumber, string pinCode, decimal balance, int addedByUserID, int accountTypeID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Client_AddNewClient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@PinCode", pinCode);
                    cmd.Parameters.AddWithValue("@Balance", balance);
                    cmd.Parameters.AddWithValue("@AddedByUserID", addedByUserID);
                    cmd.Parameters.AddWithValue("@AccountTypeID", accountTypeID);

                    SqlParameter outParameter = new SqlParameter("@ClientID", SqlDbType.Int)
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

        public static async Task<bool> UpdateClientAsync(int clientID, int personID, string accountNumber, string pinCode, decimal balance, int addedByUserID, int accountTypeID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Client_UpdateClient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", clientID);
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@PinCode", pinCode);
                    cmd.Parameters.AddWithValue("@Balance", balance);
                    cmd.Parameters.AddWithValue("@AddedByUserID", addedByUserID);
                    cmd.Parameters.AddWithValue("@AccountTypeID", accountTypeID);

                    try
                    {
                        await conn.OpenAsync();
                        return (await cmd.ExecuteNonQueryAsync() > 0);
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static async Task<bool> DeleteClientAsync(int clientID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Client_DeleteClient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", clientID);

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

        public static async Task<bool> InDeleteClientAsync(int clientID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Client_InDeleteClient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", clientID);

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

        public static async Task<bool> ClientExistsAsync(int clientID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_ExistsByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientID", clientID);

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

        public static async Task<bool> ExistsByPersonIDAsync(int personID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_ExistsByPersonID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PersonID", personID);

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

        public static async Task<DataTable> FindClientByIDAsync(int clientID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_FindByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientID", clientID);

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

        public static async Task<DataTable> GetAllClientsAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetAllClients", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
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

        public static async Task<DataTable> GetAllDeletedClientsAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetAllDeletedClients", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
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

        public static async Task<(DataTable Data, int TotalCount)> GetPagedClientsAsync(int pageNumber, int pageSize)
        {
            DataTable dataTable = new DataTable();
            int totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetAllClientsByPages", conn))
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

            return (dataTable, totalCount);
        }

        public static async Task<(DataTable Data, int TotalCount)> GetPagedDeletedClientsAsync(int pageNumber, int pageSize)
        {
            DataTable dataTable = new DataTable();
            int totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetAllDeletedClientsByPages", conn))
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

            return (dataTable, totalCount);
        }
    }
}
