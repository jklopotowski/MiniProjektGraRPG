using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyjatki
{
	public class BrakEnergiiException : Exception
	{
		public BrakEnergiiException() { }
		public BrakEnergiiException(string message) : base(message) { }
		public BrakEnergiiException(string message, Exception inner) : base(message, inner) { }
		protected BrakEnergiiException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
