﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
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
		private bool _isPasswordProtected;
		private ICommand _leaveLobbyCommand;
		private gameLobby _lobby;
		private string _lobbyName;
		private string _newLobbyName;
		private string _newLobbyPassword;
		private int _newPlayerLimit;
		private int _playerLimit;
		private DelegateCommand _removePasswordCommand;
		private DelegateCommand _startGameCommand;
		private DelegateCommand _updateSettingsCommand;

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyViewModel()
		{
			// ReSharper disable once ExplicitCallerInfoArgument
			Members.CollectionChanged += (sender, e) => { OnPropertyChanged(nameof(LobbyLeaderName)); };
			StartUpdate();
		}

		/// <summary>
		///     Der Name des Lobby-Leiters. Benutzt für die Lobby-Übersicht
		/// </summary>
		public string LobbyLeaderName => Members.FirstOrDefault(m => m.IsLeader)?.Username;

		/// <summary>
		///     Alle Mitglieder dieser Lobby
		/// </summary>
		public ObservableCollection<PlayerViewModel> Members { get; } = new ObservableCollection<PlayerViewModel>();

		/// <summary>
		///     Das Limit an Spielern.
		/// </summary>
		public int PlayerLimit
		{
			get { return _playerLimit; }
			private set
			{
				if (Equals(_playerLimit, value)) return;
				NewPlayerLimit = _playerLimit = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Lobby Name
		/// </summary>
		public string LobbyName
		{
			get { return _lobbyName; }
			set
			{
				if (Equals(value, _lobbyName)) return;
				NewLobbyName = _lobbyName = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Lobby ID
		/// </summary>
		public string LobbyId { get; private set; }

		/// <summary>
		///     Lobby verfügt über ein Passwort
		/// </summary>
		public bool IsPasswordProtected
		{
			get { return _isPasswordProtected; }
			set
			{
				if (Equals(value, _isPasswordProtected)) return;
				_isPasswordProtected = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PasswordDisplayValue));
			}
		}

		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand LeaveLobbyCommand
			=> _leaveLobbyCommand ?? (_leaveLobbyCommand = new DelegateCommand(() => RequestWindowCloseEvent.Invoke(this, null)))
			;

		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand StartGameCommand
			=> _startGameCommand ?? (_startGameCommand = new DelegateCommand(StartGame, CanStartGame));

		/// <summary>
		///     Command zum übernehmen der neu gesetzten Einstellungen für die Lobby
		/// </summary>
		public DelegateCommand UpdateSettingsCommand
			=> _updateSettingsCommand ?? (_updateSettingsCommand = new DelegateCommand(UpdateSettings, CanUpdateSettings));

		/// <summary>
		///     Command zum übernehmen der neu gesetzten Einstellungen für die Lobby
		/// </summary>
		public DelegateCommand RemovePasswordCommand
			=> _removePasswordCommand ?? (_removePasswordCommand = new DelegateCommand(RemovePassword, CanRemovePassword));

		/// <summary>
		///     Die vom Server stammende Lobby
		/// </summary>
		public gameLobby Lobby
		{
			set
			{
				if (value?.gameLobbyAdmin == null)
				{
					StopUpdate();
					return;
				}

				_lobby = value;
				LobbyId = _lobby.gameLobbyID;
				LobbyName = _lobby.gameLobbyName;
				PlayerLimit = _lobby.playerLimit;
				//TODO IsPasswordProtected = _lobby.gameLobbyHasPassword;

				Members.Clear();

				PlayerViewModel admin;
				if (Equals(_lobby.gameLobbyAdmin.username, CurrentSession.Player.Identification.username))
				{
					CurrentSession.Player.Identification = _lobby.gameLobbyAdmin;
					admin = CurrentSession.Player;
				}
				else
				{
					admin = Container.Resolve<PlayerViewModel>();
					admin.Identification = _lobby.gameLobbyAdmin;
				}

				admin.IsLeader = true;

				Members.Add(admin);

				if (_lobby.gamePlayerList == null) return;

				foreach (var playerIdentification in _lobby.gamePlayerList)
				{
					PlayerViewModel playerViewModel;

					if (Equals(playerIdentification, CurrentSession.Player.Identification))
					{
						CurrentSession.Player.Identification = playerIdentification;
						playerViewModel = CurrentSession.Player;
					}
					else
					{
						playerViewModel = Container.Resolve<PlayerViewModel>();
						playerViewModel.Identification = playerIdentification;
						playerViewModel.RemoveAction = RemovePlayer;
					}

					playerViewModel.IsLeader = false;
					Members.Add(playerViewModel);
				}
			}
		}

		/// <summary>
		///     Der Text, welcher im GUI als Passwort-Platzhalter angezeigt wird.
		/// </summary>
		public string PasswordDisplayValue => IsPasswordProtected ? "*****" : string.Empty;

		/// <summary>
		///     Das neue Spieler-Limit
		/// </summary>
		public int NewPlayerLimit
		{
			get { return _newPlayerLimit; }
			set
			{
				if (Equals(_newPlayerLimit, value)) return;
				_newPlayerLimit = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Das neue Lobby-Passwort
		/// </summary>
		public string NewLobbyPassword
		{
			get { return _newLobbyPassword; }
			set
			{
				if (Equals(_newLobbyPassword, value)) return;
				_newLobbyPassword = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Der neue Lobby-Name
		/// </summary>
		public string NewLobbyName
		{
			get { return _newLobbyName; }
			set
			{
				if (Equals(_newLobbyName, value)) return;
				_newLobbyName = value;
				SetErrorForProperty(string.IsNullOrEmpty(_newLobbyName) ? Strings.TextValidationRule_Mandatory : string.Empty);

				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		private bool CanRemovePassword()
		{
			throw new NotImplementedException();
		}

		private async void RemovePassword()
		{
			IsBusy = true;

			try
			{
				await CurrentSession.Client.setGameLobbyPasswordAsync(CurrentSession.Player.PlayerId, LobbyId, string.Empty);

				await Refresh();
			}
			finally
			{
				IsBusy = false;
			}
		}

		private void StartUpdate()
		{
			CurrentSession.PollingTimer.Elapsed += PollingTimerOnElapsed;
		}

		/// <summary>
		///     Trennt die <see cref="Refresh" /> Funktion vom Timer
		/// </summary>
		public void StopUpdate()
		{
			CurrentSession.PollingTimer.Elapsed -= PollingTimerOnElapsed;
		}

		private void PollingTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			Application.Current.Dispatcher.InvokeAsync(Refresh);
		}

		private async Task Refresh()
		{
			Lobby = (await CurrentSession.Client.getGameLobbyAsync(LobbyId)).@return;
		}

		private async void UpdateSettings()
		{
			IsBusy = true;

			try
			{
				if (!Equals(NewLobbyName, LobbyName))
				{
					await CurrentSession.Client.setGameLobbyNameAsync(CurrentSession.Player.PlayerId, LobbyId, NewLobbyName);
				}

				if (!string.IsNullOrEmpty(NewLobbyPassword))
				{
					await CurrentSession.Client.setGameLobbyPasswordAsync(CurrentSession.Player.PlayerId, LobbyId, NewLobbyPassword);
				}

				if (!Equals(NewPlayerLimit, PlayerLimit))
				{
					await CurrentSession.Client.setPlayerLimitAsync(CurrentSession.Player.PlayerId, LobbyId, NewPlayerLimit);
				}

				await Refresh();
			}
			finally
			{
				IsBusy = false;
			}
		}

		private bool CanUpdateSettings()
		{
			return CurrentSession.Player.IsLeader;
		}

		private bool CanStartGame()
		{
			return Members.All(p => p.IsReady);
		}

		private void StartGame()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Verlässt die Lobby, nachdem der User darüber benachrichtigt wurde
		/// </summary>
		/// <returns><c>True</c> bei Success</returns>
		public bool LeaveLobby()
		{
			var result = MessageBox.Show(Strings.LobbyView_ReallyQuit, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (result == MessageBoxResult.No) return false;

			CurrentSession.Client.leaveGameLobby(CurrentSession.Player.PlayerId, LobbyId);

			var lobbyBrowserViewModel = Container.Resolve<LobbyBrowserViewModel>();

			lobbyBrowserViewModel.Refresh();

			var lobbyBrowserView = new LobbyBrowserView(lobbyBrowserViewModel);

			lobbyBrowserView.Show();
			return true;
		}

		private void RemovePlayer(PlayerViewModel player)
		{
			CurrentSession.Client.kickPlayer(CurrentSession.Player.PlayerId, LobbyId, player.Username);
		}

		private bool Equals(LobbyViewModel other)
		{
			return string.Equals(LobbyId, other.LobbyId);
		}

		/// <summary>
		///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((LobbyViewModel) obj);
		}

		/// <summary>
		///     Fungiert als die Standardhashfunktion.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return LobbyId?.GetHashCode() ?? 0;
		}
	}
}