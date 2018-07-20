using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MobilePatronsApp.DataContracts.Interfaces;


namespace MobilePatronsApp.WebApi.Controllers
{
	/// <summary>
	/// PATRON CONTROLLER
	/// </summary>
	[RoutePrefix("api/patron")]
	public class PatronController : SecureApiController
	{
		/// <summary>
		/// PATRON DETAIL
		/// </summary>
		/// <returns></returns>
		[Route("detail")]
		[ResponseType(typeof(IUserModel))]
		public HttpResponseMessage Get()
		{
			return Request.CreateResponse(HttpStatusCode.OK, CurrentUser);
		}
	}
}
