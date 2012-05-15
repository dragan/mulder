using System;
using System.Collections.Generic;

namespace Mulder.Base.Domain
{
	public class Item : ISourceFile, IEquatable<Item>
	{
		readonly string identifier;
		readonly bool isBinary;
		readonly string content;
		readonly IDictionary<string, object> meta;
		readonly DateTime modificationTime;
		readonly List<StaticFile> staticFiles;
		
		public string Identifier { get { return identifier; } }
		public bool IsBinary { get { return isBinary; } }
		public string Content { get { return content; } }
		public IDictionary<string, object> Meta { get { return meta; } }
		public DateTime ModificationTime { get { return modificationTime; } }
		public IEnumerable<StaticFile> StaticFiles { get { return staticFiles; } }
		
		public Item(string identifier, bool isBinary, string content, IDictionary<string, object> meta, DateTime modificationTime)
		{
			this.identifier = identifier;
			this.isBinary = isBinary;
			this.content = content;
			this.meta = meta;
			this.modificationTime = modificationTime;
			
			staticFiles = new List<StaticFile>();
		}
		
		public void AddStaticFile(StaticFile staticFile)
		{
			staticFiles.Add(staticFile);
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + identifier.GetHashCode();
			hash = hash * 23 + isBinary.GetHashCode();
			hash = hash * 23 + content.GetHashCode();
			hash = hash * 23 + meta.GetHashCode();
			hash = hash * 23 + modificationTime.GetHashCode();
			hash = hash * 23 + staticFiles.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Item) return Equals((Item)obj);
			
			return false;
		}
		
		public bool Equals(Item other)
		{
			if (other == null)
				return false;
			
			return identifier == other.identifier
				&& isBinary == other.isBinary
				&& content == other.content
				&& meta == other.meta
				&& modificationTime == other.modificationTime
				&& staticFiles == other.staticFiles;
		}
	}
}
