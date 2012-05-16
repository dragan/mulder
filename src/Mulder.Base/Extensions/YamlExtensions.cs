using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using YamlDotNet.RepresentationModel;
using YamlDotNet.RepresentationModel.Serialization;

namespace Mulder.Base.Extensions
{
	public static class YamlExtensions
	{
		static readonly Regex yamlHeaderRegex = new Regex(@"^---([\d\D\w\W\s\S]+)---", RegexOptions.Multiline);
		
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
		
		public static IList<object> DeserializeYaml(this string text)
		{
			var input = new StringReader(text);
			var results = new List<object>();
			
			var yaml = new YamlStream();
			yaml.Load(input);
			
			if (yaml.Documents.Count == 0)
				return results;
			
			for (int i = 0; i < yaml.Documents.Count; i++)
			{
				results.Add(GetValue(yaml.Documents[i].RootNode));
			}
			
			return results;
		}
		
		public static IDictionary<string, object> DeserializeYamlHeader(this string text)
		{
			MatchCollection matches = yamlHeaderRegex.Matches(text);
			if (matches.Count == 0)
				return new Dictionary<string, object>();
			
			string yaml = matches[0].Groups[1].Value;
			
			return yaml.DeserializeYaml()[0] as IDictionary<string, object>;
		}
		
		public static string ExcludeYamlHeader(this string text)
		{
			MatchCollection matches = yamlHeaderRegex.Matches(text);
			if (matches.Count == 0)
				return text;
			
			return text.Replace(matches[0].Groups[0].Value, "").Trim();
		}
		
		static object GetValue(YamlNode yamlNode)
		{
			if (yamlNode is YamlMappingNode)
				return GetMappingValue((YamlMappingNode)yamlNode);
			
			if (yamlNode is YamlSequenceNode)
				return GetSequenceValue((YamlSequenceNode)yamlNode);
			
			return yamlNode.ToString();
		}
		
		static IDictionary<string, object> GetMappingValue(YamlMappingNode mapping)
		{
			var results = new Dictionary<string, object>();
			foreach (var entry in mapping.Children)
			{
				var node = entry.Key as YamlScalarNode;
				if (node != null)
				{
					results.Add(node.Value, GetValue(entry.Value));
				}
			}
			
			return results;
		}
		
		static IList<object> GetSequenceValue(YamlSequenceNode sequence)
		{
			var results = new List<object>();
			foreach (var entry in sequence.Children)
			{
				results.Add(GetValue(entry));
			}
				
			return results;
		}
	}
}
