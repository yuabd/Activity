using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Activity.Models;
using Activity.Models.Others;

namespace Activity.Service
{
	public class PictureService
	{
		private SiteDataContext db = new SiteDataContext();

		public void InsertPhoto(Photo photo, HttpPostedFileBase file)
        {
            if (file != null)
            {
				UploadGalleryPhoto(photo, file);
	            photo.ActivityID = 0;

				db.Photos.Add(photo);
            }
        }

		//SiteSettings siteSettings = new SiteSettings();

		public BaseObject AddHomePhoto(int id)
		{
			BaseObject obj = new BaseObject();
			var active = db.Activities.Find(id);
			if (active == null)
			{
				obj.Tag = -1;
				return obj;
			}
			Photo photo = new Photo();
			photo.PhotoName = active.Title;
			photo.PhotoFile = active.HomePictureFile;
			photo.Url = "/home/detail/" + id;
			photo.ActivityID = 0;
            photo.SortOrder = 0;
			db.Photos.Add(photo);
			Save();
			obj.Tag = 1;

			return obj;
		}
 

		public Photo GetPhoto(int photoId)
        {
            return db.Photos.FirstOrDefault(m => m.PhotoID == photoId);
        }

		public void UpdatePhoto(Photo photo)
        {
			var gd = GetPhoto(photo.PhotoID);
			gd.PhotoName = photo.PhotoName;
			gd.Url = photo.Url;
            gd.SortOrder = photo.SortOrder;
        }

        public void DeletePhoto(Photo photo)
        {
            var gd = GetPhoto(photo.PhotoID);
            Activity.Helpers.IO.DeleteFile(gd.PhotoFolder, gd.PhotoFile);

			db.Photos.Remove(gd);
        }

        public IQueryable<Photo> GetPhotos()
        {
	        return db.Photos.Where(m => m.ActivityID == 0).OrderByDescending(m => m.SortOrder);
        }

        // Upload Photo

		public void UploadGalleryPhoto(Photo photo, HttpPostedFileBase file)
		{
			var guid = Guid.NewGuid();
			//string filename = string.Format("{0}.jpg", guid);
            // delete to overwrite
            //Activity.Helpers.IO.DeleteFile(photo.PhotoFolder, photo.PhotoFile);
            // update filename
			string saveUrl = DateTime.Now.ToString("yyyyMM") + @"/" + DateTime.Now.ToString("yyyyMM") + guid + ".jpg";
			photo.PhotoFile = saveUrl;

			string url = System.Web.HttpContext.Current.Server.MapPath("/content/images/" + saveUrl);

			string directory = Path.GetDirectoryName(url);
			if (directory != null && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			if (file != null) file.SaveAs(url);
        }

        //  others

        public void Save()
        {
            db.SaveChanges();
        }

		~PictureService()
		{
			db.Dispose();
		}
	}
}