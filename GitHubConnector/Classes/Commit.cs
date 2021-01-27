using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubConnector.Classes
{
    public class Commit
    {
        public Committer committer { get; set; }
        public string message { get; set; }
    }
}
