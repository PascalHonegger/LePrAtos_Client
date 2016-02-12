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
using LePrAtos.Service_References.GameManagerService;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     ViewModel für <see cref="LoginView" />
	/// </summary>
	[Export(typeof (ILoginViewModel))]
	public sealed class LoginViewModel : ViewModelBase, ILoginViewModel
	{
		private ICommand _loginCommand;

		/// <summary>
		///     Alle möglichen Sprachen
		/// </summary>
		public IEnumerable<string> PossibleLanguages
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
		public string Username { get; set; }

		/// <summary>
		///     Command für die Anmeldung
		/// </summary>
		public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(Login));

		/// <summary>
		/// Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestDialogCloseEventHandler { get; set; }

		private async void Login(PasswordBox passwordBox)
		{
			var client = new GameManagerClient(CurrentSession.Endpointconfiguration);
			var response = await client.loginAsync(Username);
			var lobbyBrowser = new CreateJoinLobbyView(response.ToString());
			lobbyBrowser.Show();

			RequestDialogCloseEventHandler.Invoke(this, null);
		}
	}
}