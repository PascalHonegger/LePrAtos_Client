﻿// Projekt: LePrAtos
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
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Service_References.GameManagerService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     ViewModel für <see cref="LoginView" />
	/// </summary>
	[Export(typeof(LoginViewModel))]
	public sealed class LoginViewModel : ViewModelBase, IRequestWindowClose
	{
		private const int UsernameMaxLength = 30;
		private const int UsernameMinLength = 3;
		private DelegateCommand<PasswordBox> _loginCommand;
		private string _username = string.Empty;

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
				if (_username != value && value.Length <= UsernameMaxLength)
				{
					_username = value;
					OnPropertyChanged();
				}

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

		private async void Login(PasswordBox passwordBox)
		{
			var client = new GameManagerClient(CurrentSession.Endpointconfiguration);

			//TODO Use
			//var response = await client.loginAsync(Username);

			var player = Container.Resolve<PlayerViewModel>();

			player.PlayerId = "Example xy";

			player.Username = "Exmaple xy";

			CurrentSession.Player = player;

			if (SaveLogin)
			{
				Settings.Default.SavedUser = CurrentSession.Player.PlayerId;
				Settings.Default.Save();
			}

			var lobbyBrowserViewModel = Container.Resolve<LobbyBrowserViewModel>();

			var lobbyBrowser = new LobbyBrowserView(lobbyBrowserViewModel);

			lobbyBrowser.Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}
	}
}