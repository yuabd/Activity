using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Activity.Models;
using Activity.Models.Others;
using Activity.Models.ViewModels;

namespace Activity.Service
{
	public class VoteService : DataAccess
	{
		public VoteService()
			: base()
		{
		}

		public string GetDate(DateTime? date, int type)
		{
			var d = "";
			if (!string.IsNullOrEmpty(date.ToString()))
			{
				if (type == 1)
				{
					if (date != null) d = ((DateTime)date).ToString("yyyy-MM-dd") + " 00:00:00";
				}
				else
				{
					if (date != null) d = ((DateTime)date).ToString("yyyy-MM-dd") + " 23:59:59";
				}
			}
			
			return d;
		}

		#region 投票

		public BaseObject InsertVote(Vote vote, string names)
		{
			BaseObject obj = new BaseObject();
			if (vote == null)
			{
				obj.Tag = -1;
				obj.Message = "系统错误!";
				return obj;
			}
			try
			{
				vote.StartDate = Convert.ToDateTime(GetDate(vote.StartDate, 1));
				vote.EndDate = Convert.ToDateTime(GetDate(vote.EndDate, 2));
				db.Votes.Add(vote);
				db.SaveChanges();

				var listName = names.Trim(';').Split(';');
				listName = listName.Distinct().ToArray();
				foreach (var item in listName)
				{
					var voteDetail = new VoteDetail();
					if (!string.IsNullOrEmpty(item))
					{
						voteDetail.VoteID = vote.ID;
						voteDetail.Name = item;
						voteDetail.Count = 0;

						db.VoteDetails.Add(voteDetail);
					}
				}

				db.SaveChanges();
				obj.Tag = 1;
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = e.Message;
			}

			return obj;
		}

		public Vote GetVoteByID(int id)
		{
			var vote = db.Votes.Find(id);

			return vote;
		}

		public VoteDetail GetVoteDetailByID(int id)
		{
			var detail = db.VoteDetails.Find(id);
			return detail;
		}

		//public VoteViewModel GetVoteAll(int id)
		//{
		//    var vote = (from l in db.Votes
		//               where l.ID == id
		//               select new VoteViewModel()
		//               {
		//                   Description = l.Description,
		//                   EndDate = l.EndDate,
		//                   ID = l.ID,
		//                   IsLogin = l.IsLogin,
		//                   IsSingle = l.IsSingle,
		//                   StartDate = l.StartDate,
		//                   UserID = l.UserID,
		//                   VoteName = l.VoteName
		//               }).FirstOrDefault();

		//    var voteDet = (from l in db.VoteDetails
		//                  where l.VoteID == id
		//                  select new VoteDet()
		//                  {
		//                      ID = l.ID,
		//                      Name = l.Name,
		//                      VoteID = l.VoteID
		//                  }).ToList();

		//    vote.VoteDetails = voteDet;

		//    return vote;
		//}

		public BaseObject UpdateVoteDetail(VoteDetail voteDetail)
		{
			var obj = new BaseObject();
			var vd = db.VoteDetails.Find(voteDetail.ID);

			vd.Name = voteDetail.Name;

			db.SaveChanges();
			obj.Tag = 1;

			return obj;
		}

		public BaseObject UpdateVote(Vote vote, string names)
		{
			BaseObject obj = new BaseObject();
			try
			{
				var v = db.Votes.Find(vote.ID);

				if (v == null)
				{
					obj.Tag = -1;
					obj.Message = "记录不存在!";
					return obj;
				}

				v.Description = vote.Description;
				v.EndDate = Convert.ToDateTime(GetDate(vote.EndDate, 2));
				v.IsLogin = vote.IsLogin;
				v.IsSingle = vote.IsSingle;
				v.VoteName = vote.VoteName;

				var str = new List<string>();
				var listName = names.Trim(';').Split(';');
				//listName = listName.Distinct().ToArray();
				foreach (var item in listName)
				{
					str.Add(item.Split(',')[1].ToString());
				}

				//var details = db.VoteDetails.Where(m => m.VoteID == v.ID && (str.Contains(m.Name))).ToList();
				//foreach (var item in details)
				//{
				//    db.VoteDetails.Remove(item);
				//}

				//var nameList = names.Split(';');
				//nameList = nameList.Distinct().ToArray();
				foreach (var item in listName)
				{
					var sti = item.Split(',');
					var id = Convert.ToInt32(sti[0]);
					var name = sti[1].ToString();
					var details = db.VoteDetails.FirstOrDefault(m => m.VoteID == v.ID && m.ID != id && (!str.Contains(m.Name)));
					if (details != null)
					{
						db.VoteDetails.Remove(details);
					}

					if (id != 0)
					{
						var detail = db.VoteDetails.FirstOrDefault(m => m.ID == id);
						if (detail == null)
						{
							continue;
						}
						detail.Name = name;
					}
					else
					{
						var detail = new VoteDetail();
						detail.Name = name;
						detail.VoteID = v.ID;
						detail.Count = 0;

						db.VoteDetails.Add(detail);
					}

					//if (!string.IsNullOrEmpty(item))
					//{
					//    var detail = new VoteDetail();
					//    detail.Name = item; detail.VoteID = v.ID;
					//    detail.Count = 0;

					//    db.VoteDetails.Add(detail);
					//}
				}

				db.SaveChanges();

				obj.Tag = 1;
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = e.Message;
			}

			return obj;
		}

