using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Activity.Models;
using Activity.Models.Others;

namespace Activity.Service
{
	public partial class PhotoService
	{
		private SiteDataContext db = new SiteDataContext();

		#region 图片
		public BaseObject InsertPhoto(Photo photo)
		{
			BaseObject obj = new BaseObject();

			try
			{
				db.Photos.Add(photo);
				db.SaveChanges();
				obj.Tag = 1;
				obj.Message = "新增成功！";
				obj.Result = photo.PhotoID.ToString();
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = "新增失败！";
			}

			return obj;
		}

		public Photo GetPhoto(int id)
		{
			return db.Photos.Find(id);
		}

		public BaseObject UpdatePhoto(Photo photo)
		{
			BaseObject obj = new BaseObject();
			var p = GetPhoto(photo.PhotoID);

			p.PhotoName = photo.PhotoName;

			try
			{
				db.SaveChanges();

				obj.Tag = 1;
				obj.Message = "更新成功！";
			}
			catch (Exception e)
			{
				obj.Tag = -1;
				obj.Message = "更新失败！";
			}

			return obj;
		}

		public BaseObject DelPhoto(int id)
		{
			BaseObject obj = new BaseObject();
			var photo = GetPhoto(id);

			try
			{
				db.Photos.Remove(photo);
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

		#endregion
	}
}