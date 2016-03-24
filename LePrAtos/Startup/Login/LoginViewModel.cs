// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.Register;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     ViewModel für <see cref="LoginView" />
	/// </summary>
	public sealed class LoginViewModel : ViewModelBase, IRequestWindowClose
	{
		private DelegateCommand<PasswordBox> _loginCommand;
		private ICommand _registerCommand;
		private string _usernameOrMail = string.Empty;

		/// <summary>
		///     Alle möglichen Sprachen
		/// </summary>
		public static IEnumerable<LanguageViewModel> PossibleLanguages
		{
			get
			{
				var supportedCultures = new List<LanguageViewModel>();

				var rm = new ResourceManager(typeof (Strings));

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
				OnPropertyChanged();
			}
		}

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

				//New LoginViewModel and new View to completely reload language

				var loginViewModel = new LoginViewModel
				{
					UsernameOrMail = UsernameOrMail,
					SaveLogin = SaveLogin
				};

				new LoginView(loginViewModel).Show();

				RequestWindowCloseEvent.Invoke(this, null);
			}
		}

		/// <summary>
		///     Benutzername
		/// </summary>
		public string UsernameOrMail
		{
			get
			{
				SetErrorForProperty(string.IsNullOrEmpty(_usernameOrMail) ? Strings.TextValidationRule_Mandatory : null);
				return _usernameOrMail;
			}
			set
			{
				if (Equals(value, _usernameOrMail)) return;

				_usernameOrMail = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Command für die Anmeldung
		/// </summary>
		public DelegateCommand<PasswordBox> LoginCommand
			=>
				_loginCommand ??
				(_loginCommand = new DelegateCommand<PasswordBox>(Login));

		/// <summary>
		///     Bestimmt, ob der user beim nächsten Starten der Application angemeldet bleibt
		/// </summary>
		public bool SaveLogin { get; set; }

		/// <summary>
		///     Command für die Registrierung
		/// </summary>
		public ICommand RegisterCommand
			=>
				_registerCommand ??
				(_registerCommand = new DelegateCommand(Register));

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Entscheided, ob das Login ausgeführt werden kann
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool CanLogin(string password)
		{
			return !string.IsNullOrEmpty(password) && !HasErrors;
		}

		private void Register()
		{
			var registerView = new RegisterView(new RegisterViewModel {Username = UsernameOrMail});
			registerView.Show();
			RequestWindowCloseEvent.Invoke(this, null);
		}

		private void Login(PasswordBox box)
		{
			BusyRunner.RunAsync(async () =>
			{
				await LoginUser(box.Password);

				var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());

				lobbyBrowser.Show();

				RequestWindowCloseEvent.Invoke(this, null);
			}, Strings.LoginView_BadLogin);
		}

		/// <summary>
		///     Meldet den User an, indem 
		/// </summary>
		/// <param name="password">Das zu verwendende Passwort</param>
		public async Task LoginUser(string password)
		{
			var response = await CurrentSession.Client.loginAsync(UsernameOrMail, PasswordHasher.HashPasswort(password));

			var player = Container.Resolve<PlayerViewModel>();

			player.Player = response.@return;

			CurrentSession.Player = player;

			if (SaveLogin)
			{
				Settings.Default.SavedUser = CurrentSession.Player.PlayerId;
				Settings.Default.Save();
			}
		}
	}
}