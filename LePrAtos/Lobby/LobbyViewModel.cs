// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;
using LePrAtos.Service_References;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Lobby
{
	/// <summary>
	///     ViewModel für <see cref="LobbyView" />
	/// </summary>
	[Export(typeof (LobbyViewModel))]
	public class LobbyViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _joinLobbyCommand;
		private DelegateCommand _startGameCommand;
		private gameLobby _lobby;

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyViewModel()
		{
			// ReSharper disable once ExplicitCallerInfoArgument
			Members.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(MemberCount));
		}

		/// <summary>
		///     Alle Mitglieder dieser Lobby
		/// </summary>
		public ObservableCollection<PlayerViewModel> Members { get; } = new ObservableCollection<PlayerViewModel>();

		/// <summary>
		///     Anzahl <see cref="Members" /> / maximum <see cref="MaxMemberCount" />
		/// </summary>
		public string MemberCount => $"{Members.Count} / {MaxMemberCount}";

		//TODO: Implementierung Server & Client
		private static int MaxMemberCount => 13;

		/// <summary>
		///     Lobby Name
		/// </summary>
		public string LobbyName { get; set; }

		/// <summary>
		///     Lobby ID
		/// </summary>
		public string LobbyId { get; set; }

		/// <summary>
		///     Lobby verfügt über ein Passwort
		/// </summary>
		public bool HasLobbyPassword { get; set; }

		/// <summary>
		///     Der Leiter der Lobby, darf beispielsweise leute aus der Lobby entfernen
		/// </summary>
		public string LobbyLeaderName { private get; set; }

		/// <summary>
		/// Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand LeaveLobbyCommand => _joinLobbyCommand ?? (_joinLobbyCommand = new DelegateCommand(LeaveLobby));
		
		/// <summary>
		/// Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand StartGameCommand => _startGameCommand ?? (_startGameCommand = new DelegateCommand(StartGame, CanStartGame));

		private bool CanStartGame()
		{
			return Members.Where(p => !Equals(p.PlayerId, LobbyLeaderName)).All(p => p.IsReady);
		}

		private void StartGame()
		{
			throw new NotImplementedException();
		}

		private void LeaveLobby()
		{
			//TODO Tell Server to check LobbyLeaderName

			//TODO Tell Server to remove me from Members

			Members.Remove(CurrentSession.Player);
			
			CurrentSession.Player.IsReady = false;
			
			var lobbyBrowserView = new LobbyBrowserView(Container.Resolve<LobbyBrowserViewModel>());

			lobbyBrowserView.Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Die vom Server stammende Lobby
		/// </summary>
		public gameLobby Lobby
		{
			set
			{
				if (Equals(_lobby, value))
				{
					return;
				}
				_lobby = value;
				LobbyId = _lobby.gameLobbyID;
				LobbyLeaderName = _lobby.gameLobbyAdmin;
				Members.Clear();
				foreach (var playerName in _lobby.gamePlayerListPublic)
				{
					var playerViewModel = Container.Resolve<PlayerViewModel>();
					playerViewModel.Player = new player {username = playerName};
					if (Equals(LobbyLeaderName, playerViewModel.Username))
					{
						playerViewModel.IsLeader = true;
					}
					Members.Add(playerViewModel);
				}

				LobbyName = "TODO SERVER";
				HasLobbyPassword = false;
			}
		}
	}
}