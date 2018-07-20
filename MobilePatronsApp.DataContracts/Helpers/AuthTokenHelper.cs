using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MobilePatronsApp.DataContracts.Implementations;
using MobilePatronsApp.DataContracts.Interfaces;

namespace MobilePatronsApp.DataContracts.Helpers
{
	public static class AuthTokenHelper
	{
		/*
		 * TO GENERATE KEYS:
		 *  var rsa = new RSACryptoServiceProvider(2048);
		 *  var publicPrivateKey = rsa.ToXmlString(true);
		 *  var publicKey = rsa.ToXmlString(false);
		 */
		private const char AuthorizationHeaderSeparator = ':';
		private const String PrivateKey = "<RSAKeyValue><Modulus>is7jq0wLePJnEefYNcDKUFr92TdA5uJyjUxMa7lxdhY6UDyyQEnKwyuUNS6f+MzN+UINwialcREXmv8+U6SuVAKJDdVX4oeOdZgl2cmiPz1LOpGU4jIey0cwiP4XaDr3AYExd/XKPpMuldeALB37SaJgfBAk2GydpgM2u0pwGQSyGqp7CKJwWrtKdtALEPxTQSOEyfLf+LD1jJSo+EJ7AKgC1gpavwL5vuwVF2Xdh33M0pamB8JU5+h3g+zE7sj8vpwhQw/nY39Jhc5N5dmqNUBVY2qnWNbUJyMgJ9oGq04akJACe6NMZhY0JKifrzCvsq/U79pkzru1bJLyDiX4tw==</Modulus><Exponent>AQAB</Exponent><P>wQs9SbGg7r8gczCQJqBSbgs4Q8WKTZv/SbZn3hhcKeBUrRATlr+CJreVMy+AG4I4bMbfmIldj/TOaXhlQf1HgHEN9g9+5tiEuvb+fItwV/H1Cq1Gdhvji77cYLZQ5e/XjIu/dbNqLj1QTglh4L/CDXcVoR9uf3oSB00rMR0paRM=</P><Q>uBOkJzpBrUANJfnBJF1uLxCz2ib2xujZ3Il9BD4Y69k7bcLhHl71XHPTCu6ZgWffrBsfbPTXGEHc82KeOudCAph8jCpP3tjWse+xDe1KKni8U7vL5ZCHkWvXJsI7QRj6LBr5ksJMrNqtEanmhoLHZq8oahy4gnsO1pIeHAGC6k0=</Q><DP>lKd2C6C4gAE4GNwFtWjx8QG80xt0VC1TmkzXkUSK/EhejGJ74zMYI35ta1wht41ArCs9FbZ6ERgAN2HZ/8Xt4K+ug5QNMfD7zQnUkM3DtkWBYDZssxjo0b0o8WSTyW03PGiFCDsgAfl/NIivEaY45auVQVz19z5mUa8QGqMNN3c=</DP><DQ>L4P6apnSHDudaUEYByAK8iE4m8ZVCzFOE2x2FeNFoZO4kHOukp1mbpADWR4QB+RdS1rQ+NoWr89IjpbVS1Uop+zesXu2lEqa2OGLbZHkBCSYUHD4h/CP3gzdQ8b9bqdY5IuuAqEfE7t600VtGSmm+S2bnBB/3EaRpPegyA/n0D0=</DQ><InverseQ>vT/vfIxWYN3R49Bs2zb3gAyeYKYEaPLY1R/vhRj6EkgrdfSXSuQg1Uhii+xBVHrTWK/1p/SL2wDdbcYrEX02GCWZcqLDeCwkzSPirra4bKrOyIC/RV8VqFGTWZSwEOoqgGWNtRETcjieuSAI3G29R7WAbx9k6u1ZONW8TCerTmM=</InverseQ><D>AIz5kkBm+z/a8NM+pFfzdYYm46m4dDIJyk23L+Y4cQN9j7vt6Rth1oxhtiRNNit51bdRXqfQmDY9JbxFfwd0OZkY+hkSlC99fTHxVH/cSOvxVHYnZQ5V2PCbXK/FYAB74mGk1JkBCGaFFMsaExK7YHtobpWRz9n+dh4lJXESKMc+fqE5jQulyAcz4N3P3UOrYVJq8g31hKwigmdgHlw2PqX1oa2ATra+7a7kN3D6AKXapOW1GVaajMl1RZ5HTuMZlIJRYzQC8jX6NhdlJxWsjr0qdavMAQ8cVrwKmwdwD7D2onrb2xIpEOpATYLYNnYjYldBOZlIlxxqJoFYSirtYQ==</D></RSAKeyValue>";
		private const String PublicKey = "<RSAKeyValue><Modulus>is7jq0wLePJnEefYNcDKUFr92TdA5uJyjUxMa7lxdhY6UDyyQEnKwyuUNS6f+MzN+UINwialcREXmv8+U6SuVAKJDdVX4oeOdZgl2cmiPz1LOpGU4jIey0cwiP4XaDr3AYExd/XKPpMuldeALB37SaJgfBAk2GydpgM2u0pwGQSyGqp7CKJwWrtKdtALEPxTQSOEyfLf+LD1jJSo+EJ7AKgC1gpavwL5vuwVF2Xdh33M0pamB8JU5+h3g+zE7sj8vpwhQw/nY39Jhc5N5dmqNUBVY2qnWNbUJyMgJ9oGq04akJACe6NMZhY0JKifrzCvsq/U79pkzru1bJLyDiX4tw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		private static readonly UnicodeEncoding Encoder = new UnicodeEncoding();

