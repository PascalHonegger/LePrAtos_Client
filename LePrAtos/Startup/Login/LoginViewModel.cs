// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Lobby;
using LePrAtos.Service_References.HelloWorldService;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// ViewModel für LoginView
	/// </summary>
	public sealed class LoginViewModel : ViewModelBase
	{
		/// <summary>
		/// Benutzername
		/// </summary>
		public string Username { get; set; }

		private async void Login(PasswordBox passwordBox)
		{
			var client = new HelloWorldClient(Session.Instance.Endpointconfiguration);
			var response = await client.sayHelloWorldAsync($"{Username} ; {passwordBox.Password}");
			var lobbyBrowser = new CreateJoinLobby(response.Body.sayHelloWorldReturn);
			lobbyBrowser.Show();
		}

		/// <summary>
		/// Command für <see cref="Login"/>
		/// </summary>
		public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(Login));

		private ICommand _loginCommand;
	}
}
