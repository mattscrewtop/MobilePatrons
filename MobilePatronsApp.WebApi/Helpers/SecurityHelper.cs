using MobilePatronsApp.DataContracts.Helpers;
using MobilePatronsApp.DataContracts.Implementations;
using MobilePatronsApp.DataContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace MobilePatronsApp.WebApi.Helpers
{
	/// <summary>
	/// Security Helper used for Authentication, Registration
	/// </summary>
	public static class SecurityHelper
	{
		
		/// <summary>
		/// LOGIN AND AUTHENTICATE USER VIA A LOGIN FORM
		/// </summary>
		/// <param name="loginModel"></param>
		/// <returns></returns>
		public static IUserModel GetWinstarPatron(IWinstarDataModel dataModel)
		{
			var MOC = new WinStarSoap.MobileOffersClient();
			var userName = dataModel.UserName;
			var password = dataModel.Password;
			var facility = dataModel.Facility;
			var ipAddress = dataModel.IpAddress;

			try
			{
				WinStarSoap.patron patron = MOC.getPatron(userName, password, facility, ipAddress);

				IUserModel userModel = new UserModel();
                userModel.AcctType = patron.acctType;
                userModel.DOB = patron.dob;
                userModel.FirstName = patron.firstName;
                userModel.MiddleInit = patron.midInit;
                userModel.LastName = patron.lastName;
				userModel.PhoneNumber = patron.phone;
                userModel.MobileNumber = patron.mobile;
                userModel.EmailAddress = patron.email;
				userModel.EmailFlag = (patron.emailFlag == "Y");
				userModel.AddressLine1 = patron.addr1;
				userModel.AddressLine2 = patron.addr2;
				userModel.City = patron.city;
				userModel.State = patron.state;
				userModel.Zip = patron.zip;
				userModel.CashAvailable = patron.cashAvailable;
				userModel.CurrentCardLevel = patron.currentCardLevel;
				userModel.CurrentPoints = patron.currentPoints;
				userModel.PointsAvailable = patron.pointsAvailable;
				userModel.PointsToNextLevel = patron.pointsToNextLevel;
				userModel.PointsToNextLevelByDate = patron.pointsToNextLevelByDate;
				userModel.NextCardLevel = patron.nextCardLevel;
				userModel.SixMonthADT = patron.sixMonthADT;
				userModel.Facility = patron.facility;
				userModel.IsGolfMember = (patron.golfMember == "Y");
				userModel.HostFlag = patron.hostFlag;
                userModel.NewsFlag = patron.newsFlag;
                userModel.HostName = patron.hostName;
                userModel.Token = patron.token;
                userModel.ErrorNumber = patron.error;

				return userModel;
			}
			catch (Exception ex)
			{				
				throw new Exception("Error happened retrieving patron", ex);
			}
		}


		/// <summary>
		/// CONVERTS USER MODEL TO AUTHENTICATED USER (EXTENDS IUserModel)
		/// </summary>
		/// <param name="currentUser"></param>
		/// <param name="winstarDataModel"></param>
		/// <returns></returns>
		public static IAuthenticatedUser ToAuthenticatedUser(this IUserModel currentUser, IWinstarDataModel winstarDataModel)
		{
			var token = AuthTokenHelper.GenerateAuthToken(winstarDataModel);

			var autehnticatedUser = new AuthenticatedUser
			{
				CurrentUser = currentUser,
				Token = token
			};

			return autehnticatedUser;
		}


		/// <summary>
		/// CREATES CLAIMS PRINCIPAL (EXTENDS IUserModel)
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns></returns>
		public static ClaimsPrincipal ToClaimsPrincipal(this IUserModel userModel)
		{
			var emailAddress = userModel.EmailAddress ?? "";

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Sid, emailAddress.ToString(CultureInfo.InvariantCulture)),
				new Claim(ClaimTypes.NameIdentifier, emailAddress),
				new Claim(ClaimTypes.GivenName, userModel.FirstName ?? ""),
				new Claim(ClaimTypes.Surname, userModel.LastName ?? ""),
				new Claim(ClaimTypes.Name, String.Format("{0} {1}", userModel.FirstName ?? "", userModel.LastName ?? "")),
				new Claim(ClaimTypes.Email, emailAddress)
			};

			return new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuthentication"));
		}
	}
}