// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using LePrAtos.Infrastructure.Extensions;
using LePrAtos.Startup;
using Exception = System.Exception;

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
			LobbyPassword = string.Empty;
		}

		/// <summary>
		///     Entscheidet, ob die Lobbies neue Daten vom Server laden
		/// </summary>
		private void StopRefresh(bool ignoreSelectedLobby = true)
		{
			foreach (var lobby in _availableLobbies.Where(l => Equals(l, SeletedLobby) != ignoreSelectedLobby))
			{
				lobby.StopUpdate();
			}
		}

		/// <summary>
		///     Entscheidet, ob die Lobbies neue Daten vom Server laden
		/// </summary>
		private void StartRefresh()
		{
			StopRefresh(false);

			foreach (var lobby in _availableLobbies)
			{
				lobby.StartUpdate();
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

		private IEnumerable<LobbyViewModel> _availableLobbies = new List<LobbyViewModel>();
		private bool _showProtectedLobbies = true;
		private bool _showFullLobbies;
		private string _searchText = string.Empty;

		/// <summary>
		///     Alle verfügbaren Lobbies
		/// </summary>
		public ObservableCollection<LobbyViewModel> FilteredLobbies { get; } = new ObservableCollection<LobbyViewModel>();

		private void ApplyLobbyFilter()
		{
			var filteredLobbies = _availableLobbies
				.Where(l => ShowProtectedLobbies || !l.PasswordProtected)
				.Where(l => ShowFullLobbies || l.PlayerLimit > l.Members.Count)
				.Where(
					l =>
						l.LobbyName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
						l.LobbyLeaderName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));

			FilteredLobbies.Clear();

			foreach (var lobby in filteredLobbies)
			{
				FilteredLobbies.Add(lobby);
			}
		}

		/// <summary>
		///     Passwort, welches für das beitreten in die Lobby verwendet wird
		/// </summary>
		public string LobbyPassword
		{
			get { return _lobbyPassword; }
			set
			{
				_lobbyPassword = value;

				if (SeletedLobby != null && SeletedLobby.PasswordProtected && string.IsNullOrEmpty(_lobbyPassword))
				{
					SetErrorForProperty(Strings.TextValidationRule_Mandatory);
				}
				else
				{
					SetErrorForProperty(string.Empty);
				}

				OnPropertyChanged();
				JoinLobbyCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Filtert die Lobbies für die View
		/// </summary>
		public bool ShowProtectedLobbies
		{
			get { return _showProtectedLobbies; }
			set
			{
				_showProtectedLobbies = value;
				ApplyLobbyFilter();
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Filtert die Lobbies für die View
		/// </summary>
		public bool ShowFullLobbies
		{
			get { return _showFullLobbies; }
			set
			{
				_showFullLobbies = value;
				ApplyLobbyFilter();
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Filtert die Lobbies für die View
		/// </summary>
		public string SearchText
		{
			get { return _searchText; }
			set
			{
				_searchText = value;
				ApplyLobbyFilter();
				OnPropertyChanged();
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

				LobbyPassword = _lobbyPassword;
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Visitor-Pattern, maybe? Updates the <see cref="_availableLobbies" />
		/// </summary>
		public void Refresh()
		{
			BusyRunner.RunAsync(async () =>
			{
				var gameLobbies = (await CurrentSession.Client.getGameLobbiesAsync()).@return;

				if (gameLobbies == null)
				{
					return;
				}

				_availableLobbies = gameLobbies.Where(l => l?.gameLobbyAdmin != null && l.gameLobbyID != null).Select(lobby =>
				{
					var lobbyViewModel = Container.Resolve<LobbyViewModel>();
					lobbyViewModel.Lobby = lobby;
					return lobbyViewModel;
				});

				ApplyLobbyFilter();
			});
		}

	private void Logout()
		{
			CurrentSession.PollingTimer.Stop();

			StopRefresh(false);

			var preSelectedUsername = CurrentSession.Player.Username;

			CurrentSession.Player = null;

			Settings.Default.SavedUser = null;
			Settings.Default.Save();

			var loginViewModel = new LoginViewModel {UsernameOrMail = preSelectedUsername};

			var loginView = new LoginView(loginViewModel);

			loginView.Show();

			RequestWindowCloseEvent(this, null);
		}

		private bool CanJoinLobby()
		{
			return SeletedLobby != null && !HasErrors;
		}

		private void JoinLobby()
		{
			if (SeletedLobby == null) return;

			BusyRunner.RunAsync(async () =>
			{
				StopRefresh();
				SeletedLobby.Lobby =
					(await
						CurrentSession.Client.joinGameLobbyAsync(CurrentSession.Player.PlayerId, SeletedLobby.LobbyId,
							PasswordHasher.HashPasswort(LobbyPassword))).@return;
				new LobbyView(SeletedLobby).Show();
				RequestWindowCloseEvent.Invoke(this, null);
			}, Strings.LobbyBrowser_WrongLobbyPassword);
		}

		private async void CreateLobby()
		{
			StopRefresh();

			var createdLobby =
				(await CurrentSession.Client.createGameLobbyAsync(CurrentSession.Player.PlayerId, string.Format(Strings.LobbyBrowser_CreatedByTempalte, CurrentSession.Player.Username))).@return;

			var lobbyViewModel = Container.Resolve<LobbyViewModel>();

			lobbyViewModel.Lobby = createdLobby;

			new LobbyView(lobbyViewModel).Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}
	}
}