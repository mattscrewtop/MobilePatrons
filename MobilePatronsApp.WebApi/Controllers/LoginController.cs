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
	/// LOGIN AND AUTHENTICATE USER
	/// </summary>
	[RoutePrefix("api/login")]
	public class LoginController : SecureApiController
	{
		private const int ERROR_USER_LOCKED_OUT = 1;
		private const int ERROR_INVALID_PASSWORD = 2;
		private const int ERROR_PULLING_PATRON_NUMBER = 3;
		private const int ERROR_ACCOUNT_MERGED = 4;
        private const int ERROR_ACCOUNT_NOT_AUTHORIZED = 7;
        private const int ERROR_GENERIC = 20;
		private const int ERROR_INVALID_LOGIN = 25;

		readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
			{ ERROR_USER_LOCKED_OUT, new Tuple<string, string>("The user is locked out", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
			{ ERROR_INVALID_PASSWORD, new Tuple<string, string>("Invalid password", "You have entered an invalid Username or Password. Please try again.  Three failed attempts will result in this account being locked out for 24 hours.  If you forgot your Username or Password, you may visit any Players Club desk for further assistance.  If you would like to reset your password, you can use the 'Reset Password' link on the login page.") },
			{ ERROR_PULLING_PATRON_NUMBER, new Tuple<string, string>("Error pulling patron number", "Something went wrong. Please try again.") },
			{ ERROR_ACCOUNT_MERGED, new Tuple<string, string>("The account tied to this user name has been merged", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
            { ERROR_ACCOUNT_NOT_AUTHORIZED, new Tuple<string, string>("The account is not authorized yet", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
            { ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error when validating the user", "Something went wrong. Please try again.") },
			{ ERROR_INVALID_LOGIN, new Tuple<string, string>("Invalid login", "Something went wrong. Please try again.") }
        };

		private const int ERROR_RETRIEVE_LOGIN_ERROR_GATEWAY = 8;
		private const int ERROR_RETRIEVE_LOGIN_PATRON_NUMBER_NOT_FOUND = 11;
		private const int ERROR_RETRIEVE_LOGIN_INVALID_PIN = 13;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_VALIDATING_PATRON = 14;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_MISSING_PATRON_NUMBER = 18;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_PIN_NUMBER_LENGTH = 19;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_GENERIC = 20;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_PATRON_LOCKED_OUT = 21;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_DIGITS_ONLY = 22;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_MISSING_EMAIL = 23;
		private const int ERROR_RETRIEVE_LOGIN_ERROR_MISSING_LOGIN = 24;
		private const int ERROR_RETRIEVE_LOGIN_INVALID_MODEL = 25;

		readonly Dictionary<int, Tuple<String, String>> errorsLoginRetrieval = new Dictionary<int, Tuple<String, String>>
		{
			{ ERROR_RETRIEVE_LOGIN_ERROR_GATEWAY, new Tuple<string, string>("Gateway Service", "There was an error reaching the gateway service.") },
			{ ERROR_RETRIEVE_LOGIN_PATRON_NUMBER_NOT_FOUND, new Tuple<string, string>("Patron Number", "The patron number entered was not found.") },
			{ ERROR_RETRIEVE_LOGIN_INVALID_PIN, new Tuple<string, string>("Pin Number", "The pin was invalid.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_VALIDATING_PATRON, new Tuple<string, string>("Patron Information", "There was an error validating patron information.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_MISSING_PATRON_NUMBER, new Tuple<string, string>("Patron Number", "The patron number must be filled out.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_PIN_NUMBER_LENGTH, new Tuple<string, string>("Pin Number", "The pin number must be 4 digits.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_GENERIC, new Tuple<string, string>("Error", "Generic Mobile Offers Service error.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_PATRON_LOCKED_OUT, new Tuple<string, string>("Patron", "The patron is locked out.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_DIGITS_ONLY, new Tuple<string, string>("Patron Number", "The patron number must be digits only.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_MISSING_EMAIL, new Tuple<string, string>("Patron", "The patron did not have an email tied to their account.") },
			{ ERROR_RETRIEVE_LOGIN_ERROR_MISSING_LOGIN, new Tuple<string, string>("Patron", "The patron did not have a login tied to their account.") },
			{ ERROR_RETRIEVE_LOGIN_INVALID_MODEL, new Tuple<string, string>("Patron", "The data provided for login retrieval is invalid.") }
		};

		/// <summary>
		/// AUTHENTICATE USER
		/// </summary>
		/// <param name="loginModel"></param>
		/// <returns></returns>
		/// <exception cref="HttpResponseException"></exception>
		[AllowAnonymous]
		[Route("authenticate")]
		[ResponseType(typeof(IAuthenticatedUser))]
		public HttpResponseMessage Post(LoginModel loginModel)
		{
			if (ModelState.IsValid)
			{
				var MOC = new WinStarSoap.MobileOffersClient();

				var winstarDataModel = new WinstarDataModel
				{
					UserName = loginModel.UserName,
					Password = loginModel.Password
				};

				int responseValue = MOC.validateUser(
										winstarDataModel.UserName,
										winstarDataModel.Password,
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

			throw ThrowIfError(ERROR_INVALID_LOGIN, HttpStatusCode.BadRequest, errors, ModelState);
		}


		/// <summary>
		/// AUTHENTICATE USER
		/// </summary>
		/// <param name="loginModel"></param>
		/// <returns></returns>
		/// <exception cref="HttpResponseException"></exception>
		[AllowAnonymous]
		[Route("retrieval")]
		public HttpResponseMessage PostRetrieveLogin(LoginRetrievalModel loginRetrievalModel)
		{
			if (ModelState.IsValid)
			{
				var MOC = new WinStarSoap.MobileOffersClient();
				var Facility = ConfigurationManager.AppSettings["Facility"];
				var IpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";

				int responseValue = MOC.retrieveLogin(
										loginRetrievalModel.PatronNumber,
										loginRetrievalModel.Pin,
										Facility,
										IpAddress);

				//SUCCESS
				if (responseValue == 0)
				{
					return Request.CreateResponse(HttpStatusCode.OK);
				}

				throw ThrowIfError(responseValue, HttpStatusCode.BadRequest, errorsLoginRetrieval);
			}

			throw ThrowIfError(ERROR_RETRIEVE_LOGIN_INVALID_MODEL, HttpStatusCode.BadRequest, errorsLoginRetrieval, ModelState);
		}
	}
}
