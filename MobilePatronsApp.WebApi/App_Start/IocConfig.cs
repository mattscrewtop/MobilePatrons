﻿using System.Web.Http;
using System.Web.Mvc;
using MobilePatronsApp.WebApi.DependencyResolution;

namespace MobilePatronsApp.WebApi
{
	public class IocConfig
	{
		public static void Register()
		{
			var container = IoC.Initialize();
			DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
		}
	}
}