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
		
		public override string ToString()
		{
			var stringBuilder = new System.Text.StringBuilder();
			
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Identifier: \"" + identifier + "\"");
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
			
			bool metasAreEqual = true;
			if (meta.Count == other.meta.Count) {
				foreach (var kvp in meta) {
					if (other.meta[kvp.Key].ToString() == kvp.Value.ToString())
						continue;
				
					metasAreEqual = false;
					break;
				}
			} else {
				metasAreEqual = false;
			}
			
			return identifier == other.identifier
				&& content == other.content
				&& metasAreEqual
				&& modificationTime == other.modificationTime;
		}
	}
}
