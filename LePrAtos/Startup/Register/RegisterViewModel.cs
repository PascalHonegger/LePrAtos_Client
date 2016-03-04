// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
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
		public const int PasswordMinLength = 5;

		/// <summary>
		///     Die Maximallänge des Passworts
		/// </summary>
		public const int PasswordMaxLength = 30;

		private const string MailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

		/// <summary>
		///     Regex für die Überprüfung von E-Mail-Adressen
		/// </summary>
		public static readonly Regex MailRegex = new Regex(MailPattern);
		private ICommand _cancelCommand;
		private DelegateCommand<PasswordBox> _registerCommand;

		/// <summary>
		///     Der Benutzername für das Login und die Anzeige
		/// </summary>
		public string Username { get; set; }

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
		public string MailAddress { get; set; } = string.Empty;

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Cancel the Registration
		/// </summary>
		private void CancelRegister()
		{
			var loginWindow = new LoginView(new LoginViewModel());

			loginWindow.Show();
			RequestWindowCloseEvent.Invoke(this, null);
		}

		private async void Register(PasswordBox passwordBox)
		{
			try
			{
				var response = await CurrentSession.Client.registrationAsync(MailAddress, Username, passwordBox.Password);

				var player = Container.Resolve<PlayerViewModel>();

				player.Player = response.@return;

				CurrentSession.Player = player;

				var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());

				lobbyBrowser.Show();

				RequestWindowCloseEvent.Invoke(this, null);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Fehlurr");
			}
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
				Username.Length <= UsernameMaxLength &&
				Username.Length >= UsernameMinLength &&
				!Username.Contains("@") &&
				MailRegex.IsMatch(MailAddress) &&
				Equals(password, repeatPassword) &&
				password.Length <= PasswordMaxLength &&
				password.Length >= PasswordMinLength;
		}
	}
}