// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.GameManagerService;
using LePrAtos.Lobby;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// ViewModel für <see cref="LoginView"/>
	/// </summary>
	[Export(typeof(ILoginViewModel))]
	public sealed class LoginViewModel : ViewModelBase, ILoginViewModel
	{
		/// <summary>
		/// Benutzername
		/// </summary>
		public string Username { get; set; }

		private async void Login(PasswordBox passwordBox)
		{
			var client = new GameManagerClient(CurrentSession.Endpointconfiguration);
			var response = await client.loginAsync(Username);
			var lobbyBrowser = new CreateJoinLobbyView(response.ToString());
			lobbyBrowser.Show();
		}

		/// <summary>
		/// Command für die Anmeldung
		/// </summary>
		public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(Login));

		private ICommand _loginCommand;
	}
}
