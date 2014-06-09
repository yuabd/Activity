using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class UserVote
    {
        public int ID { get; set; }

        public int VoteID { get; set; }

        public string VoteDetailID { get; set; }

        public string UserID { get; set; }
    }
}