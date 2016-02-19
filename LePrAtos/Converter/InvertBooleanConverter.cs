// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Globalization;
using System.Windows.Data;

namespace LePrAtos.Converter
{
	/// <summary>
	/// Ein Converter für das invertieren eines <see cref="bool"/>. Falls der Input-Wert kein <see cref="bool"/> ist, wird false zurückgegeben
	/// </summary>
	[ValueConversion(typeof (bool), typeof (bool))]
	public class InvertBooleanConverter : IValueConverter
	{
		/// <summary>
		///     Konvertiert einen Wert.
		/// </summary>
		/// <returns>
		///     Ein konvertierter Wert.Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.
		/// </returns>
		/// <param name="value">Der von der Bindungsquelle erzeugte Wert.</param>
		/// <param name="targetType">Der Typ der Bindungsziel-Eigenschaft.</param>
		/// <param name="parameter">Der zu verwendende Konverterparameter.</param>
		/// <param name="culture">Die im Konverter zu verwendende Kultur.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !(bool) value;
			}

			return false;
		}

		/// <summary>
		///     Konvertiert einen Wert.
		/// </summary>
		/// <returns>
		///     Ein konvertierter Wert.Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.
		/// </returns>
		/// <param name="value">Der Wert, der vom Bindungsziel erzeugt wird.</param>
		/// <param name="targetType">Der Typ, in den konvertiert werden soll.</param>
		/// <param name="parameter">Der zu verwendende Konverterparameter.</param>
		/// <param name="culture">Die im Konverter zu verwendende Kultur.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}
	}
}