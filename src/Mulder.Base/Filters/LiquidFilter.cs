using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using DotLiquid;
using DotLiquid.FileSystems;

using Mulder.Base.Compilation;

namespace Mulder.Base.Filters
{
	public class LiquidFilter : IFilter
	{
		public LiquidFilter()
		{
		}
		
		public string Execute(string source, FilterContext filterContext)
		{
			string includesPath = GetIncludesPath(filterContext.Layout);

			var model = new {
				Configuration = filterContext.Configuration,
				Layout = new DynamicDrop(filterContext.Layout),
				Item = new DynamicDrop(filterContext.Item),
				Content = filterContext.Content
			};

			Hash hash = Hash.FromAnonymousObject(model);
			Template template = Template.Parse(source);
			Template.FileSystem = new Includes(includesPath);
			string output = template.Render(hash);
			
			return output;
		}
		
		string GetIncludesPath(IDictionary<string, object> layout)
		{
			string includesPath = string.Empty;

			if (layout != null) {
				if (layout.ContainsKey("includes_path"))
					includesPath = layout["includes_path"].ToString();
			}
			
			return includesPath;
		}
	}
	
	class Includes : IFileSystem
	{
		string root;
		
		public Includes(string root)
		{
			this.root = root;
		}
		
		public string ReadTemplateFile(Context context, string templateName)
		{
			string includePath = Path.Combine(root, templateName);
			
			if (File.Exists(includePath))
				return File.ReadAllText(includePath);
			
			return string.Empty;
		}
	}

	class DynamicDrop : Drop
	{
		readonly dynamic model;

		public DynamicDrop(dynamic model)
		{
			this.model = GetValidModelType(model);
		}

		public override object BeforeMethod(string propertyName)
		{
			if (model == null)
				return null;

			if (String.IsNullOrEmpty(propertyName))
				return null;

			Type modelType = this.model.GetType();

			object value = null;
			if (modelType.Equals(typeof(Dictionary<string, object>))) {
				value = GetExpandoObjectValue(propertyName);
			}
			else {
				value = GetPropertyValue(propertyName);
			}

			return value;
		}

		object GetExpandoObjectValue(string propertyName)
		{
			return (!model.ContainsKey(propertyName)) ? null : model[propertyName];
		}

		object GetPropertyValue(string propertyName)
		{
			var property = model.GetType().GetProperty(propertyName);

			return (property == null) ? null : property.GetValue(model, null);
		}

		static dynamic GetValidModelType(dynamic model)
		{
			if (model == null)
				return null;

			return model.GetType().Equals(typeof(ExpandoObject))
				? new Dictionary<string, object>(model, StringComparer.InvariantCultureIgnoreCase)
				: model;
		}
	}
}
