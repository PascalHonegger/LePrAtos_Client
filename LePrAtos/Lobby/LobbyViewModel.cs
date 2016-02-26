// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
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
		private ICommand _leaveLobbyCommand;
		private gameLobby _lobby;
		private DelegateCommand _startGameCommand;

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyViewModel()
		{
			// ReSharper disable once ExplicitCallerInfoArgument
			Members.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(MemberCount));
			CurrentSession.PollingTimer.Elapsed += (sender, e) =>
			{
				App.Current.Dispatcher.InvokeAsync(Reload);
			};
		}

		private async Task Reload()
		{
			if (!IsRefreshing)
			{
				return;
			}
			Lobby = (await CurrentSession.Client.getGameLobbyAsync(LobbyId)).@return;
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
		public string LobbyId { get; private set; }

		/// <summary>
		///     Lobby verfügt über ein Passwort
		/// </summary>
		public bool HasLobbyPassword { get; set; }

		/// <summary>
		///     Der Leiter der Lobby, darf beispielsweise leute aus der Lobby entfernen
		/// </summary>
		private string LobbyLeaderName { get; set; }

		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand LeaveLobbyCommand => _leaveLobbyCommand ?? (_leaveLobbyCommand = new DelegateCommand(LeaveLobby));

		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public DelegateCommand StartGameCommand
			=> _startGameCommand ?? (_startGameCommand = new DelegateCommand(StartGame, CanStartGame));

		/// <summary>
		///     Die vom Server stammende Lobby
		/// </summary>
		public gameLobby Lobby
		{
			set
			{
				_lobby = value;
				LobbyId = _lobby.gameLobbyID;
				LobbyLeaderName = _lobby.gameLobbyAdmin;
				LobbyName = _lobby.gameLobbyName;

				Members.Clear();
				if (_lobby.gamePlayerListPublic != null)
				{
					foreach (var playerName in _lobby.gamePlayerListPublic)
					{
						var playerViewModel = Container.Resolve<PlayerViewModel>();
						playerViewModel.Player = new player { username = playerName };
						if (Equals(LobbyLeaderName, playerViewModel.Username))
						{
							playerViewModel.IsLeader = true;
						}
						Members.Add(playerViewModel);
					}
				}

				HasLobbyPassword = false;
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }

		/// <summary>
		///     Entscheided, ob sich der bool refreshed
		/// </summary>
		public bool IsRefreshing { private get; set; } = true;

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
			CurrentSession.Client.leaveGameLobby(CurrentSession.Player.PlayerId, LobbyId);

			var lobbyBrowserViewModel = Container.Resolve<LobbyBrowserViewModel>();

			lobbyBrowserViewModel.IsRefreshing = true;

			lobbyBrowserViewModel.Refresh();

			var lobbyBrowserView = new LobbyBrowserView(lobbyBrowserViewModel);

			lobbyBrowserView.Show();

			RequestWindowCloseEvent.Invoke(this, null);
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