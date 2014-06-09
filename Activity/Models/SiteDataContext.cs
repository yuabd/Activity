using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Activity.Models
{
	public class SiteDataContext: DbContext
	{
		public DbSet<Active> Activities { get; set; }
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Apply> Applies { get; set; }
		public DbSet<ActiveTag> Tags { get; set; }
		public DbSet<TagJoin> TagJoins { get; set; }
		public DbSet<Category> Categories { get; set; }
        public DbSet<VolunteerApply> VolunteerApply { get; set; }
        public DbSet<SysConfig> SysConfigs { get; set; }

		public DbSet<Blog> Blogs { get; set; }
		public DbSet<BlogCategory> BlogCategories { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<UserRoleJoin> UserRoleJoins { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }


        public DbSet<Vote> Votes { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }
        public DbSet<VoteDetail> VoteDetails { get; set; }

		public DbSet<Discuss> Discusses { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			base.OnModelCreating(modelBuilder);
		}

		public class SiteDataContextInitializer : DropCreateDatabaseAlways<SiteDataContext>
		{
			protected override void Seed(SiteDataContext context)
			{
				context.SaveChanges();
			}
		}
	}

    public abstract class DbAccess : IDisposable
    {
        protected SiteDataContext db { get; set; }

        public DbAccess(SiteDataContext _db = null)
        {
            if (_db == null)
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