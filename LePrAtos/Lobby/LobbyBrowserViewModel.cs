// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Lobby
{
	/// <summary>
	///     ViewModel für für die Auswahl einer Lobby
	/// </summary>
	[Export(typeof (LobbyBrowserViewModel))]
	public class LobbyBrowserViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _createLobbyCommand;
		private DelegateCommand _joinLobbyCommand;
		private string _lobbyPassword;
		private ICommand _logoutCommand;
		private LobbyViewModel _seletedLobby;

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyBrowserViewModel()
		{
			//TODO Load Data from Server
			var playerViewModel1 = Container.Resolve<PlayerViewModel>();
			var player1 = new player
			{
				username = "ExmaplePlayer 1"
			};
			playerViewModel1.Player = player1;

			var playerViewModel2 = Container.Resolve<PlayerViewModel>();
			var player2 = new player
			{
				username = "ExmaplePlayer 2"
			};
			playerViewModel2.Player = player2;

			var playerViewModel3 = Container.Resolve<PlayerViewModel>();
			var player3 = new player
			{
				username = "ExmaplePlayer 3"
			};
			playerViewModel3.Player = player3;

			var lobby1 = Container.Resolve<LobbyViewModel>();

			lobby1.LobbyId = "Example 1";

			lobby1.LobbyName = "Example 1";

			lobby1.LobbyLeaderId = "Example 1";

			playerViewModel1.IsLeader = true;

			lobby1.Members.Add(playerViewModel1);

			var lobby2 = Container.Resolve<LobbyViewModel>();

			lobby2.LobbyId = "Example 2";

			lobby2.LobbyName = "Example 2";

			lobby2.LobbyLeaderId = "Example 2";

			playerViewModel2.IsLeader = true;

			lobby2.HasLobbyPassword = true;

			lobby2.Members.Add(playerViewModel2);
			lobby2.Members.Add(playerViewModel3);


			AvailableLobbies.Add(lobby1);
			AvailableLobbies.Add(lobby2);



		}

		/// <summary>
		///     Command zum erstellen einer Lobby
		/// </summary>
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));

		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand JoinLobbyCommand
			=> _joinLobbyCommand ?? (_joinLobbyCommand = new DelegateCommand(JoinLobby, CanJoinLobby));

		/// <summary>
		///     Command zum Abmelden des angemeldeten Spielers
		/// </summary>
		public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

		/// <summary>
		///     Alle verfügbaren Lobbies
		/// </summary>
		public ObservableCollection<LobbyViewModel> AvailableLobbies { get; } = new ObservableCollection<LobbyViewModel>();

		/// <summary>
		///     Passwort, welches für das beitreten in die Lobby verwendet wird
		/// </summary>
		public string LobbyPassword
		{
			get { return _lobbyPassword; }
			set
			{
				if (Equals(_lobbyPassword, value))
				{
					return;
				}
				_lobbyPassword = value;
				JoinLobbyCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Die ausgewählte Lobby
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

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		private void Logout()
		{
			var preSelectedUsername = CurrentSession.Player.Username;

			CurrentSession.Player = null;

			Settings.Default.SavedUser = null;

			var loginViewModel = Container.Resolve<LoginViewModel>();

			loginViewModel.Username = preSelectedUsername;

			var loginView = new LoginView(loginViewModel);

			loginView.Show();

			RequestWindowCloseEvent(this, null);
		}

		private bool CanJoinLobby()
		{
			return SeletedLobby != null &&
			       (SeletedLobby.HasLobbyPassword && !string.IsNullOrEmpty(LobbyPassword) || !SeletedLobby.HasLobbyPassword);
		}

		private void JoinLobby()
		{
			//TODO Tell Server to add me to the lobby

			SeletedLobby.Members.Add(CurrentSession.Player);

			new LobbyView(SeletedLobby).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		private async void CreateLobby()
		{
			var result = (await CurrentSession.Client.createGameLobbyAsync(CurrentSession.Player.PlayerId)).@return;

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			lobbyViewModel.Lobby = result;

			lobbyViewModel.Members.Add(CurrentSession.Player);

			lobbyViewModel.LobbyLeaderId = CurrentSession.Player.PlayerId;

			CurrentSession.Player.IsReady = true;
			CurrentSession.Player.IsLeader = true;

			new LobbyView(lobbyViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}
	}
}