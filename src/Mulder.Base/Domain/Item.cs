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
		readonly List<Item> children;
		readonly List<StaticFile> staticFiles;
		
		public string Identifier { get { return identifier; } }
		public bool IsBinary { get { return isBinary; } }
		public string Content { get { return content; } }
		public IDictionary<string, object> Meta { get { return meta; } }
		public DateTime ModificationTime { get { return modificationTime; } }
		public Item Parent { get; set; }
		public IEnumerable<Item> Children { get { return children; } }
		public IEnumerable<StaticFile> StaticFiles { get { return staticFiles; } }
		
		public Item(string identifier, bool isBinary, string content, IDictionary<string, object> meta, DateTime modificationTime)
		{
			this.identifier = identifier;
			this.isBinary = isBinary;
			this.content = content;
			this.meta = meta;
			this.modificationTime = modificationTime;
			
			children = new List<Item>();
			staticFiles = new List<StaticFile>();
		}
		
		public void AddStaticFile(StaticFile staticFile)
		{
			staticFiles.Add(staticFile);
		}
		
		public void AddChild(Item item)
		{
			children.Add(item);
		}
		
		public override string ToString()
		{
			var stringBuilder = new System.Text.StringBuilder();
			
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Identifier: \"" + identifier + "\"");
			stringBuilder.AppendLine("IsBinary: \"" + isBinary + "\"");
			stringBuilder.AppendLine("Content: \"" + content + "\"");
			stringBuilder.AppendLine("Meta:");
			foreach (var kvp in meta) {
				stringBuilder.AppendLine("  " + kvp.Key + ": \"" + kvp.Value.ToString() + "\"");
			}
			
			stringBuilder.AppendLine("ModificationTime: \"" + modificationTime + "\"");
			stringBuilder.AppendLine();
			
			return stringBuilder.ToString();
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
			
			bool metasAreEqual = true;
			if (meta.Count == other.meta.Count) {
				foreach (var kvp in meta) {
					if (other.meta[kvp.Key].ToString() == kvp.Value.ToString())
						continue;
				
					metasAreEqual = false;
					break;
				}
			}
			else {
				metasAreEqual = false;
			}
			
			return identifier == other.identifier
				&& isBinary == other.isBinary
				&& content == other.content
				&& metasAreEqual
				&& modificationTime == other.modificationTime;
		}
	}
}
