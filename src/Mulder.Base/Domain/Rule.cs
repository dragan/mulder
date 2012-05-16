using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mulder.Base.Domain
{
	public class Rule : IEquatable<Rule>
	{
		readonly Regex identifierRegex;
		
		public Rule(string identifierPattern)
		{
			this.identifierRegex = CreateIdentifierRegex(identifierPattern);
		}
		
		public bool ApplicableTo(ISourceFile sourceFile)
		{
			return identifierRegex.IsMatch(sourceFile.Identifier);
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + identifierRegex.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Rule) return Equals((Rule)obj);
			
			return false;
		}
		
		public bool Equals(Rule other)
		{
			if (other == null)
				return false;
			
			return identifierRegex == other.identifierRegex;
		}
		
		static Regex CreateIdentifierRegex(string identifierPattern)
		{
			string pattern = Regex.Escape(identifierPattern);
			pattern = pattern.StartsWith("/") ? pattern : "/" + pattern;
			
			// TODO: Not sure I like this.  Try to make easier to read
			// The logic is to add a "/" at the end of the pattern only if there isn't one already and the 
			// the last characters aren't "*" or "/"
			if (!pattern.EndsWith("/") && !(new [] { "*", "/" }.Contains(pattern.Substring(pattern.Length - 1))))
				pattern = pattern + "/";
			
			pattern = pattern.Replace(@"\*", "(.*?)").Replace(@"\+", "(.+?)");
			
			return new Regex('^' + pattern + '$', RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
		}
	}
}
