using System;
using System.Collections.Generic;

using Mulder.Base.Domain;

namespace Mulder.Base.Domain
{{
	public class RuleProxy
	{{
		readonly IList<CompileRule> compileRules;
		readonly IList<RouteRule> routeRules;
		readonly IList<LayoutRule> layoutRules;
		
		public IEnumerable<CompileRule> CompileRules {{ get {{ return compileRules; }} }}
		public IEnumerable<RouteRule> RouteRules {{ get {{ return routeRules; }} }}
		public IEnumerable<LayoutRule> LayoutRules {{ get {{ return layoutRules; }} }}
		
		public RuleProxy()
		{{
			compileRules = new List<CompileRule>();
			routeRules = new List<RouteRule>();
			layoutRules = new List<LayoutRule>();
		}}
		
		public void LoadRules()
		{{
			{0}
		}}
		
		void Compile(string identifierPattern, Action<RuleContext> block)
		{{
			compileRules.Add(new CompileRule(identifierPattern, block));
		}}
		
		void Route(string identifierPattern, Func<RuleContext, string> block)
		{{
			routeRules.Add(new RouteRule(identifierPattern, block));
		}}
		
		void Layout(string identifierPattern, string filterName)
		{{
			layoutRules.Add(new LayoutRule(identifierPattern, filterName));
		}}
	}}
}}
