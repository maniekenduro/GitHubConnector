using GitHubConnector.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubConnector
{
    public class RESTConnector
    {
        public List<CommitEntry> CommitsList { get; set; }

        public HttpClient Client { get; set; }

        public RESTConnector()
        {
            CommitsList = new List<CommitEntry>();
            Client = new HttpClient();
        }        

        public async Task GetRepositoryInformation(RepositoryInput input)
        {
            string path = $"https://api.github.com/repos/{input.Login}/{input.RepositoryName}/commits";
            Client.BaseAddress = new Uri(path);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                CommitsList = JsonSerializer.Deserialize<List<CommitEntry>>(responseBody);
                DisplayCommitsEntries(input, CommitsList);
            }
            else
            {
                Console.WriteLine("Application was unable to get repository information");
            }
            Client.CancelPendingRequests();
        }

        private void DisplayCommitsEntries(RepositoryInput input, List<CommitEntry> commitEntries)
        {
            if (commitEntries.Any())
            {
                foreach (CommitEntry entry in commitEntries)
                {
                    Console.WriteLine($"[{input.RepositoryName}]/[{entry.sha}]: {entry.commit.message} [{entry.commit.committer.name}]");
                }
            }
            else
            {
                Console.WriteLine("Application was unable to display commit details.");
            }
        }
       

    }
}
