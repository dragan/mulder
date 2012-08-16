using System;

namespace Mulder.Base.Extensions
{
	public static class StringExtensions
	{
		public static string Chop(this string text)
		{
			return text.Substring(0, text.Length - 1);
		}
	}
}
