// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
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
		public const int UsernameMaxLength = 30;
		public const int UsernameMinLength = 3;
		private DelegateCommand<PasswordBox> _loginCommand;
		private string _username = string.Empty;
		private ICommand _registerCommand;

		/// <summary>
		///     Alle möglichen Sprachen
		/// </summary>
		public static IEnumerable<string> PossibleLanguages
		{
			get
			{
				var supportedCultures = new List<string>();

				var rm = new ResourceManager(typeof (Strings));

				var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
				foreach (var culture in cultures.Where(c => !string.IsNullOrEmpty(c.Name)))
				{
					try
					{
						var rs = rm.GetResourceSet(culture, true, false);

						if (rs != null)
						{
							supportedCultures.Add(culture.Name);
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
		public string SelectedLanguage
		{
			get { return Settings.Default.SelectedCulture; }
			set
			{
				Settings.Default.SelectedCulture = value;
				Settings.Default.Save();

				Strings.Culture = new CultureInfo(value);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(value);

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
				if (_username == value || value.Length > UsernameMaxLength) return;


				_username = value;
				OnPropertyChanged();
				LoginCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Command für die Anmeldung
		/// </summary>
		public DelegateCommand<PasswordBox> LoginCommand
			=>
				_loginCommand ??
				(_loginCommand = new DelegateCommand<PasswordBox>(Login, box => Username.Length >= UsernameMinLength));

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
			var registerView = new RegisterView();
			registerView.ShowDialog();
		}

		private async void Login(PasswordBox passwordBox)
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
	}
}