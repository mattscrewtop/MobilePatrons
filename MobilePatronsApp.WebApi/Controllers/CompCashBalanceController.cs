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
    [RoutePrefix("api/comp-cash")]
    public class CompCashBalanceController : SecureApiController
    {
        private const int ERROR_USER_LOCKED_OUT = 1;
        private const int ERROR_INVALID_PASSWORD = 2;
        private const int ERROR_PULLING_PATRON_NUMBER = 3;
        private const int ERROR_ACCOUNT_MERGED = 4;
        private const int RROR_ERROR_VALIDATING_PATRON = 14;
        private const int ERROR_GENERIC = 20;

        readonly Dictionary<int, Tuple<String, String>> errors = new Dictionary<int, Tuple<String, String>>
        {
            { ERROR_USER_LOCKED_OUT, new Tuple<string, string>("The user is locked out", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
            { ERROR_INVALID_PASSWORD, new Tuple<string, string>("Invalid password", "Invlaid password.") },
            { ERROR_PULLING_PATRON_NUMBER, new Tuple<string, string>("Error pulling patron number", "Something went wrong. Please try again.") },
            { ERROR_ACCOUNT_MERGED, new Tuple<string, string>("The account tied to this user name has been merged", "We weren't able to access your account. Please visit any Players Club desk for further assistance.") },
            { RROR_ERROR_VALIDATING_PATRON, new Tuple<string, string>("There was an error validating patron information", "There was an error validating patron information") },
            { ERROR_GENERIC, new Tuple<string, string>("Generic Mobile Offers Service error when validating the user", "Something went wrong. Please try again.") }
        };

        /// <summary>
        /// LIST OF COMP CASH BALANCE
        /// </summary>
        /// <returns></returns>
        [Route("comp-cash-balance")]
        [ResponseType(typeof(CompCashBalanceStatement))]
        public HttpResponseMessage Get()
        {
            var MOC = new WinStarSoap.MobileOffersClient();
            var compCashBalanceValue = MOC.getCompCashBalance(WinstarDataModel.UserName, WinstarDataModel.Password, WinstarDataModel.Facility, WinstarDataModel.IpAddress);

            ICompCashBalanceStatement compCashBalanceStatement = new CompCashBalanceStatement();
            compCashBalanceStatement.CompCashBalanceValue = compCashBalanceValue;

            if (compCashBalanceValue.IndexOf("error") == -1) {
                return Request.CreateResponse(HttpStatusCode.OK, compCashBalanceStatement);
            } else
            {
                Int32 errorId;
                Int32.TryParse(compCashBalanceValue.Replace("error",""), out errorId);
                throw ThrowIfError(errorId, HttpStatusCode.BadRequest, errors);
            }
        }
    }
}
