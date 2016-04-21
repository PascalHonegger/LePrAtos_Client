// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.CustomSettings;
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
		private ICommand _resetPasswordCommand;
		private ICommand _settingsCommand;
		private string _usernameOrMail;

		/// <summary>
		///     Setzt die Default-Werte für die Validierung
		/// </summary>
		public LoginViewModel()
		{
			UsernameOrMail = string.Empty;
		}

		/// <summary>
		///     Benutzername
		/// </summary>
		public string UsernameOrMail
		{
			get { return _usernameOrMail; }
			set
			{
				if (Equals(value, _usernameOrMail)) return;

				_usernameOrMail = value;

				SetErrorForProperty(string.IsNullOrEmpty(_usernameOrMail) ? Strings.TextValidationRule_Mandatory : null);

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
		///     Command für den Passwort-Reset
		/// </summary>
		public ICommand ResetPasswordCommand
			=>
				_resetPasswordCommand ??
				(_resetPasswordCommand = new DelegateCommand(ResetPassword));

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Command zum öffnen der Einstellungen
		/// </summary>
		public ICommand SettingsCommand
		=>
				_settingsCommand ??
				(_settingsCommand = new DelegateCommand(OpenSettings));

		private void OpenSettings()
		{
			new SettingsView().Show();
			RequestWindowCloseEvent?.Invoke(this, EventArgs.Empty);
		}

		private static void ResetPassword()
		{
			new ResetPasswordView().ShowDialog();
		}

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
			});
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