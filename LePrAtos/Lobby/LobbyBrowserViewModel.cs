// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

namespace LePrAtos.Lobby
{
	/// <summary>
	///     ViewModel für für die Auswahl einer Lobby
	/// </summary>
	[Export(typeof (LobbyBrowserViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class LobbyBrowserViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _createLobbyCommand;
		private DelegateCommand _joinLobbyCommand;
		private string _lobbyPassword;
		private ICommand _logoutCommand;
		private ICommand _refreshViewCommand;
		private LobbyViewModel _seletedLobby;


		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyBrowserViewModel()
		{
			Refresh();
			CurrentSession.PollingTimer.Start();
		}

		/// <summary>
		///     Entscheidet, ob die Lobbies neue Daten vom Server laden
		/// </summary>
		public bool IsRefreshing
		{
			set
			{
				foreach (var lobby in AvailableLobbies)
				{
					lobby.IsRefreshing = value;
				}
			}
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
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand RefreshViewCommand
			=> _refreshViewCommand ?? (_refreshViewCommand = new DelegateCommand(Refresh));

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

		/// <summary>
		///     Visitor-Pattern, maybe? Updates the <see cref="AvailableLobbies" />
		/// </summary>
		public async void Refresh()
		{
			var gameLobbies = (await CurrentSession.Client.getGameLobbiesAsync()).@return;

			if (gameLobbies == null)
			{
				return;
			}

			AvailableLobbies.Clear();
			foreach (var lobby in gameLobbies)
			{
				var lobbyViewModel = Container.Resolve<LobbyViewModel>();

				lobbyViewModel.Lobby = lobby;

				AvailableLobbies.Add(lobbyViewModel);
			}
		}

		private void Logout()
		{
			CurrentSession.PollingTimer.Stop();

			IsRefreshing = false;

			var preSelectedUsername = CurrentSession.Player.Username;

			CurrentSession.Player = null;

			Settings.Default.SavedUser = null;
			Settings.Default.Save();

			var loginViewModel = new LoginViewModel {Username = preSelectedUsername};

			var loginView = new LoginView(loginViewModel);

			loginView.Show();

			RequestWindowCloseEvent(this, null);
		}

		private bool CanJoinLobby()
		{
			return SeletedLobby != null &&
			       (SeletedLobby.LobbyHasPassword && !string.IsNullOrEmpty(LobbyPassword) || !SeletedLobby.LobbyHasPassword);
		}

		private async void JoinLobby()
		{
			if (SeletedLobby == null) return;

			IsRefreshing = false;

			SeletedLobby.Lobby = (await CurrentSession.Client.joinGameLobbyAsync(CurrentSession.Player.PlayerId, SeletedLobby.LobbyId)).@return;

			SeletedLobby.IsRefreshing = true;

			new LobbyView(SeletedLobby).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		private async void CreateLobby()
		{
			IsRefreshing = false;

			var createdLobby =
				(await CurrentSession.Client.createGameLobbyAsync(CurrentSession.Player.PlayerId, "Random Name")).@return;

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			lobbyViewModel.Lobby = createdLobby;

			new LobbyView(lobbyViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}
	}
}