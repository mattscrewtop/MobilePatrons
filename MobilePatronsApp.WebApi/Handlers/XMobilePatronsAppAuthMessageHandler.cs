using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MobilePatronsApp.Common.Enums;
using MobilePatronsApp.DataContracts.Helpers;
using MobilePatronsApp.DataContracts.Interfaces;
using MobilePatronsApp.WebApi.Helpers;
using StructureMap;


namespace SeedApp.WebApi.Handlers
{
	/// <summary>
	/// HANDLES X-POPS-AUTH TOKEN IF IT'S PRESENT IN HTTP REQUEST
	/// </summary>
	public class XMobilePatronsAppAuthMessageHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync
		(
			HttpRequestMessage request,
			CancellationToken cancellationToken
		)
		{
			IEnumerable<string> authorizationValue;
			var hasAutorization = request.Headers.TryGetValues("X-PATRONS-AUTH-TOKEN", out authorizationValue);

			if (hasAutorization)
			{
				var token = authorizationValue.First();

				/*-----------------------------------------------------------------------------------------
				 INSPECT TOKEN TO SEE IF USER EXISTS AND TOKEN HAS NOT EXPIRED
				 -----------------------------------------------------------------------------------------*/
				DateTime timeStampTokenCreated;
				DateTime timeStampTokenExpires;

				IWinstarDataModel winstarDataModel = AuthTokenHelper.UnPackAuthToken(token, out timeStampTokenCreated, out timeStampTokenExpires);

				//IF TOKEN EXPIRES TIMESTAMP IS LESS THAN NOW, ABORT
				if (timeStampTokenExpires < DateTime.Now)
				{
					return CreateUnauthorizedResponse("Authorization has expired.  Please login again.");
				}

				//GET USER
				IUserModel currentUser = SecurityHelper.GetWinstarPatron(winstarDataModel);

				//IF USER IS UNKNOWN, THEN ABORT
				if (currentUser == null)
				{
					return CreateUnauthorizedResponse("User is unknown.");
				}

				//CAPTURE DATA FOR USE IN CONTROLLERS (SEE SecureApiController)
				ObjectFactory.Configure(x => x.For<IWinstarDataModel>().Singleton().Use(winstarDataModel));
				ObjectFactory.Configure(x => x.For<IUserModel>().Singleton().Use(currentUser));

				//CONVERT CURRENT USER TO AN AUTHENTICATED USER
				Thread.CurrentPrincipal = currentUser.ToClaimsPrincipal();

				//SET HTTP CONTEXT CURRENT USER TO AUTHENTICATED USER
				if (HttpContext.Current != null)
				{
					HttpContext.Current.User = Thread.CurrentPrincipal;
				}
			}

			return base.SendAsync(request, cancellationToken)
			   .ContinueWith(task =>
			   {
				   return task.Result;
			   });
		}


		private static Task<HttpResponseMessage> CreateUnauthorizedResponse(string responseMessage)
		{
			var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
			{
				Content = new StringContent(responseMessage),
				ReasonPhrase = "Exception"
			};

			var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
			taskCompletionSource.SetResult(response);
			return taskCompletionSource.Task;
		}
	}
}