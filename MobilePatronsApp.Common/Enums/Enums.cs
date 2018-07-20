using System.ComponentModel;

namespace MobilePatronsApp.Common.Enums
{
	public enum AuthenticationType
	{
		[Description("Windows")]
		Windows = 1,
		[Description("Form")]
		Form = 2,
		[Description("Facebook")]
		Facebook = 3,
		[Description("Twitter")]
		Twitter = 4,
		[Description("Google")]
		Google = 5,
		[Description("Yahoo")]
		Yahoo = 6,
		[Description("LinkedIn")]
		LinkedIn = 7,
		[Description("Unknown")]
		Unknown = 8,
		[Description("ByClient")]
		ByClient = 9
	}
}
