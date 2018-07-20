using System.Web.Http;
using MobilePatronsApp.WebApi.Handlers;
using SeedApp.WebApi.Handlers;
using StructureMap;

namespace MobilePatronsApp.WebApi
{
	/// <summary>
	/// HANDLES X-AUTHENTICATION
	/// </summary>
	public class MessageHandlerConfig
	{
		/// <summary>
		/// CONFIGURES VARIOUS HANDLERS TO FIRE PRE-REQUEST
		/// </summary>
		/// <param name="config"></param>
		public static void RegisterMessageHandler(HttpConfiguration config)
		{
			/* HANDLE CORS AND PRE-FLIGHT REQUESTS*/
			var corsHandler = ObjectFactory.GetInstance<CorsHandler>();
			config.MessageHandlers.Add(corsHandler);

			/* AUTHORIZATION MESSAGE HANDLER */
			var authorizationHeaderHandler = ObjectFactory.GetInstance<XMobilePatronsAppAuthMessageHandler>();
			config.MessageHandlers.Add(authorizationHeaderHandler);
		}
	}
}