using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class VolunteerApply
    {
        public int ID { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActiveID { get; set; }
        /// <summary>
        /// 志愿者类型
        /// </summary>
        public string VolunteerType { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount { get; set; }
    }
}