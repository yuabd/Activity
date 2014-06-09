using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class VoteDetail
    {
        public int ID { get; set; }

        public int VoteID { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}