		public BaseObject DeleteVote(int id)
		{
			BaseObject obj = new BaseObject();
			var vote = db.Votes.Find(id);

			if (vote == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在!";
				return obj;
			}

			var details = db.VoteDetails.Where(m => m.VoteID == id);
			foreach (var item in details)
			{
				db.VoteDetails.Remove(item);
			}

			db.Votes.Remove(vote);
			db.SaveChanges();
			obj.Tag = 1;

			return obj;
		}


		public List<Vote> GetVoteList()
		{
			var votes = from l in db.Votes
						orderby l.StartDate descending
						select l;

			return votes.ToList();
		}

		#endregion

		#region 详情

		public List<VoteDetail> GetVoteDetailByVoteID(int id)
		{
			var list = from l in db.VoteDetails
					   where l.VoteID == id
					   select l;

			return list.ToList();
		}

		public BaseObject DeleteVoteDetial(int id)
		{
			var obj = new BaseObject();
			var detail = db.VoteDetails.Find(id);
			db.VoteDetails.Remove(detail);

			db.SaveChanges();
			obj.Tag = 1;
			return obj;
		}

		#endregion

		public BaseObject InsertUserVote(UserVote userVote)
		{
			var obj = new BaseObject();
			var vote = db.Votes.FirstOrDefault(m => m.ID == userVote.VoteID);
			if (vote.IsLogin == "Y")
			{
				if (db.UserVotes.Any(m => m.VoteID == userVote.VoteID && m.UserID == userVote.UserID))
				{
					obj.Tag = -1;
					obj.Message = "您已经投过票了!";
					return obj;
				}
			}

			try
			{

				db.UserVotes.Add(userVote);
				var details = userVote.VoteDetailID.Split(';');
				foreach (var item in details)
				{
					//var uv = new UserVote();
					//uv.UserID = userVote.UserID;
					//uv.VoteDetailID = userVote.VoteDetailID;
					//uv.VoteID = userVote.VoteID;

					//db.UserVotes.Add(uv);

					if (!string.IsNullOrEmpty(item))
					{
						var id = Convert.ToInt32(item);
						var det = db.VoteDetails.FirstOrDefault(m => m.ID == id);
						det.Count += 1;
					}
				}
				db.SaveChanges();
				obj.Tag = 1;
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = e.Message;
			}

			return obj;
		}

		public List<UserVote> GetUserVotes()
		{
			var list = from l in db.UserVotes
					   select l;
			return list.ToList();
		}

		#region 讨论贴

		public BaseObject InsertDiscuss(Discuss discuss)
		{
			var obj = new BaseObject();

			try
			{
				discuss.Name = discuss.UserID;
				var parent = db.Discusses.Find(discuss.ParentID);
				if (parent != null)
				{
					discuss.LinkUrl += parent.ID + ",";
				}
				else
				{
					discuss.LinkUrl = "";
				}

				db.Discusses.Add(discuss);
				db.SaveChanges();

				obj.Tag = 1;
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = e.Message;
			}

			return obj;
		}

		public List<DiscussList> GetDiscusses(int id, int type, string linkUrl)
		{
			var list = new List<DiscussList>();
			if (linkUrl != "")
			{
				var parentID = linkUrl.Split(',')[0];
				var parent = Convert.ToInt32(parentID);
				list = (from l in db.Discusses
						where l.LinkUrl.StartsWith(parentID) || l.ID == parent
						select new DiscussList()
						{
							Content = l.Content,
							DateCreated = l.DateCreated,
							ID = l.ID,
							LinkUrl = l.LinkUrl,
							Name = l.Name,
							ParentID = l.ParentID,
							UserID = l.UserID
						}).ToList();
			}
			else
			{
				list = (from l in db.Discusses
						where l.TargetID == id && l.Type == type
						select new DiscussList()
						{
							Content = l.Content,
							DateCreated = l.DateCreated,
							ID = l.ID,
							LinkUrl = l.LinkUrl,
							Name = l.Name,
							ParentID = l.ParentID,
							UserID = l.UserID
						}).ToList();
			}

			list.ForEach(m =>
				m.HuifuName = (from q in db.Discusses where q.ID == m.ParentID select q.Name).FirstOrDefault());

			return list.ToList();
		}

		#endregion
	}
}