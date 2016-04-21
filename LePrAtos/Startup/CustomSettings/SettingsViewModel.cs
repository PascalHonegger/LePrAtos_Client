// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Startup.CustomSettings
{
	public class SettingsViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _restartCommand;
		private ICommand _returnCommand;

		/// <summary>
		///     Command für den Neustart
		/// </summary>
		public ICommand RestartCommand
			=>
				_restartCommand ??
				(_restartCommand = new DelegateCommand(Restart));

		/// <summary>
		///     Command für das zurückgehen zum Login
		/// </summary>
		public ICommand ReturnCommand
			=>
				_returnCommand ??
				(_returnCommand = new DelegateCommand(Return));

		/// <summary>
		///     Ausgewählte Sprache
		/// </summary>
		public LanguageViewModel SelectedLanguage
		{
			get { return PossibleLanguages.FirstOrDefault(l => Equals(l.Culture.Name, Settings.Default.SelectedCulture)); }
			set
			{
				if (value == null) return;

				Settings.Default.SelectedCulture = value.Culture.Name;
				Settings.Default.Save();

				Strings.Culture = value.Culture;
				Thread.CurrentThread.CurrentUICulture = value.Culture;
				Thread.CurrentThread.CurrentCulture = value.Culture;

				new SettingsView().Show();

				RequestWindowCloseEvent?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		///     Alle konfigurierten Themes
		/// </summary>
		public static IEnumerable<string> PossibleThemes => Settings.Default.ConfiguredThemes.Cast<string>();

		/// <summary>
		///     Das momentan ausgewälte Theme
		/// </summary>
		public string SelectedTheme
		{
			get { return Settings.Default.SelectedTheme; }
			set
			{
				if (Equals(value, Settings.Default.SelectedTheme)) return;
				Settings.Default.SelectedTheme = value;
				Settings.Default.Save();
			}
		}

		/// <summary>
		///     Alle möglichen Sprachen
		/// </summary>
		public static IEnumerable<LanguageViewModel> PossibleLanguages
		{
			get
			{
				var supportedCultures = new List<LanguageViewModel>();

				var rm = new ResourceManager(typeof(Strings));

				var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Distinct();
				foreach (var culture in cultures.Where(c => !string.IsNullOrEmpty(c.Name)))
				{
					try
					{
						var rs = rm.GetResourceSet(culture, true, false);

						if (rs != null)
						{
							supportedCultures.Add(new LanguageViewModel(culture));
						}
					}
					catch (CultureNotFoundException)
					{
						// Culture not supported
					}
				}

				return supportedCultures;
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		private void Return()
		{
			var viewModel = Container.Resolve<LoginViewModel>();
			new LoginView(viewModel).Show();
			RequestWindowCloseEvent?.Invoke(this, EventArgs.Empty);
		}

		private static void Restart()
		{
			Process.Start(Assembly.GetEntryAssembly().Location);
			Environment.Exit(-1);
		}
	}
}