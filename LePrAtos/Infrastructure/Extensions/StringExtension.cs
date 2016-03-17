// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;

namespace LePrAtos.Infrastructure.Extensions
{
	public static class StringExtension
	{
		public static bool Contains(this string source, string value, StringComparison comp)
		{
			return source != null && value != null && source.IndexOf(value, comp) >= 0;
		}
	}
}