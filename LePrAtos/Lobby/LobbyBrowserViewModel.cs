// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Service_References;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// ViewModel für für die Auswahl einer Lobby
	/// </summary>
	[Export(typeof(LobbyBrowserViewModel))]
	public class LobbyBrowserViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _createLobbyCommand;

		/// <summary>
		/// Command zum erstellen einer Lobby
		/// </summary>
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));

		private void CreateLobby()
		{
			//TODO: get Lobby From Server

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			new LobbyView(lobbyViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		/// Alle verfügbaren Lobbies
		/// </summary>
		public ObservableCollection<GameViewModel> AwailableLobbies => new ObservableCollection<GameViewModel>
		{
			new GameViewModel(),
			new GameViewModel(),
			new GameViewModel(),
			new GameViewModel(),
			new GameViewModel()
		};

		/// <summary>
		/// Passwort, welches für das beitreten in die Lobby verwendet wird
		/// </summary>
		public string LobbyPassword { get; set; }
	}
}