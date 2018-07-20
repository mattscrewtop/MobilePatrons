namespace MobilePatronsApp.Common.Enums
{
	public static class SecurityClaimTypes
	{
		public const string PersonId = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/personId";
		public const string UserKey = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/userKey";
		public const string AuthenticationId = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/authenticationId";
		public const string AuthenticationTypeId = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/authenticationTypeId";
		public const string AuthenticationTypeName = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/authenticationTypeName";
		public const string IsAnonymous = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/isAnonymous";
		public const string IsTemporaryPassword = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/isTemporaryPassword";
		public const string IsLockedOut = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/isLockedOut";
		public const string IsValidUser = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/isValidUser";
	}
}
