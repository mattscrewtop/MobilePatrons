using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MobilePatronsApp.Common.Exceptions
{
	[Serializable]
	public class CodeGenerationException : Exception
	{
		private readonly String errorMessage;
		private readonly IList<String> errorList = new List<String>();

		public override String Message
		{
			get
			{
				return this.errorMessage;
			}
		}

		public CodeGenerationException(String message, IList<String> errors)
		{
			this.errorMessage = message;
			this.errorList = errors;
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		// Constructor should be protected for unsealed classes, private for sealed classes.
		// (The Serializer invokes this constructor through reflection, so it can be private)
		protected CodeGenerationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.errorMessage = info.GetString("ErrorMessage");
			this.errorList = (IList<string>)info.GetValue("ErrorList", typeof(IList<string>));
		}

		public String ErrorMessage
		{
			get { return this.errorMessage; }
		}

		public IList<String> ErrorList
		{
			get { return this.errorList; }
		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("ErrorMessage", this.ErrorMessage);

			// Note: if "List<T>" isn't serializable you may need to work out another
			//       method of adding your list, this is just for show...
			info.AddValue("ErrorList", this.ErrorList, typeof(IList<string>));

			// MUST call through to the base class to let it save its own state
			base.GetObjectData(info, context);
		}
	}
}
