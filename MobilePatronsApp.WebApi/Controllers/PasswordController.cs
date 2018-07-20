using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
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
	[RoutePrefix("api/password")]
	public class PasswordController : SecureApiController
	{
		private const int ERROR_USER_LOCEKED_OUT = 1;
		private const int ERROR_INVALID_PASSWORD_LENGTH = 2;
		private const int ERROR_PULLING_PATRON_NUMBER = 3;
		private const int ERROR_ACCOUNT_MERGED = 4;
		private const int ERROR_PASSWORD_LENGTH_INVALID = 5;
		private const int ERROR_INVALID_PASSWORD_NO_LOWERCASE_LETTER = 6;
		private const int ERROR_INVALID_PASSWORD_NO_UPPERCASE_LETTER = 7;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_DIGIT = 8;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_LETTER = 9;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_SPECIAL_CHARACTER = 10;
		private const int ERROR_INVALID_PATRON_NUMBER = 11;
		private const int ERROR_SAVING_LOGIN = 12;
		private const int ERROR_INVALID_PIN = 13;
		private const int ERROR_ERROR_VALIDATING_PATRON = 14;
		private const int ERROR_REACHING_GATEWAY = 15;
		private const int ERROR_INVALID_CHARACTER_PASSWORD = 16;
		private const int ERROR_PATRON_NUMBER_LOCKEDOUT = 17;
		private const int ERROR_MISSING_PATRON_NUMBER = 18;
		private const int ERROR_PIN_MUST_BE_4_DIGITS = 19;
		private const int ERROR_GENERIC = 20;

		readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
			{ ERROR_USER_LOCEKED_OUT, new Tuple<string, string>("The user (login/pwd) is locked out", "The user (login/pwd) is locked out") },
			{ ERROR_INVALID_PASSWORD_LENGTH, new Tuple<string, string>("Invalid password", "Invalid password") },
			{ ERROR_PULLING_PATRON_NUMBER, new Tuple<string, string>("Error pulling patron number", "Error pulling patron number") },
			{ ERROR_ACCOUNT_MERGED, new Tuple<string, string>("The account tied to this user name has been merged", "The account tied to this user name has been merged") },
			{ ERROR_PASSWORD_LENGTH_INVALID, new Tuple<string, string>("The password length is improper", "The password length is improper") },
			{ ERROR_INVALID_PASSWORD_NO_LOWERCASE_LETTER, new Tuple<string, string>("The password does not contain a lowercase letter", "The password does not contain a lowercase letter") },
			{ ERROR_INVALID_PASSWORD_NO_UPPERCASE_LETTER, new Tuple<string, string>("The password does not contain an uppercase letter", "The password does not contain an uppercase letter") },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_DIGIT, new Tuple<string, string>("The password must contain one digit", "The password must contain one digit") },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_LETTER, new Tuple<string, string>("The password must contain one letter", "The password must contain one letter") },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_SPECIAL_CHARACTER, new Tuple<string, string>("The password must contain one of the designated special characters", "The password must contain one of the designated special characters") },
			{ ERROR_INVALID_PATRON_NUMBER, new Tuple<string, string>("The patron number entered was not found", "The patron number entered was not found") },
			{ ERROR_SAVING_LOGIN, new Tuple<string, string>("There was an error updating the password record", "There was an error updating the password record") },
			{ ERROR_INVALID_PIN, new Tuple<string, string>("The pin was invalid", "The pin was invalid") },
			{ ERROR_ERROR_VALIDATING_PATRON, new Tuple<string, string>("There was an error validating patron information", "There was an error validating patron information") },
			{ ERROR_REACHING_GATEWAY, new Tuple<string, string>("There was an error reaching the gateway service", "There was an error reaching the gateway service") },
			{ ERROR_INVALID_CHARACTER_PASSWORD, new Tuple<string, string>("There were invalid characters in the password field", "There were invalid characters in the password field") },
			{ ERROR_PATRON_NUMBER_LOCKEDOUT, new Tuple<string, string>("The patron number (and pin combo) is locked out", "The patron number (and pin combo) is locked out") },
			{ ERROR_MISSING_PATRON_NUMBER, new Tuple<string, string>("The patron number must be filled out", "The patron number must be filled out") },
			{ ERROR_PIN_MUST_BE_4_DIGITS, new Tuple<string, string>("The pin number must be 4 digits", "The pin number must be 4 digits") },
			{ ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error", "Generic Mobile Offers Service error") }
        };

		
		/// <summary>
		/// CHANGE PASSWORD
		/// </summary>
		/// <param name="dataModel"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[Route("save")]
		[ResponseType(typeof(IAuthenticatedUser))]
		public HttpResponseMessage PostPassword(UpdatePatronPassword dataModel)
		{
			if (ModelState.IsValid)
			{
				var MOC = new WinStarSoap.MobileOffersClient();

				int responseValue = MOC.resetPatronPassword(dataModel.UserName,
															dataModel.PatronNumber, 
															dataModel.Pin,
															ConfigurationManager.AppSettings["Facility"],
															HttpContext.Current.Request.UserHostAddress, 
															dataModel.NewPassword);

				if (responseValue > 0)
				{
					throw ThrowIfError(responseValue, HttpStatusCode.BadRequest, errors);
				}

				var winstarDataModel = new WinstarDataModel
				{
					UserName = dataModel.UserName,
					Password = dataModel.NewPassword
				};

				IUserModel currentUser = SecurityHelper.GetWinstarPatron(winstarDataModel);

				return Request.CreateResponse(HttpStatusCode.OK, currentUser.ToAuthenticatedUser(WinstarDataModel));
			}

			throw ThrowIfError(ERROR_GENERIC, HttpStatusCode.BadRequest, errors, ModelState);
		}
	}
}
