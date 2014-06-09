using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Activity.Models;
using Activity.Models.Others;

namespace Activity.Service
{
    public class SysConfigHelper
    {
        public SysConfigHelper()
        {
        }

        public SysConfig GetSysConfig()
        {
            using (SysConfigService sc = new SysConfigService())
            {
                return sc.GetSysConfig();
            }
        }

        public BaseObject UpdateSysConfig(SysConfig sysConfig)
        {
            using (SysConfigService sc = new SysConfigService())
            {
                return sc.UpdateSysConfig(sysConfig);
            }
        }

        public int GetVisitCount()
        {
            using (SysConfigService sc = new SysConfigService())
            {
                return sc.GetVisitCount();
            }
        }

        public void AddVisit()
        {
            using (SysConfigService sc = new SysConfigService())
            {
                sc.AddVisit();
            }
        }
    }

    public class SysConfigService : DbAccess
    {
        public SysConfigService()
        {
        }

        public SysConfig GetSysConfig()
        {
            var config = db.SysConfigs.FirstOrDefault();

            if (config == null)
            {
                config = new SysConfig()
                {
                    AdsContent = "http://10.130.188.24:8008/content/images/201405/20140534676ffd-eac5-412e-b8f2-9a067cf75e8b.jpg",
                    IsShowBox = false,
                    IsShowName = true,
                    Notice = "本网站平台属于警营里的每一份子，任何人都可以来发起活动、组织活动、分享活动~~~&#xA;&#xA;只要您有开展活动的任何需求，都可以在此发布，必要的审核后，即可发起、组织，我们还将根据情况给予物资、政策、协调资源等方面的支持。&#xA;&#xA;",
                    Phone = "2262145",
                    Url = "http://10.130.188.24:8008/",
                    WebsiteName = "厦门市公安局机关团委活动网站",
                    VisitCount = 24161
                };

                db.SysConfigs.Add(config);
                db.SaveChanges();
            }

            return config;
        }

        /// <summary>
        /// 更新系统参数
        /// </summary>
        /// <param name="sysConfig"></param>
        /// <returns></returns>
        public BaseObject UpdateSysConfig(SysConfig sysConfig)
        {
            var obj = new BaseObject();

            var sc = GetSysConfig();

            sc.AdsContent = sysConfig.AdsContent;
            sc.IsShowBox = sysConfig.IsShowBox;
            sc.IsShowName = sysConfig.IsShowName;
            sc.Notice = sysConfig.Notice;
            sc.Phone = sysConfig.Phone;
            sc.Url = sysConfig.Url;
            sc.WebsiteName = sysConfig.WebsiteName;

            db.SaveChanges();
            obj.Tag = 1;

            return obj;
        }

        public int GetVisitCount()
        {
            var config = GetSysConfig();

            return config.VisitCount;
        }

        public void AddVisit()
        {
            var config = GetSysConfig();

            config.VisitCount += 1;

            db.SaveChanges();
        }
    }
}