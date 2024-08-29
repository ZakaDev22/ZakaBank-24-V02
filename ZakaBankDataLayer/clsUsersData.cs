using System;
using System.Data;
using System.Data.SqlClient;
using ZakaBankDataLayer.Data_Global;

namespace ZakaBankDataLayer
{
    public class clsUsersData
    {

        public static int AddNewUser(int personID, string userName, string passwordHash, DateTime createdDate, DateTime updatedDate, int permissions, int addedByUserID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_AddNewUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@CreatedDate", createdDate);
                    cmd.Parameters.AddWithValue("@UpdatedDate", updatedDate);
                    cmd.Parameters.AddWithValue("@Permissions", permissions);
                    cmd.Parameters.AddWithValue("@AddedByUserID", addedByUserID);

                    SqlParameter outParameter = new SqlParameter("@ID", SqlDbType.Int)
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

        public static bool UpdateUser(int id, int personID, string userName, string passwordHash, DateTime createdDate, DateTime updatedDate, int permissions, int addedByUserID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@CreatedDate", createdDate);
                    cmd.Parameters.AddWithValue("@UpdatedDate", updatedDate);
                    cmd.Parameters.AddWithValue("@Permissions", permissions);
                    cmd.Parameters.AddWithValue("@AddedByUserID", addedByUserID);

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

        public static bool DeleteUser(int id)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Users_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

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

        public static bool UserExists(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Users_ExistsByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);

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

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Users_GetAllUsers", conn))
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

        public static DataTable GetPagedUsers(int pageNumber, int pageSize, out int totalCount)
        {
            DataTable dataTable = new DataTable();
            totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Users_GetAllUsersByPages", conn))
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

        public static bool FindUserByID(int id, ref int personID, ref string userName, ref string passwordHash, ref DateTime createdDate, ref DateTime updatedDate, ref int permissions, ref int addedByUserID, ref bool isactive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Users_FindByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.HasRows)
                            {
                                da.Read();
                                personID = Convert.ToInt32(da["PersonID"]);
                                userName = da["UserName"].ToString();
                                passwordHash = da["PasswordHash"].ToString();
                                createdDate = Convert.ToDateTime(da["CreatedDate"]);
                                updatedDate = Convert.ToDateTime(da["UpdatedDate"]);
                                permissions = Convert.ToInt32(da["Permissions"]);
                                addedByUserID = Convert.ToInt32(da["AddedByUserID"]);
                                isactive = Convert.ToBoolean(da["IsActive"]);

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


        public static bool FindByUserNameAndPassword(string UserName, string Password, ref int userID, ref int personID, ref DateTime createdDate, ref DateTime updatedDate, ref int permissions, ref int addedByUserID, ref bool isActive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Users_FindByUserNameAndPassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", UserName);
                        cmd.Parameters.AddWithValue("@Password", Password);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.Read())
                            {
                                userID = (int)da["UserID"];
                                personID = Convert.ToInt32(da["PersonID"]);
                                createdDate = Convert.ToDateTime(da["CreatedDate"]);
                                updatedDate = Convert.ToDateTime(da["UpdatedDate"]);
                                permissions = Convert.ToInt32(da["Permissions"]);
                                addedByUserID = da["AddedByUserID"] == DBNull.Value ? -1 : Convert.ToInt32(da["AddedByUserID"]);
                                isActive = Convert.ToBoolean(da["IsActive"]);

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
    }
}
