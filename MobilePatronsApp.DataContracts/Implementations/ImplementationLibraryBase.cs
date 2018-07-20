using System;
using MobilePatronsApp.DataContracts.Interfaces;

namespace MobilePatronsApp.DataContracts.Implementations
{
	public class BaseDataContract : IBaseDataContract
	{
		public Int32 Id { get; set; }
		public String Name { get; set; }
	}
}
