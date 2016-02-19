// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;
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

			lobby2.HasLobbyPassword = true;

			lobby2.Members.Add(player1);
			lobby2.Members.Add(player2);
			lobby2.Members.Add(player3);

			AvailableLobbies.Add(lobby2);
		}

		private ICommand _createLobbyCommand;
		private DelegateCommand _joinLobbyCommand;
		private ICommand _logoutCommand;
		private LobbyViewModel _seletedLobby;
		private string _lobbyPassword;

		/// <summary>
		/// Command zum erstellen einer Lobby
		/// </summary>
		public ICommand CreateLobbyCommand => _createLobbyCommand ?? (_createLobbyCommand = new DelegateCommand(CreateLobby));
		
		/// <summary>
		/// Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand JoinLobbyCommand => _joinLobbyCommand ?? (_joinLobbyCommand = new DelegateCommand(JoinLobby, CanJoinLobby));

		/// <summary>
		/// Command zum Abmelden des angemeldeten Spielers
		/// </summary>
		public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

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
			return SeletedLobby != null && (SeletedLobby.HasLobbyPassword && !string.IsNullOrEmpty(LobbyPassword) || !SeletedLobby.HasLobbyPassword);
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