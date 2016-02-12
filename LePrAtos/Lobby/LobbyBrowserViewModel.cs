// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain
using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Lobby
{
	internal class LobbyBrowserViewModel
	{
		private ICommand _createLobbyCommand;
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));

		private void CreateLobby()
		{
			throw new NotImplementedException();
		}
	}
}