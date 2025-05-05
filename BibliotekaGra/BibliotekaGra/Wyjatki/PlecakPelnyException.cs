using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyjatki
{

	public class PlecakPelnyException : Exception
	{
		public PlecakPelnyException() { }
		public PlecakPelnyException(string message) : base(message) { }
		public PlecakPelnyException(string message, Exception inner) : base(message, inner) { }
		protected PlecakPelnyException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
