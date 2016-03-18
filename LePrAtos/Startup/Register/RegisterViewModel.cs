// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Startup.Register
{
	/// <summary>
	///     Viewmodel for the registration
	/// </summary>
	public class RegisterViewModel : ViewModelBase, IRequestWindowClose
	{
		/// <summary>
		///     Die Maximallänge des <see cref="Username" />
		/// </summary>
		public const int UsernameMaxLength = 30;

		/// <summary>
		///     Die Minimallänge des <see cref="Username" />
		/// </summary>
		public const int UsernameMinLength = 3;

		/// <summary>
		///     Die Minimallänge des Passworts
		/// </summary>
		private const int PasswordMinLength = 5;

		/// <summary>
		///     Die Maximallänge des Passworts
		/// </summary>
		private const int PasswordMaxLength = 30;

		private const string MailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

		/// <summary>
		///     Regex für die Überprüfung von E-Mail-Adressen
		/// </summary>
		private static readonly Regex MailRegex = new Regex(MailPattern);

		private ICommand _cancelCommand;
		private string _mailAddress;
		private DelegateCommand<PasswordBox> _registerCommand;
		private string _username;

		/// <summary>
		///     Setzt die Standardwerte der Properties und führt somit die Validierung aus.
		/// </summary>
		public RegisterViewModel()
		{
			Username = string.Empty;
			MailAddress = string.Empty;
		}

		/// <summary>
		///     Der Benutzername für das Login und die Anzeige
		/// </summary>
		public string Username
		{
			get { return _username; }
			set
			{
				if (Equals(value, _username)) return;

				_username = value;

				var errors = new List<string>();

				if (_username.Length > UsernameMaxLength || _username.Length < UsernameMinLength)
				{
					errors.Add(string.Format(Strings.TextValidationRule_Length, UsernameMinLength, UsernameMaxLength));
				}

				if (_username.Contains("@"))
				{
					errors.Add(string.Format(Strings.TextValidationRule_ForbiddenChar, "@"));
				}

				if (!errors.Any() && !CurrentSession.Client.username_availability(_username))
				{
					errors.Add(Strings.TextValidationRule_UsernameAlreadyTaken);
				}

				SetErrorForProperty(errors);

				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Comand for cancelling registration
		/// </summary>
		public ICommand CancelCommand
			=>
				_cancelCommand ??
				(_cancelCommand = new DelegateCommand(CancelRegister));

		/// <summary>
		///     Comand for registration
		/// </summary>
		public DelegateCommand<PasswordBox> RegisterCommand
			=>
				_registerCommand ??
				(_registerCommand = new DelegateCommand<PasswordBox>(Register));

		/// <summary>
		///     Die Mailadresse des Users
		/// </summary>
		public string MailAddress
		{
			get { return _mailAddress; }
			set
			{
				if (Equals(value, _mailAddress)) return;

				_mailAddress = value;

				var errors = new List<string>();

				if (!MailRegex.IsMatch(_mailAddress))
				{
					errors.Add(Strings.TextValidationRule_MailValid);
				}
				else if (!CurrentSession.Client.email_verification(_mailAddress))
				{
					errors.Add(Strings.TextValidationRule_MailaddressAlreadyTaken);
				}

				SetErrorForProperty(errors);

				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Cancel the Registration
		/// </summary>
		private void CancelRegister()
		{
			var loginViewModel = new LoginViewModel {UsernameOrMail = Username};

			new LoginView(loginViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		private void Register(PasswordBox passwordBox)
		{
			BusyRunner.RunAsync(async () =>
			{
				var response =
					await
						CurrentSession.Client.registrationAsync(MailAddress, Username, PasswordHasher.HashPasswort(passwordBox.Password));

				var player = Container.Resolve<PlayerViewModel>();
				player.Player = response.@return;
				CurrentSession.Player = player;

				var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());
				lobbyBrowser.Show();
				RequestWindowCloseEvent.Invoke(this, null);
				}, Strings.RegisterView_BadRegister);
		}

		/// <summary>
		///     Entscheided, ob die Registrierung möglich ist
		/// </summary>
		/// <param name="password">Passwort</param>
		/// <param name="repeatPassword">Wiederholtes Passwort</param>
		/// <returns></returns>
		public bool CanRegister(string password, string repeatPassword)
		{
			return
				!HasErrors &&
				Equals(password, repeatPassword) &&
				password.Length <= PasswordMaxLength &&
				password.Length >= PasswordMinLength;
		}
	}
}