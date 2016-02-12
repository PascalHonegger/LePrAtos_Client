// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using LePrAtos.Infrastructure;
using LePrAtos.Service_References;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// ViewModel für <see cref="LobbyView"/>
	/// </summary>
	[Export(typeof(LobbyViewModel))]
	public class LobbyViewModel : ViewModelBase
	{
		/// <summary>
		/// Die Spieler in der jetzigen Lobby
		/// </summary>
		public ObservableCollection<PlayerViewModel> Players { get; } = new ObservableCollection<PlayerViewModel>
		{
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel()
		};

		//TODO RemoveFromLobby implementieren
		/*
		private DelegateCommand<string> _kickFromLobbyCommand;
		public DelegateCommand<string> KickFromLobbyCommand => _kickFromLobbyCommand ?? (_kickFromLobbyCommand = new DelegateCommand<string>(RemoveFromLobby));

		private void RemoveFromLobby(string user)
		{
			
		}*/
	}
}