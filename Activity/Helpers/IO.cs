using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Activity.Helpers
{
	public class IO
	{
		public static void UploadImageFile(Stream fileStream, string folderPath, string filename, int maxSideSize)
		{
			using (Image image = Image.FromStream(fileStream))
			{
				int oldWidth = image.Width;
				int oldHeight = image.Height;

				int newWidth, newHeight;

				int maxSide = oldWidth >= oldHeight ? oldWidth : oldHeight;

				if (maxSide > maxSideSize)
				{
					double coeficient = maxSideSize / (double)maxSide;
					newWidth = Convert.ToInt32(coeficient * oldWidth);
					newHeight = Convert.ToInt32(coeficient * oldHeight);
				}
				else
				{
					newWidth = oldWidth;
					newHeight = oldHeight;
				}

				using (Bitmap newImage = new Bitmap(image, newWidth, newHeight))
				{
					string fullPath = HttpContext.Current.Server.MapPath(folderPath + "/" + filename);
					DeleteFile(folderPath, filename);
					newImage.Save(fullPath, ImageFormat.Jpeg);
				}

			}
		}

		public static void DeleteFile(string folderPath, string filename)
		{
			string fullPath = HttpContext.Current.Server.MapPath(folderPath + "/" + filename);
			try
			{
				File.Delete(fullPath);	//avoid failure if file doesn't exist
			}
			catch
			{
				//TODO:handle errors here
			}
		}
	}
}
