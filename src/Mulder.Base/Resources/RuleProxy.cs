using System;
using System.Collections.Generic;

using Mulder.Base.Domain;
using Mulder.Base.Extensions;

namespace Mulder.Base.Domain
{{
	public class RuleProxy
	{{
		readonly Queue<CompileRule> compileRules;
		readonly Queue<RouteRule> routeRules;
		readonly Queue<LayoutRule> layoutRules;
		
		public IEnumerable<CompileRule> CompileRules {{ get {{ return compileRules; }} }}
		public IEnumerable<RouteRule> RouteRules {{ get {{ return routeRules; }} }}
		public IEnumerable<LayoutRule> LayoutRules {{ get {{ return layoutRules; }} }}
		
		public RuleProxy()
		{{
			compileRules = new Queue<CompileRule>();
			routeRules = new Queue<RouteRule>();
			layoutRules = new Queue<LayoutRule>();
		}}
		
		public void LoadRules()
		{{
			{0}
		}}
		
		void Compile(string identifierPattern, Action<RuleContext> block)
		{{
			compileRules.Enqueue(new CompileRule(identifierPattern, block));
		}}
		
		void Route(string identifierPattern, Func<RuleContext, string> block)
		{{
			routeRules.Enqueue(new RouteRule(identifierPattern, block));
		}}
		
		void Layout(string identifierPattern, string filterName)
		{{
			layoutRules.Enqueue(new LayoutRule(identifierPattern, filterName));
		}}
	}}
}}
