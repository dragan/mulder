using System;
using System.Collections.Generic;

namespace Mulder.Base.Domain
{
	public class Layout : ISourceFile, IEquatable<Layout>
	{
		readonly string identifier;
		readonly string content;
		readonly IDictionary<string, object> meta;
		readonly DateTime modificationTime;
		
		public string Identifier { get { return identifier; } }
		public string Content { get { return content; } }
		public IDictionary<string, object> Meta { get { return meta; } }
		public DateTime ModificationTime { get { return modificationTime; } }
		
		public Layout(string identifier, string content, IDictionary<string, object> meta, DateTime modificationTime)
		{
			this.identifier = identifier;
			this.content = content;
			this.meta = meta;
			this.modificationTime = modificationTime;
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + identifier.GetHashCode();
			hash = hash * 23 + content.GetHashCode();
			hash = hash * 23 + meta.GetHashCode();
			hash = hash * 23 + modificationTime.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Layout) return Equals((Layout)obj);
			
			return false;
		}
		
		public bool Equals(Layout other)
		{
			if (other == null)
				return false;
			
			return identifier == other.identifier
				&& content == other.content
				&& meta == other.meta
				&& modificationTime == other.modificationTime;
		}
	}
}
