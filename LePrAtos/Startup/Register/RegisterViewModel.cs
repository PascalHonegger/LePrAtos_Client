// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using LePrAtos.Lobby;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Unity;

namespace LePrAtos.Startup.Register
{
	public class RegisterViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _cancelCommand;
		private DelegateCommand<PasswordBox> _registerCommand;

		public string Username { get; set; }

		public string Sallary
		{
			get { return _sallary.ToString(); }
			set { int.TryParse(value, out _sallary); }
		}

		private int _sallary;

		public ICommand CancelCommand
		=>
				_cancelCommand ??
				(_cancelCommand = new DelegateCommand(CancelRegister));

		private void CancelRegister()
		{
			var loginWindow = new LoginView(new LoginViewModel());

			loginWindow.Show();
		    RequestWindowCloseEvent.Invoke(this, null);
		}

		public DelegateCommand<PasswordBox> RegisterCommand
		=>
				_registerCommand ??
				(_registerCommand = new DelegateCommand<PasswordBox>(Register));

		private async void Register(PasswordBox passwordBox)
		{
/*			Password = passwordBox.Password;
			var personId = CurrentSession.Client.login(Username);
			CurrentSession.Player = new PlayerViewModel {Player = personId};
			var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());
			lobbyBrowser.Show();
			RequestWindowCloseEvent.Invoke(this, null); */

			var response = await CurrentSession.Client.loginAsync(Username);

			var player = Container.Resolve<PlayerViewModel>();

			player.Player = response.@return;

			CurrentSession.Player = player;

			var lobbyBrowser = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());

			lobbyBrowser.Show();

			RequestWindowCloseEvent.Invoke(this, null);

		}

		public EventHandler RequestWindowCloseEvent { get; set; }
	}
}
