using GitHubConnector.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConnector
{
    public class DatabaseConnector
    {
        public SqlConnection SqlConnection { get; set; }

        public DatabaseConnector()
        {
            
        }
        
        public async Task SaveCommitsEntriesToDatabase(RepositoryInput repositoryInput, List<CommitEntry> commitEntries)
        {
            await CreateTable();
            try
            {
                using (SqlConnection = new SqlConnection(GetConnectionStringsFromConfiguratioFile()))
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = SqlConnection;

                    sqlCommand.CommandText = @"INSERT INTO CommitEntry (SHA, Committer, Message)
											VALUES (@SHA, @Committer, @Message);";

                    SqlConnection.Open();
                    
                    foreach (var commit in commitEntries)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@SHA", commit.sha);
                        sqlCommand.Parameters.AddWithValue("@Committer", commit.commit.committer.name);
                        sqlCommand.Parameters.AddWithValue("@Message", commit.commit.message);
                        sqlCommand.ExecuteNonQuery();
                    }
                    Console.WriteLine($"Saving data in database complete");
                    SqlConnection.Close();
                }
            }
            catch (SqlException e)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine($"Application was unable to save information on database:{e.Message}");
            }
        }


        private string GetConnectionStringsFromConfiguratioFile()
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null && settings[0] != null)
            {
                return settings[0].ConnectionString;
            }
            else
            {
                return "";
            }
        }

        private async Task CreateTable()
        {
            try
            {
                using (SqlConnection = new SqlConnection(GetConnectionStringsFromConfiguratioFile()))
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = SqlConnection;

                    sqlCommand.CommandText = @"Create table CommitEntry (ID INT NOT NULL Identity(1,1),
                                                                        SHA nvarchar(60) NOT NULL UNIQUE,  
                                                                        Committer nvarchar(50) NOT NULL, 
                                                                        Message nvarchar(200) NOT NULL)";

                    SqlConnection.Open();
                    await sqlCommand.ExecuteNonQueryAsync();
                    SqlConnection.Close();
                }
            }
            catch (SqlException e)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine($"Application was unable to create new table :{e.Message}");
            }
        }
    }
}
