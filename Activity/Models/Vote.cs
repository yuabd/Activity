using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class Vote
    {
        public int ID { get; set; }

        public string VoteName { get; set; }
        
        public string Description { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public string IsSingle { get; set; }
        
        public string UserID { get; set; }

        public string IsLogin { get; set; }
    }
}