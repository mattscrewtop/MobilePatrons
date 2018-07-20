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
	[RoutePrefix("api/account")]
	public class AccountController : SecureApiController
	{
		private const int ERROR_INVALID_LOGIN_LENGTH = 1;
		private const int ERROR_INVALID_PASSWORD_LENGTH = 2;
		private const int ERROR_INVALID_PASSWORD_NO_LOWERCASE_LETTER = 3;
		private const int ERROR_INVALID_PASSWORD_NO_UPPERCASE_LETTER = 4;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_DIGIT = 5;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_LETTER = 6;
		private const int ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_SPECIAL_CHARACTER = 7;
		private const int ERROR_REACHING_GATEWAY = 8;
		private const int ERROR_PATRON_HAS_EXISTING_LOGIN = 9;
		private const int ERROR_USERNAME_ALREADY_USED = 10;
		private const int ERROR_INVALID_PATRON_NUMBER = 11;
		private const int ERROR_SAVING_LOGIN = 12;
		private const int ERROR_INVALID_PIN = 13;
		private const int ERROR_ERROR_VALIDATING_PATRON = 14;
		private const int ERROR_INVALID_CHARACTER_LOGIN = 15;
		private const int ERROR_INVALID_CHARACTER_PASSWORD = 16;
		private const int ERROR_PATRON_NUMBER_LOCKEDOUT = 17;
		private const int ERROR_MISSING_PATRON_NUMBER = 18;
		private const int ERROR_PIN_MUST_BE_4_DIGITS = 19;
		private const int ERROR_GENERIC = 20;
		private const int ERROR_PATRON_LOCKEDOUT = 21;
		private const int ERROR_ACCOUNT_NUMBER_DIGITS_ONLY = 22;

		private const String invalidPasswordFormatMessage = @"Your Password must be between 8 and 32 charcters in length, contain at least one uppoercase letter, one lowercase letter, one numerical digit and one special character (!@#$%^&*()_\"") + \""].*). Please try again.";

		readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
			{ ERROR_INVALID_LOGIN_LENGTH, new Tuple<string, string>("The login length is improper", "Your Username must be between 8 and 50 characters in length. Please try again.")},
			{ ERROR_INVALID_PASSWORD_LENGTH, new Tuple<string, string>("The password length is improper", invalidPasswordFormatMessage) },
			{ ERROR_INVALID_PASSWORD_NO_LOWERCASE_LETTER, new Tuple<string, string>("The password does not contain a lowercase letter", invalidPasswordFormatMessage) },
			{ ERROR_INVALID_PASSWORD_NO_UPPERCASE_LETTER, new Tuple<string, string>("The password does not contain an uppercase letter", invalidPasswordFormatMessage) },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_DIGIT, new Tuple<string, string>("The password must contain one digit", invalidPasswordFormatMessage) },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_LETTER, new Tuple<string, string>("The password must contain one letter", invalidPasswordFormatMessage) },
			{ ERROR_INVALID_PASSWORD_REQUIRES_AT_LEAST_ONE_SPECIAL_CHARACTER, new Tuple<string, string>("The password must contain one of the designated special characters", invalidPasswordFormatMessage) },
			{ ERROR_REACHING_GATEWAY, new Tuple<string, string>("There was an error reaching the gateway service", "Something went wrong. Please try again.") },
			{ ERROR_PATRON_HAS_EXISTING_LOGIN, new Tuple<string, string>("The patron number already has a login tied to it", "There is already an login for this account. Please check the account number and try again. If you forgot your Username, you may visit any Players Club desk for further assistance.  If you forgot your Password, you may visit any Players Club desk." ) },
			{ ERROR_USERNAME_ALREADY_USED, new Tuple<string, string>("The user name is already tied to another patron", "Sorry! That Username is already taken. Please try again.") },
			{ ERROR_INVALID_PATRON_NUMBER, new Tuple<string, string>("The patron number entered was not found", "You have entered an invalid Account Number or PIN. Please try again.  Three failed attempts will result in this account being locked out for 24 hours.  If you forgot your Account Number or PIN, you may visit any Players Club desk for further assistance.") },
			{ ERROR_SAVING_LOGIN, new Tuple<string, string>("There was an error inserting the patron login", "Oops! Something happened. Please try again.") },
			{ ERROR_INVALID_PIN, new Tuple<string, string>("The pin was invalid", "You have entered an invalid Account Number or PIN. Please try again.  Three failed attempts will result in this account being locked out for 24 hours.  If you forgot your Account Number or PIN, you may visit any Players Club desk for further assistance.") },
			{ ERROR_ERROR_VALIDATING_PATRON, new Tuple<string, string>("There was an error validating patron information", "Something went wrong. Please try again.") },
			{ ERROR_INVALID_CHARACTER_LOGIN, new Tuple<string, string>("There were invalid characters in the login field", "Your Username must be between 8 and 50 characters in length. Please try again.") },
			{ ERROR_INVALID_CHARACTER_PASSWORD, new Tuple<string, string>("There were invalid characters in the password field", invalidPasswordFormatMessage) },
			{ ERROR_PATRON_NUMBER_LOCKEDOUT, new Tuple<string, string>("The patron number is locked out", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
			{ ERROR_MISSING_PATRON_NUMBER, new Tuple<string, string>("The patron number must be filled out", "The patron number must be filled out") },
			{ ERROR_PIN_MUST_BE_4_DIGITS, new Tuple<string, string>("The pin must be 4 digits", "The pin must be 4 digits") },
			{ ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error", "Generic Mobile Offers Service error") },
			{ ERROR_PATRON_LOCKEDOUT, new Tuple<string, string>("The patron is locked out", "The patron is locked out") },
			{ ERROR_ACCOUNT_NUMBER_DIGITS_ONLY, new Tuple<string, string>("Account number format is incorrect", "Your account number should consist of digits only; it should not include any letters, spaces or special characters. Please refer to the diagram above and try again.") }
        };

		

		/// <summary>
		/// REGISTERS A USER
		/// </summary>
		/// <param name="dataModel"></param>
		/// <returns>IAuthenticatedUser</returns>
		[AllowAnonymous]
		[Route("register")]
		[ResponseType(typeof(IAuthenticatedUser))]
		public HttpResponseMessage Post(RegistrationModel dataModel)
		{
			if (ModelState.IsValid)
			{
				var MOC = new WinStarSoap.MobileOffersClient();

				var winstarDataModel = new WinstarDataModel
				{
					UserName = dataModel.UserName,
					Password = dataModel.Password
				};

				int responseValue = MOC.createAccount(
										winstarDataModel.UserName,
										winstarDataModel.Password,
										dataModel.PatronNumber,
										dataModel.Pin,
										winstarDataModel.Facility,
										winstarDataModel.IpAddress);

				//SUCCESS
				if (responseValue == 0)
				{
					IUserModel currentUser = SecurityHelper.GetWinstarPatron(winstarDataModel);
					return Request.CreateResponse(HttpStatusCode.OK, currentUser.ToAuthenticatedUser(winstarDataModel));
				}

				throw ThrowIfError(responseValue, HttpStatusCode.BadRequest, errors);
			}

			throw ThrowIfError(ERROR_GENERIC, HttpStatusCode.BadRequest, errors, ModelState);
		}
	}
}
