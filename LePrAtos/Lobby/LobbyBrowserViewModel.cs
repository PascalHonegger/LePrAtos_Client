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
		/// <summary>
		/// Constructor
		/// </summary>
		public LobbyBrowserViewModel()
		{
			//TODO get Lobbies from Server


			var player1 = Container.Resolve<PlayerViewModel>();

			player1.PlayerId = "Example 1";
			player1.Username = "ExmaplePlayer 1";

			var player2 = Container.Resolve<PlayerViewModel>();

			player2.PlayerId = "Example 2";
			player2.Username = "ExmaplePlayer 2";

			var player3 = Container.Resolve<PlayerViewModel>();

			player3.PlayerId = "Example 3";
			player3.Username = "ExmaplePlayer 3";


			var lobby1 = Container.Resolve<LobbyViewModel>();

			lobby1.LobbyId = "Example 1";

			lobby1.LobbyName = "Example 1";

			lobby1.Members.Add(player1);
			lobby1.Members.Add(player2);

			AvailableLobbies.Add(lobby1);

			var lobby2 = Container.Resolve<LobbyViewModel>();

			lobby2.LobbyId = "Example 2";

			lobby2.LobbyName = "Example 2";

			lobby2.Members.Add(player1);
			lobby2.Members.Add(player2);
			lobby2.Members.Add(player3);

			AvailableLobbies.Add(lobby2);
		}

		private ICommand _createLobbyCommand;
		private DelegateCommand _joinLobbyCommand;
		private LobbyViewModel _seletedLobby;

		/// <summary>
		/// Command zum erstellen einer Lobby
		/// </summary>
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));
		
		/// <summary>
		/// Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand JoinLobbyCommand => _joinLobbyCommand ?? (_joinLobbyCommand = new DelegateCommand(JoinLobby, CanJoinLobby));

		private bool CanJoinLobby()
		{
			return SeletedLobby != null;
		}

		private void JoinLobby()
		{
			new LobbyView(SeletedLobby).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		private void CreateLobby()
		{
			//TODO: get Lobby From Server

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			lobbyViewModel.Members.Add(CurrentSession.Player);

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
		public ObservableCollection<LobbyViewModel> AvailableLobbies { get; } = new ObservableCollection<LobbyViewModel>();

		/// <summary>
		/// Passwort, welches für das beitreten in die Lobby verwendet wird
		/// </summary>
		public string LobbyPassword { get; set; }

		/// <summary>
		/// Die ausgewählte Lobby
		/// </summary>
		public LobbyViewModel SeletedLobby
		{
			get { return _seletedLobby; }
			set
			{
				if (Equals(value, _seletedLobby))
				{
					return;
				}

				_seletedLobby = value;
				JoinLobbyCommand.RaiseCanExecuteChanged();
			}
		}
	}
}