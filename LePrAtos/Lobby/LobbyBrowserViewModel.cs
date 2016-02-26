// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using LePrAtos.GameManagerService;
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
	public class LobbyBrowserViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _createLobbyCommand;
		private DelegateCommand _joinLobbyCommand;
		private string _lobbyPassword;
		private ICommand _logoutCommand;
		private LobbyViewModel _seletedLobby;

		/// <summary>
		///     Visitor-Pattern, maybe? Updates the <see cref="AvailableLobbies"/>
		/// </summary>
		private void Refresh()
		{
			var gameLobbies = CurrentSession.Client.getGameLobbies();

			AvailableLobbies.Clear();
			foreach (var lobby in gameLobbies.Cast<gameLobby>())
			{
				var lobbyViewModel = Container.Resolve<LobbyViewModel>();

				lobbyViewModel.Lobby = lobby;

				AvailableLobbies.Add(lobbyViewModel);
			}
		}

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyBrowserViewModel()
		{
			Refresh();
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
			Settings.Default.Save();

			var loginViewModel = new LoginViewModel {Username = preSelectedUsername};

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
			SeletedLobby.Lobby = CurrentSession.Client.joinGameLobby(CurrentSession.Player.PlayerId, SeletedLobby.LobbyId);

			new LobbyView(SeletedLobby).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		private async void CreateLobby()
		{
			var createdLobby = (await CurrentSession.Client.createGameLobbyAsync(CurrentSession.Player.PlayerId)).@return;

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			lobbyViewModel.Lobby = createdLobby;

			new LobbyView(lobbyViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}
	}
}