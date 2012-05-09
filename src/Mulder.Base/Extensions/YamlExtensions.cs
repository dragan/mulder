using System;
using System.Collections.Generic;
using System.IO;

namespace Mulder.Base.Extensions
{
	public static class YamlExtensions
	{
		public static string ToYaml(this IDictionary<string, object> dictionary)
		{
			var stringWriter = new StringWriter();
			
			foreach (var item in dictionary) {
				stringWriter.WriteLine(item.Key.ToLower() + ": " + item.Value);
			}
			
			return stringWriter.ToString();
		}
		
		public static string ToYamlHeader(this IDictionary<string, object> dictionary)
		{
			var stringWriter = new StringWriter();
			
			stringWriter.WriteLine("---");
			stringWriter.Write(dictionary.ToYaml());
			stringWriter.WriteLine("---");
			
			return stringWriter.ToString();
		}
	}
}
