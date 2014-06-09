using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class SysConfig
    {
        public int ID { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebsiteName { get; set; }
        /// <summary>
        /// 网站网址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 电话传真
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 是否显示名字
        /// </summary>
        public bool IsShowName { get; set; }
        /// <summary>
        /// 是否显示主页弹窗
        /// </summary>
        public bool IsShowBox { get; set; }
        /// <summary>
        /// 主页弹窗内容
        /// </summary>
        public string AdsContent { get; set; }
        /// <summary>
        /// 访问人数
        /// </summary>
        public int VisitCount { get; set; }
    }
}