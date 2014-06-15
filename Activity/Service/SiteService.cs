using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Activity.Models;
using Activity.Models.Others;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Activity.Helpers;

namespace Activity.Service
{
    public class SiteService
    {
        private SiteDataContext db = new SiteDataContext();

        #region 活动
        public BaseObject Pass(int id)
        {
            BaseObject obj = new BaseObject();

            var active = GetActive(id);
            if (active == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在！";
            }

            if (active.IsPublic)
            {
                obj.Tag = -1;
                obj.Message = "审核失败，该记录为已审核！";
            }

            active.IsPublic = true;
            obj.Tag = 1;
            obj.Message = "审核成功！";
            db.SaveChanges();

            return obj;
        }

        public BaseObject Good(int id)
        {
            BaseObject obj = new BaseObject();
            var active = db.Activities.Find(id);

            if (active == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在！";
            }

            active.Good += 1;

            Save();
            obj.Tag = 1;

            return obj;
        }

        public BaseObject AddActivity(Active active)
        {
            BaseObject obj = new BaseObject();

            var a = db.Activities.FirstOrDefault(m => m.Title == active.Title);
            if (a != null)
            {
                obj.Tag = -1;
                obj.Message = "该活动名称已经存在";
                return obj;
            }

            if (active.StartDate < DateTime.Now)
            {
                obj.Tag = -2;
                obj.Message = "开始时间不能小于当前时间";

                return obj;
            }

            if (active.StartDate > active.EndDate)
            {
                obj.Tag = -2;
                obj.Message = "开始时间不能大于结束时间";

                return obj;
            }

            try
            {
                //active.StartDate = Convert.ToDateTime(GetDate(active.StartDate, 1));
                //active.EndDate = Convert.ToDateTime(GetDate(active.EndDate, 2));
                active.Good = 0;

                db.Activities.Add(active);
                db.SaveChanges();

                //InsertVolunteer(active);

                var tag = InsertTags(active);

                if (tag.Tag != 1)
                {
                    obj.Tag = -1;
                    obj.Message = "标签新增失败！";
                }

                db.SaveChanges();

                obj.Tag = 1;
                obj.Message += "新增成功！";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "新增失败！";
            }

            return obj;
        }

        //public void InsertVolunteer(Active active)
        //{
        //    try
        //    {
        //        var s = active.Volunteer.Split(';');
        //        foreach (var item in s)
        //        {
        //            if (string.IsNullOrEmpty(item))
        //            {
        //                continue;
        //            }

        //            var volunt = item.Split(',');

        //            var vl = new VolunteerApply();

        //            vl.ActiveID = active.ActiveID;
        //            vl.PeopleCount = volunt[1].Uint();
        //            vl.VolunteerType = volunt[0].UString();

        //            db.VolunteerApply.Add(vl);
        //        }

        //        db.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return;
        //}

        public Active GetActive(int id)
        {
            return db.Activities.Find(id);
        }

