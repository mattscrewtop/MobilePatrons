using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MobilePatronsApp.Common.Helpers
{
	public static class PasswordHelper
	{
		const int DefaultPasswordLength = 30;

		public static String CreateRandomPassword()
		{
			return CreateRandomPassword(DefaultPasswordLength);
		}

		public static String CreateRandomPassword(int passwordLength)
		{
			/*
			define allowable character explicitly - easy to read this way an easy to 
			omit easily confused chars like l (ell) and 1 (one) or 0 (zero) and O (oh)
			*/
			const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ23456789";
			var randomBytes = new Byte[passwordLength];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);
			var chars = new char[passwordLength];
			var allowedCharCount = allowedChars.Length;

			for (var i = 0; i < passwordLength; i++)
			{
				chars[i] = allowedChars[randomBytes[i] % allowedCharCount];
			}

			return new string(chars);
		}

		public static String ToSHA512Hash(string password, out Int64 salt)
		{
			salt = CreateRandomSalt();
			return ToSHA512Hash(password, salt);
		}

		public static String ToSHA512Hash(string password, Int64 salt)
		{
			var paswordSalt = String.Format("{0}{1}", password, salt);
			return ToSHA512Hash(paswordSalt);
		}

		public static String ToSHA512Hash(string password)
		{
			var data = Encoding.ASCII.GetBytes(password);
			var hashData = SHA512.Create().ComputeHash(data);
			var hashedValue = hashData.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
			return hashedValue;
		}

		public static Int64 CreateRandomSalt()
		{
			var saltBytes = new Byte[4];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(saltBytes);

			return ((saltBytes[0] << 24) + (saltBytes[1] << 16) + (saltBytes[2] << 8) + saltBytes[3]);
		}
	}
}
