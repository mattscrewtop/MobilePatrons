﻿using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace MobilePatronsApp.DataContracts
{
	public class DataContractsRegistry : Registry
	{
		public DataContractsRegistry()
		{
			Scan(x =>
			{
				x.TheCallingAssembly();
				x.Convention<DefaultConventionScanner>();
			});
		}
	}
}
