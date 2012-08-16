using System;
using System.Collections.Generic;

namespace Mulder.Base.Domain
{
	public interface ISourceFile
	{
		string Identifier { get; }
		string Content { get; }
		IDictionary<string, object> Meta { get; }
		DateTime ModificationTime { get; }
	}
}
