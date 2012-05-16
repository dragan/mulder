using System;
using System.Collections.Generic;
using System.Linq;

namespace Mulder.Base.Domain
{
	public class Site : IEquatable<Site>
	{
		readonly IDictionary<string, object> configuration;
		readonly IEnumerable<Item> items;
		readonly IEnumerable<Layout> layouts;
		readonly IEnumerable<CompileRule> compileRules;
		readonly IEnumerable<RouteRule> routeRules;
		readonly IEnumerable<LayoutRule> layoutRules;
		
		public IDictionary<string, object> Configuration { get { return configuration; } }
		public IEnumerable<Item> Items { get { return items; } }
		public IEnumerable<Layout> Layouts { get { return layouts; } }
		public IEnumerable<CompileRule> CompileRules { get { return compileRules; } }
		public IEnumerable<RouteRule> RouteRules { get { return routeRules; } }
		public IEnumerable<LayoutRule> LayoutRules { get { return layoutRules; } }
		
		public Site(IDictionary<string, object> configuration, IEnumerable<Item> items, IEnumerable<Layout> layouts, IEnumerable<CompileRule> compileRules, IEnumerable<RouteRule> routeRules, IEnumerable<LayoutRule> layoutRules)
		{
			this.configuration = configuration;
			this.items = items;
			this.layouts = layouts;
			this.compileRules = compileRules;
			this.routeRules = routeRules;
			this.layoutRules = layoutRules;
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
	}
}
