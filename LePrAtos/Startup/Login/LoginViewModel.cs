// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using System.Threading;
using System.Windows;
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
		/// <summary>
		///     Die Maximallänge des <see cref="Username"/>
		/// </summary>
		public const int UsernameMaxLength = 30;

		/// <summary>
		///     Die Minimallänge des <see cref="Username"/>
		/// </summary>
		public const int UsernameMinLength = 3;

		/// <summary>
		///     Die Minimallänge des Passworts
		/// </summary>
		public const int PasswordMinLength = 5;
		private DelegateCommand<string> _loginCommand;
		private string _username = string.Empty;
		private ICommand _registerCommand;

		/// <summary>
		///     Alle möglichen Sprachen
		/// </summary>
		public static IEnumerable<LanguageViewModel> PossibleLanguages
		{
			get
			{
				var supportedCultures = new List<LanguageViewModel>();

				var rm = new ResourceManager(typeof(Strings));

				var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
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
		///     Ausgewählte Sprache
		/// </summary>
		public LanguageViewModel SelectedLanguage
		{
			get
			{
				return PossibleLanguages.FirstOrDefault(l => Equals(l.Culture.Name, Settings.Default.SelectedCulture));
			}
			set
			{
				if (value == null) return;

				Settings.Default.SelectedCulture = value.Culture.Name;
				Settings.Default.Save();

				Strings.Culture = value.Culture;
				Thread.CurrentThread.CurrentUICulture = value.Culture;

				//Reload Language
				RequestWindowCloseEvent.Invoke(new LoginView(this), null);
			}
		}

		/// <summary>
		///     Benutzername
		/// </summary>
		public string Username
		{
			get { return _username; }
			set
			{
				if (value != null && (_username == value || value.Length > UsernameMaxLength)) return;

				_username = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Command für die Anmeldung
		/// </summary>
		public DelegateCommand<string> LoginCommand
			=>
				_loginCommand ??
				(_loginCommand = new DelegateCommand<string>(Login));

		/// <summary>
		///     Entscheided, ob das Login ausgeführt werden kann
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool CanLogin(string password)
		{
			return Username.Length >= UsernameMinLength && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(password);
		}

		/// <summary>
		///     Bestimmt, ob der user beim nächsten Starten der Application angemeldet bleibt
		/// </summary>
		public bool SaveLogin { get; set; }

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Command für die Registrierung
		/// </summary>
		public ICommand RegisterCommand
			=>
				_registerCommand ??
				(_registerCommand = new DelegateCommand(Register));

		private void Register()
		{
			var registerView = new RegisterView(new RegisterViewModel { Username = Username});
			registerView.Show();
			RequestWindowCloseEvent.Invoke(this, null);
		}

		private async void Login(string passwordBox)
		{
			try
			{
				var response = await CurrentSession.Client.loginAsync(Username);

				var player = Container.Resolve<PlayerViewModel>();

				player.Player = response.@return;

				CurrentSession.Player = player;

				if (SaveLogin)
				{
					Settings.Default.SavedUser = CurrentSession.Player.PlayerId;
					Settings.Default.Save();
				}

				var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());

				lobbyBrowser.Show();

				RequestWindowCloseEvent.Invoke(this, null);
			}
			catch (FaultException e)
			{
				MessageBox.Show(e.Message);
			}
		}
	}
}