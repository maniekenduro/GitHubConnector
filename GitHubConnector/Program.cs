using GitHubConnector.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            while (command != "exit")
            {
                var repositoryInput = new RepositoryInput();
 
                Console.WriteLine("Hello there! You are in GitHubConnector application");
                Console.WriteLine("To close the application, write 'exit'");

                Console.WriteLine("Lets start - please enter name of GitHub user: ");
                repositoryInput.Login = Console.ReadLine();
                Console.WriteLine("Please enter name of repository: ");
                repositoryInput.RepositoryName = Console.ReadLine();

                MainAsync(repositoryInput);

                Console.ReadKey();
                Console.Clear();
            }
        }  
        
        private static async Task MainAsync(RepositoryInput repositoryInput)
        {
            RESTConnector RESTConnector = new RESTConnector();
            await RESTConnector.GetRepositoryInformation(repositoryInput);

            DatabaseConnector DatabaseConnector = new DatabaseConnector();
            await DatabaseConnector.SaveCommitsEntriesToDatabase(repositoryInput, RESTConnector.CommitsList);
        }

        
    }
}
