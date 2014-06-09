using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models.Others
{
    public class DataAccess : IDisposable
    {
        protected SiteDataContext db { get; set; }

        public DataAccess(SiteDataContext _db = null)
		{
			if (db == null)
			{
                db = new SiteDataContext();
			}
			else
			{
                db = _db;
			}
		}


		#region IDisposable 成员

		public void Dispose()
		{
			db.Database.Connection.Close();
            db.Dispose();
            db = null;
		}

		#endregion
    }
}