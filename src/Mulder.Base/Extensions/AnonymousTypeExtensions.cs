using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Mulder.Base.Extensions
{
	public static class AnonymousTypeExtensions
	{
		public static IDictionary<string, object> ToDictionary(this object anonymousType)
		{
			return AnonymousTypeConverter.ConvertToDictionary(anonymousType);
		}
		
		class AnonymousTypeConverter
		{
			static readonly Dictionary<Type, Func<object, IDictionary<string, object>>> cache 
				= new Dictionary<Type, Func<object, IDictionary<string, object>>>();
			
			public static IDictionary<string, object> ConvertToDictionary(object model)
			{
				if (model == null)
					return new Dictionary<string, object>();
				
				if (model is IDictionary<string, object>)
					return (IDictionary<string, object>)model;
			
				return GetDictionaryConverter(model)(model);
			}
			
			static Func<object, IDictionary<string, object>> GetDictionaryConverter(object item)
			{
				Func<object, IDictionary<string, object>> function;
				if (!cache.TryGetValue(item.GetType(), out function)) {
					function = CreateDictionaryConverter(item.GetType());
					cache[item.GetType()] = function;
				}
				
				return function;
			}
			
			static Func<object, IDictionary<string, object>> CreateDictionaryConverter(Type type)
			{
				Type dictionaryType = typeof(Dictionary<string, object>);
				
				// Setup dynamic method and make type owner of the method
				var dynamicMethod = new DynamicMethod(string.Empty, typeof(IDictionary<string, object>), new[] { typeof(object) }, type);
				var il = dynamicMethod.GetILGenerator();
				
				// Dictionary.Add(object key, object value);
				var addMethod = dictionaryType.GetMethod("Add");
				
				// Create a dictionary and store it in a local variable
				il.DeclareLocal(dictionaryType);
				il.Emit(OpCodes.Newobj, dictionaryType.GetConstructor(Type.EmptyTypes));
				il.Emit(OpCodes.Stloc_0);
				
				var attributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
				foreach (var property in type.GetProperties(attributes).Where(info => info.CanRead)) {
					// Load Dictionary, prepare for call later
					il.Emit(OpCodes.Ldloc_0);
					
					// Load key, name of property
					il.Emit(OpCodes.Ldstr, property.Name);
					
					// Load value of property to stack
					il.Emit(OpCodes.Ldarg_0);
					il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
					
					// Perform boxing if necessary
					if (property.PropertyType.IsValueType)
						il.Emit(OpCodes.Box, property.PropertyType);
					
					// stack at this point
					// 1. string or null (value)
					// 2. string (key)
					// 3. dictionary
					
					// Ready to call dictionary.Add(key, value)
					il.EmitCall(OpCodes.Callvirt, addMethod, null);
				}
				
				// Load dictionary and return
				il.Emit(OpCodes.Ldloc_0);
				il.Emit(OpCodes.Ret);
				
				return (Func<object, IDictionary<string, object>>)dynamicMethod.CreateDelegate(typeof(Func<object, IDictionary<string, object>>));
			}
		}
	}
}
