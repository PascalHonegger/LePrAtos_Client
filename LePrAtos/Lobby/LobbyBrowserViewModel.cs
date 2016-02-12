// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain
using System;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Lobby
{
	internal class LobbyBrowserViewModel : IRequestDialogCloseViewModel
	{
		private ICommand _createLobbyCommand;
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));

		private void CreateLobby()
		{
			//TODO: get Lobby From Server
			var lobby = "TODO";

			var lobbyViewModel = new LobbyViewModel();

			new LobbyView(lobbyViewModel).Show();

			RequestDialogCloseEventHandler.Invoke(this, null);
		}

		public EventHandler RequestDialogCloseEventHandler { get; set; }
	}
}