// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Collections.ObjectModel;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// ViewModel für <see cref="LobbyView"/>
	/// </summary>
	public class LobbyViewModel
	{
		/// <summary>
		/// Die Spieler in der jetzigen Lobby
		/// </summary>
		public ObservableCollection<string> Players { get; } = new ObservableCollection<string>();


		//TODO RemoveFromLobby implementieren
		/*
		private DelegateCommand<string> _kickFromLobbyCommand;
		public DelegateCommand<string> KickFromLobbyCommand => _kickFromLobbyCommand ?? (_kickFromLobbyCommand = new DelegateCommand<string>(RemoveFromLobby));

		private void RemoveFromLobby(string user)
		{
			
		}*/
	}
}