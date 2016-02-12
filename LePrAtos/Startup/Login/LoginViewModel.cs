// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Controls;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References.GameManagerService;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     ViewModel für <see cref="LoginView" />
	/// </summary>
	public sealed class LoginViewModel : ViewModelBase, IRequestDialogCloseViewModel
	{
		private DelegateCommand<PasswordBox> _loginCommand;
		private string _username = string.Empty;
		private const int UsernameMaxLength = 30;
		private const int UsernameMinLength = 3;

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
		public static string SelectedLanguage
		{
			get { return Settings.Default.SelectedCulture; }
			set
			{
				Settings.Default.SelectedCulture = value;
				Settings.Default.Save();

				Strings.Culture = new CultureInfo(value);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(value);
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
		public DelegateCommand<PasswordBox> LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(Login, box => Username.Length >= UsernameMinLength));

		/// <summary>
		/// Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestDialogCloseEventHandler { get; set; }

		private async void Login(PasswordBox passwordBox)
		{
			var client = new GameManagerClient(CurrentSession.Endpointconfiguration);
			var response = await client.loginAsync(Username);
			var lobbyBrowser = new LobbyBrowserView(response.Body.loginReturn);
			lobbyBrowser.Show();

			RequestDialogCloseEventHandler.Invoke(this, null);
		}
	}
}