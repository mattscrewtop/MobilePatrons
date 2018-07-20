using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MobilePatronsApp.DataContracts.Interfaces
{
	public interface ILoginRetrievalModel
	{
		String PatronNumber { get; set; }
		String Pin { get; set; }
	}

	public interface IRegistrationModel
	{
		[Required]
		[DataMember(IsRequired = true)]
		String UserName { get; set; }

		[Required]
		[DataMember(IsRequired = true)]
		String Password { get; set; }

		[Required]
		[DataMember(IsRequired = true)]
		String PatronNumber { get; set; }

		[Required]
		[DataMember(IsRequired = true)]
		String Pin { get; set; }
	}

	public interface ILoginModel
	{
		[Required]
		[DataMember(IsRequired = true)]
		String UserName { get; set; }

		[Required]
		[DataMember(IsRequired = true)]
		[DataType(DataType.Password)]
		String Password { get; set; }
	}

	public interface IWinstarDataModel
	{
		String UserName { get; set; }
		String Password { get; set; }
		String Facility { get; set; }
		String IpAddress { get; set; }
	}

	public interface IUserModel
    {
        String AcctType { get; set; }
        String DOB { get; set; }
        String FirstName { get; set; }
        String MiddleInit { get; set; }
        String LastName { get; set; }
		String PhoneNumber { get; set; }
        String MobileNumber { get; set; }
        String EmailAddress { get; set; }
		Boolean EmailFlag { get; set; }
		String AddressLine1 { get; set; }
		String AddressLine2 { get; set; }
		String City { get; set; }
		String State { get; set; }
		String Zip { get; set; }
		String CashAvailable { get; set; }
		String CurrentCardLevel { get; set; }
		String CurrentPoints { get; set; }
		String PointsAvailable { get; set; }
		String PointsToNextLevel { get; set; }
		String PointsToNextLevelByDate { get; set; }
		String NextCardLevel { get; set; }
		String SixMonthADT { get; set; }
		String Facility { get; set; }
		Boolean IsGolfMember { get; set; }
		String HostFlag { get; set; }
        String NewsFlag { get; set; }
        String HostName { get; set; }
        String Token { get; set; }
        String ErrorNumber { get; set; }
	}

	public interface IAuthenticatedUser
	{
		IUserModel CurrentUser { get; set; }
		String Token { get; set; }
	}
}
