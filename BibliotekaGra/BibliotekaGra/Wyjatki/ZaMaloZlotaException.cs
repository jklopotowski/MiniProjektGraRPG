using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyjatki
{
	public class ZaMaloZlotaException : Exception
	{
		public ZaMaloZlotaException() { }
		public ZaMaloZlotaException(string message) : base(message) { }
		public ZaMaloZlotaException(string message, Exception inner) : base(message, inner) { }
		protected ZaMaloZlotaException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
