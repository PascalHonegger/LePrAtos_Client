// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Globalization;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     Beinhaltet den Text mit der dazugehörigen Flagge einer Sprache
	/// </summary>
	public class LanguageViewModel
	{
		/// <summary>
		///     Setzt den <see cref="DisplayText" /> und <see cref="PathToFlag" />
		/// </summary>
		/// <param name="culture">Die Sprache</param>
		public LanguageViewModel(CultureInfo culture)
		{
			DisplayText = culture.DisplayName;
			PathToFlag = $"../../Resources/{culture.Name}.jpg";
			Culture = culture;
		}

		/// <summary>
		///     Der Name der Sprache
		/// </summary>
		public string DisplayText { get; }

		/// <summary>
		///     Der Pfad zu der Datai der dazugehörigen Flagge. Die Flagge muss jeweils Kürzel + .jpg heissten. Bsp: de-CH.jpg
		/// </summary>
		public string PathToFlag { get; }

		/// <summary>
		///     Die ausgewählte Kultur / Sprache
		/// </summary>
		public CultureInfo Culture { get; }

		private bool Equals(LanguageViewModel other)
		{
			return Equals(Culture, other.Culture);
		}

		/// <summary>
		///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((LanguageViewModel) obj);
		}

		/// <summary>
		///     Fungiert als die Standardhashfunktion.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Culture?.GetHashCode() ?? 0;
		}
	}
}