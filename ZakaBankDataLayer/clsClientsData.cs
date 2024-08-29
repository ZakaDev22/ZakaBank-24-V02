using System;
using System.Data;
using System.Data.SqlClient;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsClientsData
    {

        public static int AddNewClient(int personID, string accountNumber, string pinCode, decimal balance, int addedByUserID, int accountTypeID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Clients_AddNewClient", conn))
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

        public static bool UpdateClient(int clientID, int personID, string accountNumber, string pinCode, decimal balance, int addedByUserID, int accountTypeID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Clients_UpdateClient", conn))
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
                        conn.Open();
                        return (cmd.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static bool DeleteClient(int clientID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Clients_DeleteClient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", clientID);

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
        }

        public static bool ClientExists(int clientID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_ExistsByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientID", clientID);

                        conn.Open();
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

        public static bool FindClientByID(int clientID, ref int personID, ref string accountNumber, ref string pinCode, ref decimal balance, ref int addedByUserID, ref int accountTypeID, ref bool isDeleted)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Clients_FindByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientID", clientID);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.HasRows)
                            {
                                da.Read();
                                personID = Convert.ToInt32(da["PersonID"]);
                                accountNumber = da["AccountNumber"].ToString();
                                pinCode = da["PinCode"].ToString();
                                balance = Convert.ToDecimal(da["Balance"]);
                                addedByUserID = Convert.ToInt32(da["AddedByUserID"]);
                                accountTypeID = Convert.ToInt32(da["AccountTypeID"]);
                                isDeleted = Convert.ToBoolean(da["IsDeleted"]);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return false;
        }

        public static DataTable GetAllClients()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Client_GetAllClients", conn))
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

        public static DataTable GetPagedClients(int pageNumber, int pageSize, out int totalCount)
        {
            DataTable dataTable = new DataTable();
            totalCount = 0;

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
    }
}
