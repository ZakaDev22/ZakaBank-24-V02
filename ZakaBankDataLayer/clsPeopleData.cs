using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ZakaBankDataLayer.Data_Global;


namespace ZakaBankDataLayer
{

    public static class clsPeopleData
    {
        private static void AddNullableParameter(SqlCommand cmd, string paramName, object value)
        {
            cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
        }

        public static async Task<int> AddNewPersonAsync(string firstName, string lastName, DateTime? dateOfBirth,
                                                  short? gender, string address, string phone, string email, string imagePath, int? countryId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_AddNewPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Required fields
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    // Nullable fields with helper method
                    AddNullableParameter(cmd, "@DateOfBirth", dateOfBirth);
                    AddNullableParameter(cmd, "@Gender", gender);
                    AddNullableParameter(cmd, "@CountryID", countryId);

                    // Optional string fields
                    AddNullableParameter(cmd, "@Address", address);
                    AddNullableParameter(cmd, "@PhoneNumber", phone);
                    AddNullableParameter(cmd, "@Email", email);
                    AddNullableParameter(cmd, "@ImagePath", imagePath);


                    SqlParameter outParameter = new SqlParameter("@PersonID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParameter);

                    try
                    {
                        await conn.OpenAsync();  // Open the connection asynchronously
                        await cmd.ExecuteNonQueryAsync();  // Execute the command asynchronously
                        int personId = outParameter.Value != DBNull.Value ? (int)outParameter.Value : -1;
                        return personId;
                    }
                    catch (Exception ex)
                    {
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return -1;  // Handle exception and return error code
                    }
                }
            }
        }


        public static async Task<bool> UpdatePersonAsync(int? personID, string firstName, string lastName,
                                                  DateTime? dateOfBirth, short? gender, string address, string phone,
                                                  string email, string imagePath, int? countryId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_UpdatePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Required fields
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    // Nullable fields with helper method
                    AddNullableParameter(cmd, "@DateOfBirth", dateOfBirth);
                    AddNullableParameter(cmd, "@Gender", gender);
                    AddNullableParameter(cmd, "@CountryID", countryId);

                    // Optional string fields
                    AddNullableParameter(cmd, "@Address", address);
                    AddNullableParameter(cmd, "@PhoneNumber", phone);
                    AddNullableParameter(cmd, "@Email", email);
                    AddNullableParameter(cmd, "@ImagePath", imagePath);

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

        public static async Task<bool> DeletePersonAsync(int personID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_DeletePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

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


        public static async Task<bool> PersonExistsAsync(int personID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {

                    using (SqlCommand comd = new SqlCommand("sp_People_ExistsByID", conn))
                    {
                        comd.CommandType = CommandType.StoredProcedure;

                        comd.Parameters.AddWithValue("@PersonID", personID);

                        await conn.OpenAsync();
                        object result = await comd.ExecuteScalarAsync();

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

        public static async Task<DataTable> FindPersonByIDAsync(int personID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_People_FindByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PersonID", personID);

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())  // Asynchronously execute reader
                        {
                            dt.Load(da); // load The DataTable
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


        public static async Task<DataTable> GetAllPeopleAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_People_GetAllPeople", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await conn.OpenAsync();
                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(da); // load The DataTable
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


        public static async Task<(DataTable, int)> GetPeopleByPageAsync(int pageNumber, int pageSize)
        {
            DataTable dataTable = new DataTable();
            int totalCount = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_People_GetAllPeopleByPages", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PageNumber", pageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        SqlParameter totalParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalParam);

                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader); // load The DataTable
                        }

                        totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

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



