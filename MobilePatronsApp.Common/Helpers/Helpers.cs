using System;

namespace MobilePatronsApp.Common.Helpers
{
	public static class Helpers
	{
		public static string EncodeTo64(string toEncode)
		{
			var toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(toEncode);
			return Convert.ToBase64String(toEncodeAsBytes);
		}

		#region STRING EXTENSIONS
		public static Boolean IsSameAs(this string str, string target)
		{
			return (String.Compare(str.Trim().ToLower(), target.Trim().ToLower(), StringComparison.Ordinal) == 0);
		}
		#endregion STRING EXTENSIONS
	}
}
