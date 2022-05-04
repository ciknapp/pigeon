using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace CS414_Team2.Data
{
    public static class DataProvider
    {
        private const int PASSWORD_MAX_SIZE = 128;
        private const int USERNAME_MAX_SIZE = 25;
        private const int EMAIL_MAX_SIZE = 254;
        private const int GROUPNAME_MAX_SIZE = 50;

        public static bool ValidToken(string username, string token)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.security.valid_token(p_username => :p_usernameFromSite,
                                                                       p_token    => :p_tokenFromSite);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "p_usernameFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_tokenFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: 40,
                                           val: token,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    command.Parameters["c_Data"].Size = 1;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString() == "Y";

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static string LoginUser(string username, string password = "", string token = "", bool EmailJustConfirmed = false)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.security.login_user(p_username => :p_usernameFromSite,
                                                                      p_password => :p_passwordFromSite,
                                                                      p_token    => :p_tokenFromSite,
                                                                      p_EmailJustConfirmed => :EmailJustConfirmed);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "p_usernameFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_passwordFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: PASSWORD_MAX_SIZE,
                                           val: password,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_tokenFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: 40,
                                           val: token,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "EmailJustConfirmed",
                                           dbType: OracleDbType.Varchar2,
                                           size: 2,
                                           val: EmailJustConfirmed ? "Y" : "N",
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    command.Parameters["c_Data"].Size = 40;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static void LogoutUser(string username)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.security.logout_user(p_username => :p_usernameFromSite);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "p_usernameFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static int CreatePrivateNest(int userID1, int userID2)
        {
            int response;

            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.CreatePrivateNest(p_UserA => :userID1, p_UserB => :userID2);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID1",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID1,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID2",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID2,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Int32,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 10;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = int.Parse(command.Parameters["c_Data"].Value.ToString());

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static void AddUser(string username, string password, string email)
        {
            if (email.Trim().EndsWith("pcci.edu"))
            {
                // Oracle Connection code from offical Oracle documentation:
                // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

                using (OracleCommand command = new OracleCommand())
                {
                    using (OracleConnection connection = new OracleConnection())
                    {
                        // Using connection string attribute from Web.config
                        connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        connection.Open();

                        // Execute the command
                        command.BindByName = true;
                        command.CommandType = System.Data.CommandType.Text;

                        command.CommandText = @"
                        BEGIN
                           cs414_team2.security.add_user (p_username => :p_usernameFromSite,
                                                          p_password => :p_passwordFromSite,
                                                          p_email    => :p_emailFromSite);
                        END;";

                        // Input parameters
                        command.Parameters.Add(name: "p_usernameFromSite",
                                               dbType: OracleDbType.Varchar2,
                                               size: USERNAME_MAX_SIZE,
                                               val: username,
                                               dir: System.Data.ParameterDirection.Input);

                        command.Parameters.Add(name: "p_passwordFromSite",
                                               dbType: OracleDbType.Varchar2,
                                               size: PASSWORD_MAX_SIZE,
                                               val: password,
                                               dir: System.Data.ParameterDirection.Input);

                        command.Parameters.Add(name: "p_emailFromSite",
                                               dbType: OracleDbType.Varchar2,
                                               size: EMAIL_MAX_SIZE,
                                               val: email,
                                               dir: System.Data.ParameterDirection.Input);

                        command.Connection = connection;

                        command.ExecuteNonQuery();

                        // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }

        public static DataSet GetMessageIds(int userID, int nestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.messaging.get_message_ids(p_group_id => :nestID, p_user_id => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static DataSet GetAllMessages(string userName, int userID, int nestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            DataProvider.NotifyUser(userID);

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.messaging.get_all_group_messages(p_username => :userName, p_group_id => :nestID, p_user_id => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        /// <summary>
        /// Returns Base64 string file.
        /// </summary>
        /// <param name="id">File ID to retrieve.</param>
        /// <returns></returns>
        public static string GetFile(int userId, int nestId, int fileId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.messaging.GetFile (p_UserId  => :userId,
                                                                     p_GroupId => :groupId,
                                                                     p_FileId  => :fileId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "groupId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "fileId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: fileId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        response = reader.GetOracleClob(0).Value;
                    }

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        /// <summary>
        /// Returns Base64 string file.
        /// </summary>
        /// <param name="id">File ID to retrieve.</param>
        /// <returns></returns>
        public static DataTable GetFileMetadata(int userId, int nestId, int fileId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataTable response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.messaging.GetFileMetadata (p_UserId  => :userId,
                                                                             p_GroupId => :groupId,
                                                                             p_FileId  => :fileId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "groupId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "fileId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: fileId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    try
                    {
                        adapter.Fill(data);
                        response = data.Tables[0];
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            case 20400:
                                // Don't care, didn't ask.
                                response = null;
                                break;
                            default:
                                throw;
                        }
                    }

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static DataTable GetNewMessages(string userName, int userID, int nestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataTable response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.messaging.get_new_group_messages(p_username => :userName, p_group_id => :nestID, p_user_id => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    try
                    {
                        adapter.Fill(data);
                        response = data.Tables[0];
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            case 20400:
                                // Don't care, didn't ask.
                                response = null;
                                break;
                            default:
                                throw;
                        }
                    }

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static void NotifyUser(int userID)
        {
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Update notifications
                    command.CommandText = @"
                        BEGIN
                          cs414_team2.nests.NotifyUser(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void LogoutInactiveUsers()
        {
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Update notifications
                    command.CommandText = @"
                        BEGIN
                          cs414_team2.security.LogoutInactiveUsers;
                        END;";

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void LogUserActivity(int userID)
        {
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Update notifications
                    command.CommandText = @"
                        BEGIN
                          cs414_team2.security.LogUserActivity(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static DataSet GetNotificationNestsForUser(int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Get nests
                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetNestsWithNotifications(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static DataSet GetMembersInNest(int userID, int nestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetNestMembers(p_UserID => :userID, p_NestID => :nestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static string GetNestName(int userID, int nestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetNestName(p_UserID => :userID, p_NestID => :nestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 100;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static int GetUserID(string userName)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            int response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.security.get_user_id(p_username => :userName);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: 100,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Int32,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = int.Parse(command.Parameters["c_Data"].Value.ToString());

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static void SendMessage(int userID, int nestID, string messageText, string fileB64 = "", string fileName = "", string extension = "")
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          cs414_team2.messaging.send_message(p_user_id => :userID,
                                                             p_group_id => :nestID,
                                                             p_text => :messageText,
                                                             p_file => :fileB64,
                                                             p_FileName => :fileName,
                                                             p_Extension => :extension);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "messageText",
                                           dbType: OracleDbType.Varchar2,
                                           size: 255,
                                           val: messageText,
                                           dir: System.Data.ParameterDirection.Input);

                    if (fileB64 == string.Empty)
                    {
                        command.Parameters.Add(name: "fileB64",
                                           dbType: OracleDbType.Clob,
                                           val: DBNull.Value,
                                           dir: System.Data.ParameterDirection.Input);
                    }
                    else
                    {
                        command.Parameters.Add(name: "fileB64",
                                              dbType: OracleDbType.Clob,
                                              val: fileB64,
                                              dir: System.Data.ParameterDirection.Input);
                    }

                    command.Parameters.Add(name: "fileName",
                                           dbType: OracleDbType.Varchar2,
                                           size: 255,
                                           val: fileName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "extension",
                                           dbType: OracleDbType.Varchar2,
                                           size: 255,
                                           val: extension,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            case 20403:
                                break;
                            default:
                                throw oEx;
                        }
                    }
                    finally
                    {
                        // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
        public static DataTable SearchUser(string userName)
        {
            DataTable search_results;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config 
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command 
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @" 
                    BEGIN 
                        :c_Data := cs414_team2.searching.search_username(p_username => :p_searchFromSite); 
                    END;";

                    // Input parameters 
                    command.Parameters.Add(name: "p_searchFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: 10,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters 
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataTable data = new DataTable();

                    adapter.Fill(data);

                    search_results = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return search_results;
        }

        public static string ReturnPasscode(string email, string username)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.security.return_passcode(p_email => :email, p_username => :p_username_from_site);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "email",
                                           dbType: OracleDbType.Varchar2,
                                           size: EMAIL_MAX_SIZE,
                                           val: email,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_username_from_site",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    command.Parameters["c_Data"].Size = 8;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static void UpdatePasscode(string email, string username)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.security.update_passcode(p_email    => :p_email_from_site,
                                                                p_username => :p_username_from_site);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "p_email_from_site",
                                           dbType: OracleDbType.Varchar2,
                                           size: EMAIL_MAX_SIZE,
                                           val: email,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_username_from_site",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static string GetMessage(int user_id, int text_id, int group_id)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.messaging.message_retrieval(p_user_id => :user_id, p_text_id => :text_id, p_group_id => :group_id);
                        END;";

                    // Input parameters

                    command.Parameters.Add(name: "user_id",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: user_id,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "group_id",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: group_id,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "text_id",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: text_id,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 250;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static void ChangePassword(string username, string newpassword, string password = "", string passcode = "")
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.security.change_password(p_username     => :p_usernameFromSite,
                                                                p_new_password => :p_newPasswordFromSite,
                                                                p_old_password => :p_passwordFromSite,
                                                                p_passcode     => :p_passcodeFromSite);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "p_usernameFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: username,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_newPasswordFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: PASSWORD_MAX_SIZE,
                                           val: newpassword,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_passwordFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: PASSWORD_MAX_SIZE,
                                           val: password,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "p_passcodeFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: 8,
                                           val: passcode,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static DataSet GetStatistics(string statisticType, DateTime startDate, DateTime endDate, int nestID, int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.statistics.gather_statistics(p_statistic_type => :statisticType, 
                                                                              p_start_date     => :startDate, 
                                                                              p_end_date       => :endDate, 
                                                                              p_group_id       => :nestID, 
                                                                              p_user_id        => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "statisticType",
                                           dbType: OracleDbType.Varchar2,
                                           size: 50,
                                           val: statisticType,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "startDate",
                                           dbType: OracleDbType.Date,
                                           size: 50,
                                           val: startDate,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "endDate",
                                           dbType: OracleDbType.Date,
                                           size: 50,
                                           val: endDate,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    try
                    {
                        adapter.Fill(data);
                        response = data;
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            // No statistic type
                            case 20700:
                                response = null;
                                break;
                            // Nest does not exist
                            case 20600:
                                response = null;
                                break;
                            // User does not exist
                            case 20002:
                                response = null;
                                break;
                            default:
                                throw;
                        }
                    }

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static DataSet GetAllFlaggedMessages(string userName, int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.admin.GetFlaggedMessages(p_Username => :userName, p_UserId => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static void IsAdmin(string userName, int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.IsAdmin(p_UserName => :userName, p_UserId => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: 254,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 25,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static DataSet GetAllUsers(string userName, int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.admin.GetAllUsers(p_Username => :userName, p_UserId => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static bool HasNewMessages(int NestID, int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.HasNewMessages(p_NestID => :NestID, p_UserID => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    command.Parameters["c_Data"].Size = 8;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString() == "Y";

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static int GetUserRole(int UserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            int response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.GetUserRole(p_UserID => :UserID, p_NestID => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Int32,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = int.Parse(command.Parameters["c_Data"].Value.ToString());

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static DataSet GetUserInvites(int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetUserInvites(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);
                    // Output parameters 
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }
        public static void AcceptGroupInvitation(int userID, int nestID, string acceptReject)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.AcceptGroupInvitation(p_UserID => :userID, p_NestID => :nestID, p_Decision => :acceptReject);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "acceptReject",
                                           dbType: OracleDbType.Varchar2,
                                           size: 1,
                                           val: acceptReject,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void RemoveFromNest(int RemovedUserID, int NestID, int RequestingUser)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.RemoveFromNest(p_RemovedUserID => :RemovedUserID, p_NestID => :NestID, p_RequestingUser => :RequestingUser);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "RemovedUserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: RemovedUserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "RequestingUser",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: RequestingUser,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static int CreateGroupNest(string groupName, int userID)
        {
            int response;

            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.CreateNest(p_Name => :groupName, p_User => :userID, o_GroupId => :c_Data);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "groupName",
                                           dbType: OracleDbType.Varchar2,
                                           size: GROUPNAME_MAX_SIZE,
                                           val: groupName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Int32,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 10;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = int.Parse(command.Parameters["c_Data"].Value.ToString());

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static void AddToNest(int AddingUserID, int nestID, int RequestingUserID, int localrole = 3)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.AddToNest(p_UserID => :AddingUserID, p_NestID => :nestID, p_LocalRole => :localrole, p_RequestingUser => :RequestingUserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "AddingUserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: AddingUserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "nestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: nestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "localrole",
                                           dbType: OracleDbType.Int32,
                                           size: 2,
                                           val: localrole,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "RequestingUserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: RequestingUserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static DataSet GetGroupNestsForUser(int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Get nests
                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetGroupNestsForUser(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static DataSet GetPrivateNestsForUser(int userID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    // Get nests
                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.nests.GetPrivateNestsForUser(p_UserID => :userID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static DataTable InviteUsername(string userName, int NestID)
        {
            DataTable search_results;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config 
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command 
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @" 
                    BEGIN 
                        :c_Data := cs414_team2.searching.invite_username(p_username => :p_searchFromSite, p_NestId => :NestID); 
                    END;";

                    // Input parameters 
                    command.Parameters.Add(name: "p_searchFromSite",
                                           dbType: OracleDbType.Varchar2,
                                           size: 50,
                                           val: userName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters 
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataTable data = new DataTable();

                    adapter.Fill(data);

                    search_results = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return search_results;
        }
        public static void ConfirmEmailAddress(string email, string passcode)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.security.ConfirmEmailAddress(p_email => :email,
                                                                    p_passcode => :passcode);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "email",
                                           dbType: OracleDbType.Varchar2,
                                           size: EMAIL_MAX_SIZE,
                                           val: email,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "passcode",
                                           dbType: OracleDbType.Varchar2,
                                           size: 8,
                                           val: passcode,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void DeleteMessage(int UserId, int TextId, int NestId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.messaging.DeleteMessage(p_UserId => :UserId, p_TextId => :TextId, p_NestId => :NestId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "TextId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: TextId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void FlagMessage(int UserId, int TextId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.FlagMessage(p_UserId => :UserId, p_TextId => :TextId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "TextId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: TextId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void UnFlagMessage(string UserName, int UserId, int TextId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.UnFlagMessage(p_UserName => :UserName, p_UserId => :UserId, p_TextId => :TextId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserName",
                                           dbType: OracleDbType.Varchar2,
                                           size: 10,
                                           val: UserName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "TextId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: TextId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void LeaveNest(int NestID, int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.LeaveNest(p_NestID => :NestID, p_UserID => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static int GetMemberCount(int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            int response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.GetMemberCounts(p_NestID => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Int32,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 10;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = int.Parse(command.Parameters["c_Data"].Value.ToString());

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static void DeleteGroupNest(int NestID, int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.DeleteGroupNest(p_NestID => :NestID, p_UserID => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static bool IsNestPrivate(int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.IsNestPrivate(p_NestID => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static string GetUserIcon(int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.user_settings.GetUserIcon(p_UserId => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 1000;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static DataTable GetNotifications(int userId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataTable response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.notification_api.GetNotifications(p_UserId  => :userId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "userId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: userId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);
                    response = data.Tables[0];

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static bool UserHasNewNotifications(int UserId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.notification_api.UserHasNewNotifications(p_UserId => :UserId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }
        public static void UnBanUser(int BannedUserId, int AdminUserId, string AdminName)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.UnBanUser(p_BannedUserId => :BannedUserId, p_AdminUserId => :AdminUserId, p_AdminName => :AdminName);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "BannedUserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: BannedUserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminUserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: AdminUserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: AdminName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void BanUser(int BannedUserId, int AdminUserId, string AdminName)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.BanUser(p_BannedUserId => :BannedUserId, p_AdminUserId => :AdminUserId, p_AdminName => :AdminName);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "BannedUserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: BannedUserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminUserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: AdminUserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: AdminName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void GrantLocalAdmin(int NestCreator, int NestID, int LocalAdmin)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.GrantLocalAdmin(p_NestCreator => :NestCreator, p_NestID => :NestID, p_LocalAdmin => :LocalAdmin);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestCreator",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestCreator,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "LocalAdmin",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: LocalAdmin,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static void RevokeLocalAdmin(int NestCreator, int NestID, int LocalAdmin)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.RevokeLocalAdmin(p_NestCreator => :NestCreator, p_NestID => :NestID, p_LocalAdmin => :LocalAdmin);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestCreator",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestCreator,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "LocalAdmin",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: LocalAdmin,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static DataTable GetAllUserIcons()
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataTable response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.user_settings.GetAllUserIcons;
                        END;";

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);
                    response = data.Tables[0];

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }

        public static void ChangeUserIcon(int UserID, int IconID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.user_settings.ChangeUserIcon(p_UserId => :UserID, p_IconId => :IconID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "IconID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: IconID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static void ToggleNestMute(int UserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.ToggleNestMute(p_UserId => :UserID, p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static bool UserHasNestMuted(int UserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.UserHasNestMuted(p_UserId => :UserId, p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static void ToggleNestLock(int UserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.nests.ToggleNestLock(p_UserId => :UserID, p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static bool NestIsLocked(int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.NestIsLocked(p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static DataTable SearchForNest(string NestName, int? NestId = null)
        {
            DataTable search_results;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config 
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command 
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @" 
                    BEGIN 
                        :c_Data := cs414_team2.searching.SearchForNest(p_NestName => :NestName, p_NestId => :NestId); 
                    END;";

                    // Input parameters 
                    command.Parameters.Add(name: "NestName",
                                           dbType: OracleDbType.Varchar2,
                                           size: 10,
                                           val: NestName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters 
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataTable data = new DataTable();

                    adapter.Fill(data);

                    search_results = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return search_results;
        }

        public static DataSet AdminGetSelectedNestMessages(string UserName, int NestId, int UserId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.admin.GetSelectedNestMessages(p_UserName => :UserName, p_GroupId => :NestId, p_UserId => :UserId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: UserName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return response;
        }
        public static DataSet AdminGetSelectedNestMembers(int UserId, int GroupId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.admin.GetSelectedNestMembers(p_UserId => :UserId, p_GroupId => :GroupId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "GroupId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: GroupId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }
        public static void AdminConfirmUserEmail(string UserEmailToConfirm, int AdminUserId, string AdminName)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           cs414_team2.admin.ConfirmUserEmail(p_UserEmail => :UserEmailToConfirm, p_AdminUserId => :AdminUserId, p_AdminName => :AdminName);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserEmailToConfirm",
                                           dbType: OracleDbType.Varchar2,
                                           size: EMAIL_MAX_SIZE,
                                           val: UserEmailToConfirm,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminUserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: AdminUserId,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "AdminName",
                                           dbType: OracleDbType.Varchar2,
                                           size: USERNAME_MAX_SIZE,
                                           val: AdminName,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Connection = connection;

                    command.ExecuteNonQuery();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        public static string IsUserInvited(int InvitedUserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.IsUserInvited(p_InvitedUserID => :InvitedUserID, p_NestID => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "InvitedUserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: InvitedUserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static string GetUserSystemRole(int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            string response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.user_settings.GetUserSystemRole(p_UserId => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 10;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = command.Parameters["c_Data"].Value.ToString();

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static bool UserIsKicked(int UserID, int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.UserIsKicked(p_UserId => :UserID, p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static bool NestIsVisible(int NestID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.nests.NestIsVisible(p_NestId => :NestID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "NestID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: NestID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static bool UserIsBanned(int UserID)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            bool response;
            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                           :c_Data := cs414_team2.user_settings.UserIsBanned(p_UserId => :UserID);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserID",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserID,
                                           dir: System.Data.ParameterDirection.Input);

                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.Varchar2,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Parameters["c_Data"].Size = 2;

                    command.Connection = connection;

                    command.ExecuteScalar();

                    response = (command.Parameters["c_Data"].Value.ToString() == "Y");

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }

        public static DataSet AdminGetSelectedUserMessages(int UserId)
        {
            // Oracle Connection code from offical Oracle documentation:
            // https://docs.oracle.com/cd/E11882_01/win.112/e23174/featConnecting.htm#ODPNT163
            DataSet response;

            using (OracleCommand command = new OracleCommand())
            {
                using (OracleConnection connection = new OracleConnection())
                {
                    // Using connection string attribute from Web.config
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection.Open();

                    // Execute the command
                    command.BindByName = true;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = @"
                        BEGIN
                          :c_Data := cs414_team2.admin.GetSelectedUserMessages(p_UserId => :UserId);
                        END;";

                    // Input parameters
                    command.Parameters.Add(name: "UserId",
                                           dbType: OracleDbType.Int32,
                                           size: 10,
                                           val: UserId,
                                           dir: System.Data.ParameterDirection.Input);

                    // Output parameters
                    command.Parameters.Add(name: "c_Data",
                                           dbType: OracleDbType.RefCursor,
                                           direction: System.Data.ParameterDirection.Output);

                    command.Connection = connection;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataSet data = new DataSet();

                    adapter.Fill(data);

                    response = data;

                    // Close and Dispose OracleConnection object (quicker than C# garbage collection)
                    adapter.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return response;
        }
    }
}