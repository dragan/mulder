using System;
using System.Collections.Generic;

namespace Mulder.Base.Domain
{
	public interface ISourceFile
	{
		string Identifier { get; }
		string Content { get; }
		dynamic Meta { get; }
		DateTime ModificationTime { get; }
	}
}
