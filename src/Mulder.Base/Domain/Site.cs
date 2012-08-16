using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mulder.Base.Domain
{
	public class Site : IEquatable<Site>
	{
		readonly IConfiguration configuration;
		readonly IEnumerable<Item> items;
		readonly IEnumerable<Layout> layouts;
		readonly IEnumerable<CompileRule> compileRules;
		readonly IEnumerable<RouteRule> routeRules;
		readonly IEnumerable<LayoutRule> layoutRules;
		
		public IConfiguration Configuration { get { return configuration; } }
		public IEnumerable<Item> Items { get { return items; } }
		public IEnumerable<Layout> Layouts { get { return layouts; } }
		public IEnumerable<CompileRule> CompileRules { get { return compileRules; } }
		public IEnumerable<RouteRule> RouteRules { get { return routeRules; } }
		public IEnumerable<LayoutRule> LayoutRules { get { return layoutRules; } }
		
		public Site(IConfiguration configuration, IEnumerable<Item> items, IEnumerable<Layout> layouts, IEnumerable<CompileRule> compileRules, IEnumerable<RouteRule> routeRules, IEnumerable<LayoutRule> layoutRules)
		{
			this.configuration = configuration;
			this.items = items;
			this.layouts = layouts;
			this.compileRules = compileRules;
			this.routeRules = routeRules;
			this.layoutRules = layoutRules;
			
			CreateParentChildLinks();
		}
		
		public CompileRule GetCompilationRuleFor(Item item)
		{
			return compileRules.Where(rule => rule.ApplicableTo(item)).First();
		}
		
		public RouteRule GetRoutingRuleFor(Item item)
		{
			return routeRules.Where(rule => rule.ApplicableTo(item)).First();
		}
		
		public LayoutRule GetLayoutRuleFor(Layout layout)
		{
			return layoutRules.Where(rule => rule.ApplicableTo(layout)).First();
		}
		
		public Layout GetLayoutWithIdentifier(string layoutIdentifier)
		{
			return layouts.Where(layout => layout.Identifier == layoutIdentifier).SingleOrDefault();
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + configuration.GetHashCode();
			hash = hash * 23 + items.GetHashCode();
			hash = hash * 23 + layouts.GetHashCode();
			hash = hash * 23 + compileRules.GetHashCode();
			hash = hash * 23 + routeRules.GetHashCode();
			hash = hash * 23 + layoutRules.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Site) return Equals((Site)obj);
			
			return false;
		}
		
		public bool Equals(Site other)
		{
			if (other == null)
				return false;
			
			return configuration == other.configuration
				&& items == other.items
				&& layouts == other.layouts
				&& compileRules == other.compileRules
				&& routeRules == other.routeRules
				&& layoutRules == other.layoutRules;
		}
		
		void CreateParentChildLinks()
		{
			var stripLastPathRegex = new Regex("[^/]+/$");
			
			var sortedItems = items.OrderBy(i => i.Identifier).ToList();
			
			foreach (Item item in sortedItems) {
				string parentIdentifier = stripLastPathRegex.Replace(item.Identifier, "");
				Item parent = null;
				
				foreach (Item candidate in items) {
					if (candidate.Identifier != parentIdentifier) {
						continue;
					}
					else if (candidate.Identifier == item.Identifier) {
						break;
					}
					
					parent = candidate;
				}
				
				if (parent != null) {
					item.Parent = parent;
					parent.AddChild(item);
				}
			}
		}
	}
}
