// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Controls;
using LePrAtos.Properties;
using LePrAtos.Startup.Register;

namespace LePrAtos.ValidationRules
{
	/// <summary>
	///     <see cref="ValidationRule" /> für die überprüfung von Text
	/// </summary>
	public class TextValidationRule : ValidationRule
	{
		/// <summary>
		///     Die verbotenen Zeichen
		/// </summary>
		public string[] ForbiddenChars { get; set; }

		/// <summary>
		///     Die benötigten Zeichen
		/// </summary>
		public string[] MandatoryChars { get; set; }

		/// <summary>
		///     Die Mindestlänge
		/// </summary>
		public int MinLength { private get; set; }

		/// <summary>
		///     Die Maximallänge
		/// </summary>
		public int MaxLength { private get; set; } = int.MaxValue;

		/// <summary>
		///     True, wenn diese überprüft werden sollte, ob eine Valide E-Mail-Adresse eingegeben wurde
		/// </summary>
		public bool IsMailAddress { private get; set; }

		/// <summary>
		///     Führt beim Überschreiben in einer abgeleiteten Klasse Validierungsprüfungen für einen Wert durch.
		/// </summary>
		/// <returns>
		///     Ein <see cref="T:System.Windows.Controls.ValidationResult" />-Objekt.
		/// </returns>
		/// <param name="value">Der Wert aus dem Bindungsziel, der überprüft werden soll.</param>
		/// <param name="cultureInfo">Die in dieser Regel zu verwendende Kultur.</param>
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return new ValidationResult(false, string.Empty);
			}

			var results = new StringBuilder();

			if (text.Length < MinLength || text.Length > MaxLength)
			{
				results.AppendLine(string.Format(Strings.TextValidationRule_Lenght, MinLength, MaxLength));
			}

			if (ForbiddenChars != null && ForbiddenChars.Any())
			{
				foreach (
					var forbiddenChar in
						ForbiddenChars.Where(forbiddenChar => !string.IsNullOrEmpty(forbiddenChar) && text.Contains(forbiddenChar)))
				{
					results.AppendLine(string.Format(Strings.TextValidationRule_ForbiddenChar, forbiddenChar));
				}
			}

			if (MandatoryChars != null && MandatoryChars.Any())
			{
				foreach (
					var mandatorychar in
						MandatoryChars.Where(forbiddenChar => !string.IsNullOrEmpty(forbiddenChar) && text.Contains(forbiddenChar)))
				{
					results.AppendLine(string.Format(Strings.TextValidationRule_MandatoryChar, mandatorychar));
				}
			}

			if (IsMailAddress)
			{
				if (!RegisterViewModel.MailRegex.IsMatch(text))
				{
					results.AppendLine(string.Format(Strings.TextValidationRule_MailValid));
				}
			}

			var message = results.ToString().Trim(Environment.NewLine.ToCharArray());

			return new ValidationResult(string.IsNullOrEmpty(message), message);
		}
	}
}