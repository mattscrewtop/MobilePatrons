using MobilePatronsApp.DataContracts.Interfaces;
using System;
using System.Configuration;
using System.Web;

namespace MobilePatronsApp.DataContracts.Implementations
{
	public class LoginRetrievalModel : ILoginRetrievalModel
	{
		public String PatronNumber { get; set; }
		public String Pin { get; set; }
	}

	public class RegistrationModel : IRegistrationModel
	{
		public String UserName { get; set; }
		public String Password { get; set; }
		public String PatronNumber { get; set; }
		public String Pin { get; set; }
	}

	public class LoginModel : ILoginModel
	{
		public String UserName { get; set; }
		public String Password { get; set; }
	}

	public class WinstarDataModel : IWinstarDataModel
	{
		public String UserName { get; set; }
		public String Password { get; set; }
		public String Facility { get; set; }
		public String IpAddress { get; set; }

		public WinstarDataModel()
		{
			Facility = ConfigurationManager.AppSettings["Facility"];
			IpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";
		}
	}

	public class UserModel : IUserModel
    {
        public String AcctType { get; set; }
        public String DOB { get; set; }
        public String FirstName { get; set; }
        public String MiddleInit { get; set; }
        public String LastName { get; set; }
		public String PhoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String EmailAddress { get; set; }
		public Boolean EmailFlag { get; set; }
        public String AddressLine1 { get; set; }
		public String AddressLine2 { get; set; }
		public String City { get; set; }
		public String State { get; set; }
		public String Zip { get; set; }
		public String CashAvailable { get; set; }
		public String CurrentCardLevel { get; set; }
		public String CurrentPoints { get; set; }
		public String PointsAvailable { get; set; }
		public String PointsToNextLevel { get; set; }
		public String PointsToNextLevelByDate { get; set; }
		public String NextCardLevel { get; set; }
		public String SixMonthADT { get; set; }
		public String Facility { get; set; }
		public Boolean IsGolfMember { get; set; }
		public String HostFlag { get; set; }
        public String NewsFlag { get; set; }
        public String HostName { get; set; }
        public String Token { get; set; }
        public String ErrorNumber { get; set; }
	}

	public class AuthenticatedUser : IAuthenticatedUser
	{
		public IUserModel CurrentUser { get; set; }
		public String Token { get; set; }
	}
}
