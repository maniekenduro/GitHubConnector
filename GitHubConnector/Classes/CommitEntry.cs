using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubConnector.Classes
{
    public class CommitEntry
    { 
        public string sha { get; set; }
        public Commit commit { get; set; }
    }
}