        public BaseObject UpdateActive(Active activity)
        {
            BaseObject obj = new BaseObject();

            var b = db.Activities.FirstOrDefault(m => m.Title == activity.Title && m.ActiveID != activity.ActiveID);
            if (b != null)
            {
                obj.Tag = -1;
                obj.Message = "该活动名称已经存在";
                return obj;
            }

            //if (activity.StartDate < DateTime.Now)
            //{
            //    obj.Tag = -2;
            //    obj.Message = "开始时间不能小于当前时间";

            //    return obj;
            //}

            if (activity.StartDate > activity.EndDate)
            {
                obj.Tag = -2;
                obj.Message = "开始时间不能大于结束时间";

                return obj;
            }

            var a = GetActive(activity.ActiveID);

            //a.EndDate = Convert.ToDateTime(GetDate(activity.EndDate, 2));
            //a.StartDate = Convert.ToDateTime(GetDate(activity.StartDate, 1));

            a.Content = activity.Content;
            a.DateCreated = activity.DateCreated;
            a.StartDate = activity.StartDate;
            a.EndDate = activity.EndDate;
            a.Price = activity.Price;
            a.PictureFile = activity.PictureFile;
            a.Address = activity.Address;
            a.CategoryID = activity.CategoryID;
            a.Title = activity.Title;
            a.People = activity.People;
            a.Tags = activity.Tags;
            a.ContactPerson = activity.ContactPerson;
            a.Remind = activity.Remind;
            a.SortOrder = activity.SortOrder;
            a.ContactPeople = activity.ContactPeople;
            a.HomePictureFile = activity.HomePictureFile;
            a.PersonPhone = activity.PersonPhone;
            a.VolunteerCount = activity.VolunteerCount;
            a.IsVolunteerFirst = activity.IsVolunteerFirst;

            if (activity.IsDiscuss == "on")
            {
                a.IsDiscuss = "Y";
            }
            else
            {
                a.IsDiscuss = "N";
            }

            if (!string.IsNullOrEmpty(activity.EndContent))
            {
                a.EndContent = activity.EndContent;
            }

            try
            {
                db.SaveChanges();

                var tag = InsertTags(activity);
                if (tag.Tag == 1)
                {
                    obj.Tag = 1;
                    obj.Message = "更新成功！";
                }
                else
                {
                    obj.Tag = -1;
                    obj.Message = "标签更新失败！";
                }
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "更新失败！";
                throw;
            }

            return obj;
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public BaseObject CheckActive(int id)
        {
            var obj = new BaseObject() { Tag = 1 };
            var active = db.Activities.Find(id);
            if (active == null)
            {
                obj.Tag = -1;
                obj.Message = "系统出错，请联系管理员！";

                return obj;
            }
            if (active.Status == "End")
            {
                obj.Tag = -1;
                obj.Message = "活动已结束,不能报名！";

                return obj;
            }

            return obj;
        }

        public BaseObject DelActive(int id)
        {
            BaseObject obj = new BaseObject();

            var active = GetActive(id);

            try
            {
                var tags = GetTagJoins(id).ToList();
                foreach (var item in tags)
                {
                    db.TagJoins.Remove(item);
                }
                //DeletePicture(active);
                db.Activities.Remove(active);
                db.SaveChanges();
                obj.Tag = 1;
                obj.Message = "删除成功！";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "删除失败！";
                throw;
            }

            return obj;
        }

        public IQueryable<Active> GetActives(string userID)
        {
            return db.Activities.Where(m => m.UserID == userID && m.IsPublic == true).OrderByDescending(m => m.SortOrder).ThenByDescending(m => m.EndDate);
        }

        public IQueryable<Active> GetActives()
        {
            return db.Activities.Where(m => m.IsPublic == true).OrderByDescending(m => m.SortOrder).ThenByDescending(m => m.EndDate);
        }

        public IQueryable<Active> GetWaitActives()
        {
            return db.Activities.Where(m => m.IsPublic == false).OrderByDescending(m => m.SortOrder).ThenByDescending(m => m.EndDate);
        }

        public IQueryable<Active> GetWaitActives(string userID)
        {
            return db.Activities.Where(m => m.IsPublic == false && m.UserID == userID).OrderByDescending(m => m.SortOrder).ThenByDescending(m => m.EndDate);
        }
        //OrderBy(m => m.SortOrder).ThenByDescending(m => m.EndDate)
        public IQueryable<Active> GetActivesByTag(string tag)
        {
            var tags = db.TagJoins.Where(m => m.Tag == tag).Select(m => m.ActiveID);

            var actives = from l in db.Activities
                          where tags.Contains(l.ActiveID)
                          orderby l.SortOrder descending
                          orderby l.EndDate descending
                          select l;

            return actives;
        }

        public List<Active> Search(string keyword)
        {
            var word = keyword.Split(' ');

            var list = from l in db.Activities
                       select l;
            var result = new List<Active>();
            foreach (var item in word)
            {
                list = (from p in list
                        where p.Title.Contains(item) || p.Content.Contains(item) || p.Tags.Contains(item)
                        orderby p.SortOrder descending
                        orderby p.EndDate descending
                        select p);

                result = result.Concat(list).ToList();
            }

            return result;
        }

        // Files

        public BaseObject UploadPicture(Active active, HttpPostedFileBase file)
        {
            BaseObject obj = new BaseObject();
            try
            {
                var oldPicture = active.PictureFile;

                active.PictureFile = string.Format("{0}-{1}.jpg", active.ActiveID, Activity.Helpers.Utilities.GenerateSlug(active.Title, 43));
                Activity.Helpers.IO.UploadImageFile(file.InputStream, active.PictureFolder, active.PictureFile, 680);

                // if one already existed, delete
                if (!string.IsNullOrEmpty(oldPicture) && oldPicture != active.PictureFile)
                {
                    Activity.Helpers.IO.DeleteFile(active.PictureFolder, oldPicture);
                }

                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        private void DeletePicture(Active active)
        {
            Activity.Helpers.IO.DeleteFile(active.PictureFolder, active.PictureFile);
        }

        public BaseObject<string> AddVolunteer(string volunteerType, int? volunteerPeople, int? activeID)
        {
            var obj = new BaseObject<string>() { Tag = 1 };
            if (activeID == null || activeID == 0)
            {
                obj.Tag = -1;
                obj.Message = "新增出错！";

                return obj;
            }

            if (volunteerPeople == null || volunteerPeople == 0)
            {
                obj.Tag = -1;
                obj.Message = "人数异常！";
                return obj;
            }

            try
            {
                var volunteer = new VolunteerApply()
                {
                    ActiveID = activeID.Uint(),
                    PeopleCount = volunteerPeople.Uint(),
                    VolunteerType = volunteerType
                };

                db.VolunteerApply.Add(volunteer);
                db.SaveChanges();

                obj.Result = volunteerType + "," + volunteerPeople + "," + volunteer.ID.UString();
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        public List<VolunteerApply> GetVolunteerByActive(int id)
        {
            var list = (from l in db.VolunteerApply
                        where l.ActiveID == id
                        select l).ToList();

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseObject<string> DeleteVolunteer(int? id)
        {
            var obj = new BaseObject<string>() { Tag = 1 };
            if (id == null || id == 0)
            {
                obj.Tag = -1;
                obj.Message = "删除发生未指定错误，请联系管理员！";
                return obj;
            }

            var voluteer = db.VolunteerApply.Find(id);
            if (voluteer == null)
            {
                obj.Tag = -1;
                obj.Message = "记录已删除！";

                return obj;
            }

            db.VolunteerApply.Remove(voluteer);
            db.SaveChanges();

            return obj;
        }

        #endregion

        #region 分类

        // Blog Category

        public void InsertCategory(Category blogCategory)
        {
            if (string.IsNullOrEmpty(blogCategory.PageTitle))
            {
                blogCategory.PageTitle = blogCategory.CategoryName;
            }

            if (string.IsNullOrEmpty(blogCategory.Slug))
            {
                blogCategory.Slug = Activity.Helpers.Utilities.GenerateSlug(blogCategory.CategoryName, 50);
            }

            db.Categories.Add(blogCategory);
        }

        public Category GetCategory(int id)
        {
            return db.Categories.Find(id);
        }

        public void UpdateCategory(Category blogCategory)
        {
            var bc = GetCategory(blogCategory.CategoryID);

            if (string.IsNullOrEmpty(blogCategory.Slug))
            {
                bc.Slug = Activity.Helpers.Utilities.GenerateSlug(blogCategory.CategoryName, 50);
            }

            bc.CategoryName = blogCategory.CategoryName;
            bc.PageTitle = blogCategory.PageTitle;
            bc.MetaDescription = blogCategory.MetaDescription;
            bc.MetaKeywords = blogCategory.MetaKeywords;
            bc.SortOrder = blogCategory.SortOrder;
        }

        public void DeleteCategory(int categoryID)
        {
            var bc = GetCategory(categoryID);
            db.Categories.Remove(bc);
        }

        public IQueryable<Category> GetCategories()
        {
            return db.Categories.OrderBy(m => m.SortOrder);
        }

        #endregion

        #region 报名

        public BaseObject InsertApply(Apply apply)
        {
            BaseObject obj = new BaseObject();

            if (string.IsNullOrEmpty(apply.UserID))
            {
                obj.Tag = -2;
                obj.Message = "您还未登录，请先登录!";

                return obj;
            }

            var a = db.Applies.FirstOrDefault(m => m.UserID == apply.UserID && m.ActiveID == apply.ActiveID);

            if (a != null)
            {
                obj.Tag = -1;
                obj.Message = "您已经报名该活动，不能重复报名！";

                return obj;
            }

            if (apply.Backup == "Y")
            {
                var applies = db.Applies.Where(m => m.ActiveID == apply.ActiveID && m.Backup == "Y").ToList();
                var active = db.Activities.FirstOrDefault(m => m.ActiveID == apply.ActiveID);

                if ((applies.Sum(m => m.People) + apply.People) >= (active.People * 0.15))
                {
                    obj.Tag = -2;
                    obj.Message = "候补人员报名人数超出！";

                    return obj;
                }
            }
            else if (apply.Backup == "N")
            {
                var active = db.Activities.Find(apply.ActiveID);
                var applies = db.Applies.Where(m => m.ActiveID == apply.ActiveID && m.Backup == "N").ToList();
                if ((applies.Sum(m => m.People) + apply.People) > active.People)
                {
                    obj.Tag = -2;
                    obj.Message = "报名人数超出！";

                    return obj;
                }
            }
            else
            {
                var active = db.Activities.Find(apply.ActiveID);
                var applies = db.Applies.Where(m => m.ActiveID == apply.ActiveID && m.Backup == "V").ToList();
                if (applies.Sum(m => m.People) > active.VolunteerCount)
                {
                    obj.Tag = -2;
                    obj.Message = "志愿者人数已满！";

                    return obj;
                }
            }

            try
            {
                db.Applies.Add(apply);
                db.SaveChanges();
                obj.Tag = 1;
                obj.Message = "报名成功！";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "报名失败, 请联系管理员！";
            }

            return obj;
        }

        public Apply GetApply(int id)
        {
            return db.Applies.Find(id);
        }

        public IQueryable<Apply> GetApplies(int activeID)
        {
            return db.Applies.Where(m => m.ActiveID == activeID);
        }

        public IQueryable<Apply> GetApplies(string userID)
        {
            return db.Applies.Where(m => m.UserID == userID);
        }

        public BaseObject DelApply(int id)
        {
            BaseObject obj = new BaseObject();
            var apply = GetApply(id);
            if (apply == null)
            {
                obj.Tag = -1;
                obj.Message = "记录不存在！";

                return obj;
            }

            try
            {
                var active = db.Activities.Find(apply.ActiveID);
                //var activeID = apply.ActiveID;
                if (active == null)
                {
                    obj.Tag = -2;
                    obj.Message = "对应活动不存在";

                    return obj;
                }

                db.Applies.Remove(apply);

                db.SaveChanges();

                var applies = db.Applies.Where(m => m.ActiveID == active.ActiveID && m.Backup == "N").ToList();

                if (applies.Count < active.People)
                {
                    var backup = db.Applies.Where(m => m.ActiveID == active.ActiveID && m.Backup == "Y").OrderByDescending(m => m.DateCreated).FirstOrDefault();

                    backup.Backup = "N";

                    db.SaveChanges();
                }

                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        #endregion

        #region 标签

        public BaseObject AddTag(string tag)
        {
            BaseObject obj = new BaseObject();

            if (string.IsNullOrEmpty(tag))
            {
                obj.Tag = -1;
                obj.Message = "参数错误";

                return obj;
            }

            try
            {
                foreach (var item in tag.Split(' '))
                {
                    var t = new ActiveTag();
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }

                    t = GetTag(item);

                    if (t != null)
                    {
                        t.IsPublic = "Y";
                    }
                    else
                    {
                        var tt = new ActiveTag();
                        tt.IsPublic = "Y";
                        tt.Tag = item;
                        db.Tags.Add(tt);
                    }
                }

                db.SaveChanges();
                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "添加失败！";
                throw e;
            }

            return obj;
        }

        public BaseObject Remove(string tag)
        {
            BaseObject obj = new BaseObject();

            if (string.IsNullOrEmpty(tag))
            {
                obj.Tag = -1;
                obj.Message = "参数错误";

                return obj;
            }

            try
            {
                var t = GetTag(tag);

                if (t != null)
                {
                    t.IsPublic = "N";
                }

                db.SaveChanges();
                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "移除失败！";
                throw e;
            }

            return obj;
        }

        public BaseObject InsertTag(ActiveTag tag)
        {
            BaseObject obj = new BaseObject();

            try
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                obj.Tag = 1;
                obj.Message = "新增成功！";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "新增失败！";
            }

            return obj;
        }

        public ActiveTag GetTag(string tag)
        {
            return db.Tags.FirstOrDefault(m => m.Tag == tag);
        }

        public IQueryable<ActiveTag> GetTags()
        {
            return db.Tags;
        }

        #endregion

        #region 关联标签

        public BaseObject InsertTags(Active active)
        {
            BaseObject obj = new BaseObject();

            var ts = GetTagJoins(active.ActiveID).ToList();
            try
            {
                foreach (var item in ts)
                {
                    DelTagJoin(item.ActiveID, item.Tag);
                }

                if (!string.IsNullOrEmpty(active.Tags))
                {
                    var result = active.Tags.Split(' ');

                    List<string> tags = new List<string>();

                    for (int i = 0; i < result.Length; i++)
                    {
                        if (!tags.Contains(result[i].ToString()) && !string.IsNullOrEmpty(result[i].ToString()))
                            tags.Add(result[i].ToString().Trim());
                    }

                    foreach (var item in tags)
                    {
                        var tag = GetTag(item);
                        if (tag == null)
                        {
                            tag = new ActiveTag();
                            tag.Tag = item.Trim();
                            tag.IsPublic = "N";
                            InsertTag(tag);
                        }

                        var tagjoin = new TagJoin();
                        tagjoin.Tag = item;
                        tagjoin.ActiveID = active.ActiveID;
                        db.TagJoins.Add(tagjoin);
                    }
                }
                db.SaveChanges();
                obj.Tag = 1;
                obj.Message = "新增成功!";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                throw;
            }

            return obj;
        }

        public TagJoin GetTagJoin(int id, string tag)
        {
            return db.TagJoins.FirstOrDefault(m => m.ActiveID == id && m.Tag == tag);
        }

        public void DelTagJoin(int id, string tag)
        {
            var t = GetTagJoin(id, tag);

            db.TagJoins.Remove(t);
        }

        public IQueryable<TagJoin> GetTagJoins(int id)
        {
            return db.TagJoins.Where(m => m.ActiveID == id);
        }

        public IQueryable<TagJoin> GetTagJoins(string tag)
        {
            return db.TagJoins.Where(m => m.Tag == tag);
        }

        #endregion

        public static string SmsIp = System.Configuration.ConfigurationManager.AppSettings["SmsIp"];
        public static string SmsPortString = System.Configuration.ConfigurationManager.AppSettings["SmsPort"];
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phones">多个号码用;号隔开</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public BaseObject SendSms(string phones, string content, string userName, int? idss)
        {
            var obj = new BaseObject();
            try
            {
                //Thread objThread = new Thread(new ThreadStart(delegate{ ConnectSend(phones, content, userName, idss, ref obj)};)); 

                //objThread.Start();
            }
            catch (Exception e)
            {
                //Console.WriteLine("argumentNullException: {0}", e); 
                obj.Tag = -1;
                obj.Message = e.Message;
            }
            if (idss == null)
            {
                var log = new SmsLog();
                log.IsSuccess = obj.Tag == 1 ? "Y" : "N";
                log.Number = phones;
                log.ErrorMessage = obj.Message;
                log.DateCreated = DateTime.Now;
                log.Content = content;
                log.OperaUser = userName;
                db.SmsLogs.Add(log);
            }
            else
            {
                var log = GetSmsLog((int)idss);
                log.IsSuccess = obj.Tag == 1 ? "Y" : "N";
                //log.Number = phones;
                log.ErrorMessage = obj.Message;
                log.DateCreated = DateTime.Now;
                //log.Content = content;
                log.OperaUser = userName;
            }

            Save();

            return obj;
        }

        //public delegate void ConnectSend(string phones, string content, string userName, int? idss, ref BaseObject obj);

        private void ConnectSend(string phones, string content, string userName, int? idss, ref BaseObject obj)
        {
            /**/
            ///创建终结点EndPoint 
            int SmsPort = Convert.ToInt32(SmsPortString);
            IPAddress ip = IPAddress.Parse(SmsIp);
            IPEndPoint ipe = new IPEndPoint(ip, SmsPort);//把ip和端口转化为IPEndpoint实例 

            /**/
            ///创建socket并连接到服务器 
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket 
            //Console.WriteLine("Conneting…"); 
            c.Connect(ipe);//连接到服务器

            /**/
            ///向服务器发送信息 
            string sendStr = "@b" + phones.Trim(';') + "|" + content + " |2@e";
            byte[] bs = Encoding.Unicode.GetBytes(sendStr);//把字符串编码为字节 

            //Console.WriteLine("Send Message"); 
            c.Send(bs, bs.Length, 0);//发送信息 

            ///接受从服务器返回的信息 
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息 
            recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
            //Console.WriteLine("client get message:{0}", recvStr);//显示服务器返回信息 
            if (recvStr == "@ba1@e")
            {
                obj.Tag = 1;
                obj.Message = recvStr;
            }
            /**/
            ///一定记着用完socket后要关闭 
            c.Close();
        }

        public SmsLog GetSmsLog(int id)
        {
            return db.SmsLogs.Find(id);
        }

        public List<SmsLog> GetSmsLogs()
        {
            return db.SmsLogs.OrderByDescending(m => m.DateCreated).ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        ~SiteService()
        {
            db.Dispose();
        }
    }
}