using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models.Others
{
	public class BaseObject
	{
		public int Tag { get; set; }

		public string Message { get; set; }

		public string Result { get; set; }
	}

    public class BaseObject<T>
    {
        public int Tag { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }

	public static class DiscussType
	{
		public const int Active = 1;
		public const int News = 2;
	}
}