using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MobilePatronsApp.DataContracts.Implementations;
using MobilePatronsApp.DataContracts.Interfaces;
using MobilePatronsApp.WebApi.Helpers;

namespace MobilePatronsApp.WebApi.Controllers
{
	/// <summary>
	/// ACCOUNT CONTROLLER
	/// </summary>
	[RoutePrefix("api/email-address")]
	public class EmailAddressController : SecureApiController
	{
		private const int ERROR_USER_LOCKED_OUT = 1;
		private const int ERROR_INVALID_PASSWORD = 2;
		private const int ERROR_PULLING_PATRON_NUMBER = 3;
		private const int ERROR_ACCOUNT_MERGED = 4;
		private const int ERROR_NO_DATA_AVAILABLE = 5;
		private const int ERROR_INVALID_EMAIL_LENGTH = 6;
		private const int ERROR_GENERIC = 20;

		readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
			{ ERROR_USER_LOCKED_OUT, new Tuple<string, string>("The user is locked out", "The user is locked out") },
			{ ERROR_INVALID_PASSWORD, new Tuple<string, string>("Invalid password", "Invalid password") },
			{ ERROR_PULLING_PATRON_NUMBER, new Tuple<string, string>("Error pulling patron number", "Error pulling patron number") },
			{ ERROR_ACCOUNT_MERGED, new Tuple<string, string>("The account tied to this user name has been merged", "The account tied to this user name has been merged") },
			{ ERROR_NO_DATA_AVAILABLE, new Tuple<string, string>("There was an error updating the email", "There was an error updating the email") },
			{ ERROR_INVALID_EMAIL_LENGTH, new Tuple<string, string>("The email address length was not proper", "The email address length was not proper") },
			{ ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error", "Generic Mobile Offers Service error") }
        };
		
		/// <summary>
		/// UPDATE EMAIL ADDRESS
		/// </summary>
		/// <param name="dataModel"></param>
		/// <returns></returns>
		[Route("save")]
		[ResponseType(typeof(IAuthenticatedUser))]
		public HttpResponseMessage PostEmailAddress(UpdatePatronEmailAddress dataModel)
		{
			if (ModelState.IsValid)
			{
				var MOC = new WinStarSoap.MobileOffersClient();

				var responseValue = MOC.updatePatronEmail
					(
						WinstarDataModel.UserName, 
						WinstarDataModel.Password, 
						WinstarDataModel.Facility, 
						WinstarDataModel.IpAddress,
						dataModel.NewEmailAddress
					);

				if (responseValue > 0)
				{
					throw ThrowIfError(responseValue, HttpStatusCode.BadRequest, errors);
				}

				IUserModel currentUser = SecurityHelper.GetWinstarPatron(WinstarDataModel);
				return Request.CreateResponse(HttpStatusCode.OK, currentUser.ToAuthenticatedUser(WinstarDataModel));
			}

			throw ThrowIfError(ERROR_GENERIC, HttpStatusCode.BadRequest, errors, ModelState);
		}
	}
}
