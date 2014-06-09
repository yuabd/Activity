using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models.ViewModels
{
    public class VoteViewModel
    {
        public Vote Vote { get; set; }

        public List<VoteDetail> VoteDetails { get; set; }
    }

    public class NameValue
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }
}