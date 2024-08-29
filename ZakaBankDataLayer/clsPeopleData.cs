using System;
using System.Data;
using System.Data.SqlClient;
using ZakaBankDataLayer.Data_Global;


namespace ZakaBankDataLayer
{

    public static class clsPeopleData
    {

        public static int AddNewPerson(string firstName, string lastName, DateTime dateOfBirth,
                                        short gender, string address, string phone, string email, string imagePath, int countryId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_AddNewPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.Parameters.AddWithValue("@CountryID", countryId);

                    SqlParameter outParameter = new SqlParameter("@PersonID", SqlDbType.Int)
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
                        // Handle exception
                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return -1;
                    }


                }
            }
        }

        public static bool UpdatePerson(int personID, string firstName, string lastName,
                                        DateTime dateOfBirth, short gender, string address, string phone,
                                        string email, string imagePath, int countryId)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_UpdatePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.Parameters.AddWithValue("@CountryID", countryId);

                    try
                    {
                        conn.Open();
                        return (Convert.ToByte(cmd.ExecuteNonQuery()) > 0);
                    }
                    catch (Exception ex)
                    {
                        // Handle exception

                        ExLogClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }


                }
            }

        }

        public static bool DeletePerson(int personID)
        {
            using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_People_DeletePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);


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

        public static bool PersonExists(int personID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {

                    using (SqlCommand comd = new SqlCommand("sp_People_ExistsByID", conn))
                    {
                        comd.CommandType = CommandType.StoredProcedure;

                        comd.Parameters.AddWithValue("@PersonID", personID);

                        object result = comd.ExecuteScalar();

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

        public static bool FindPersonByID(int personID, ref string firstName, ref string lastName,
                                        ref DateTime dateOfBirth, ref short gender, ref string address, ref string phone,
                                        ref string email, ref string imagePath, ref int countryID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_People_FindByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PersonID", personID);

                        conn.Open();
                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            if (da.Read())
                            {
                                firstName = da["FirstName"].ToString();
                                lastName = da["LastName"].ToString();
                                dateOfBirth = da["DateOfBirth"] == DBNull.Value ? DateTime.Now : DateTime.Parse(da["DateOfBirth"].ToString());
                                gender = Convert.ToInt16(da["Gender"]);
                                address = da["Address"].ToString();
                                phone = da["PhoneNumber"].ToString();
                                email = da["Email"].ToString();
                                imagePath = da["ImagePath"].ToString();
                                countryID = Convert.ToInt32(da["CountryID"]);

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

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataLayerSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_People_GetAllPeople", conn))
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

        public static DataTable GetPagedPeople(int pageNumber, int pageSize, out int totalCount)
        {
            DataTable dataTable = new DataTable();
            totalCount = 0;

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

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
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



