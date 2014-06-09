using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;

namespace Activity.Areas.Admin.Controllers
{
	[Authorize(Users = "admin")]
    public class PictureController : Controller
    {
		private PictureService photoService = new PictureService();
        //
        // GET: /Admin/Picture/

		public ActionResult Index()
		{
			var gd = photoService.GetPhotos().ToList();

			return View(gd);
		}

		[HttpPost]
		public ActionResult AddPicture(Photo picture, HttpPostedFileBase file)
		{
			if (file != null)
			{
				photoService.InsertPhoto(picture, file);
				photoService.Save();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult EditPicture(Photo picture, HttpPostedFileBase file)
		{
			if (file != null)
			{
				var gd = photoService.GetPhoto(picture.PhotoID);
				photoService.DeletePhoto(gd);
				photoService.InsertPhoto(picture, file);

				photoService.Save();
			}
			else
			{
				//socialService.UpdateGalleryDetail(galleryDetail, file);
				photoService.UpdatePhoto(picture);

				photoService.Save();
			}

			return RedirectToAction("Index");
		}

		public ActionResult DeletePicture(int id)
		{
			var galleryDetail = photoService.GetPhoto(id);
			photoService.DeletePhoto(galleryDetail);
			photoService.Save();

			return RedirectToAction("Index");
		}

    }
}
