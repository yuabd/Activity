using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    /// <summary>
    /// 短信记录
    /// </summary>
    public class SmsLog
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string IsSuccess { get; set; }
        
        public string ErrorMessage { get; set; }
        public string OperaUser { get; set; }
    }
}