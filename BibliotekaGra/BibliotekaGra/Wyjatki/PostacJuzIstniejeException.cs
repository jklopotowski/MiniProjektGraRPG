using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyjatki
{

	public class PostacJuzIstniejeException : Exception
	{
		public PostacJuzIstniejeException() { }
		public PostacJuzIstniejeException(string message) : base(message) { }
		public PostacJuzIstniejeException(string message, Exception inner) : base(message, inner) { }
		protected PostacJuzIstniejeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
