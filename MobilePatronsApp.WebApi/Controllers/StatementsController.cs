using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MobilePatronsApp.DataContracts.Implementations;
using MobilePatronsApp.DataContracts.Interfaces;


namespace MobilePatronsApp.WebApi.Controllers
{
	/// <summary>
	/// PATRON CONTROLLER
	/// </summary>
	[RoutePrefix("api/statements")]
	public class StatementsController : SecureApiController
	{
		private const int ERROR_USER_LOCKED_OUT = 1;
		private const int ERROR_INVALID_PASSWORD = 2;
		private const int ERROR_PULLING_PATRON_NUMBER = 3;
		private const int ERROR_ACCOUNT_MERGED = 4;
		private const int ERROR_NO_DATA_AVAILABLE = 5;
		private const int ERROR_GENERIC = 20;

		readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
			{ ERROR_USER_LOCKED_OUT, new Tuple<string, string>("The user is locked out", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
			{ ERROR_INVALID_PASSWORD, new Tuple<string, string>("Invalid password", "Invlaid password.") },
			{ ERROR_PULLING_PATRON_NUMBER, new Tuple<string, string>("Error pulling patron number", "Something went wrong. Please try again.") },
			{ ERROR_ACCOUNT_MERGED, new Tuple<string, string>("The account tied to this user name has been merged", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
			{ ERROR_NO_DATA_AVAILABLE, new Tuple<string, string>("No win/loss statement available for the year", "No win/loss statement available for the year") },
			{ ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error when validating the user", "Something went wrong. Please try again.") }
        };


		/// <summary>
		/// WIN-LOSS STATEMENT FOR A GIVEN YEAR
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		[Route("win-loss/{year:int}")]
		[ResponseType(typeof(WinLossStatement))]
		public HttpResponseMessage Get(int year)
		{
			var MOC = new WinStarSoap.MobileOffersClient();
			var winLossValue = MOC.getWinLoss(WinstarDataModel.UserName, WinstarDataModel.Password, WinstarDataModel.Facility, year.ToString(), WinstarDataModel.IpAddress);

			IWinLossStatement winLossStatement = new WinLossStatement();
			winLossStatement.Year = year;
			winLossStatement.WinLossValue = winLossValue;

			return Request.CreateResponse(HttpStatusCode.OK, winLossStatement);
        }
    }
}
