﻿using System.Configuration;

namespace MobilePatronsApp.Repository
{
	public static class Database
	{
		private static readonly string DatabaseConnectionString = InitializeConnectionString();

		public static string ConnectionString
		{
			get { return DatabaseConnectionString; }
		}

		private static string InitializeConnectionString()
		{
			return ConfigurationManager.ConnectionStrings["MobilePatronsAppDatabaseConnection"].ConnectionString;
		}
	}
}