		public static String GenerateAuthToken(IWinstarDataModel userModel)
		{
			return Encrypt(userModel);
		}

		public static IWinstarDataModel UnPackAuthToken(String token, out DateTime timeStampCreated, out DateTime timeStampExpired)
		{
			var decryptedToken = Decrypt(token);
			var credentialBytes = Convert.FromBase64String(decryptedToken);
			var credentials = Encoding.ASCII.GetString(credentialBytes);
			var credentialParts = credentials.Split(AuthorizationHeaderSeparator);


			/*
			 * TOKENS SHOULD HAVE 6 PARTS:
			 * 0: USER NAME
			 * 1: PASSWORD
			 * 2: FACILITY
			 * 3: IP ADDRESS
			 * 4: TOKEN CREATED TIME STAMP
			 * 5: TOKEN EXPIRATION TIMESTAMP
			 */
			if (credentialParts.Length == 6)
			{
				IWinstarDataModel winstarDataModel = new WinstarDataModel();

				//0: USER NAME
				winstarDataModel.UserName = credentialParts[0].Trim();

				//1: PASSWORD
				winstarDataModel.Password = credentialParts[1].Trim();

				//2: FACILITY
				winstarDataModel.Facility = credentialParts[2].Trim();

				//3: IP ADDRESS
				winstarDataModel.IpAddress = credentialParts[3].Trim();

				//4: TOKEN CREATED TIME STAMP
				long ticks;
				long.TryParse(credentialParts[4].Trim(), out ticks);
				//TODO: DETERMINE AMOUNT OF TIME SINCE TOKEN WAS CREATED TO SEE IF IT'S STILL VALID.
				timeStampCreated = new DateTime(ticks);

				//5: TOKEN EXPIRATION TIMESTAMP
				long ticks2;
				long.TryParse(credentialParts[5].Trim(), out ticks2);
				//TODO: DETERMINE AMOUNT OF TIME SINCE TOKEN WAS CREATED TO SEE IF IT'S STILL VALID.
				timeStampExpired = new DateTime(ticks2);

				return winstarDataModel;
			}

			timeStampCreated = new DateTime();
			timeStampExpired = new DateTime();
			return null;
		}


		#region PRIVATE
		private static String Decrypt(String data)
		{
			var rsa = new RSACryptoServiceProvider();
			var dataArray = data.Split(new char[] { ',' });
			var dataByte = new byte[dataArray.Length];

			for (var i = 0; i < dataArray.Length; i++)
			{
				dataByte[i] = Convert.ToByte(dataArray[i]);
			}

			rsa.FromXmlString(PrivateKey);

			var decryptedByte = rsa.Decrypt(dataByte, false);

			return Encoder.GetString(decryptedByte);
		}

		private static String Encrypt(IWinstarDataModel userModel)
		{
			var tokenExpiresHours = int.Parse(ConfigurationManager.AppSettings["Max_Token_Expires_Hours"]);
			var tokenCreated = DateTime.Now.Ticks;
			var tokenExpiration = DateTime.Now.AddHours(tokenExpiresHours).Ticks;

			/*
			 * TOKEN IS COMPRISED OF THE FOLLOWING INFORMATION:
			 * 0: USER NAME
			 * 1: PASSWORD
			 * 2: FACILITY
			 * 3: IP ADDRESS
			 * 4: TOKEN CREATED TIME STAMP
			 * 5: TOKEN EXPIRATION TIMESTAMP
			 */
			var encodedValue = EncodeTo64(String.Format("{0}:{1}:{2}:{3}:{4}:{5}", userModel.UserName, userModel.Password, userModel.Facility, userModel.IpAddress, tokenCreated, tokenExpiration));

			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(PublicKey);

			var dataToEncrypt = Encoder.GetBytes(encodedValue);
			var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
			var length = encryptedByteArray.Count();
			var item = 0;
			var sb = new StringBuilder();

			foreach (var x in encryptedByteArray)
			{
				item++;
				sb.Append(x);

				if (item < length)
					sb.Append(",");
			}

			return sb.ToString();
		}

		private static String EncodeTo64(String toEncode)
		{
			var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
			return Convert.ToBase64String(toEncodeAsBytes);
		}
		#endregion PRIVATE
	}
}
