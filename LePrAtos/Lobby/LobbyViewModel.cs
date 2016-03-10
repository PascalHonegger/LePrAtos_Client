// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
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
				Application.Current.Dispatcher.InvokeAsync(Refresh);
			};
		}

		private async Task Refresh()
		{
			if (IsRefreshing)
			{
				Lobby = (await CurrentSession.Client.getGameLobbyAsync(LobbyId)).@return;
			}
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
		public bool LobbyHasPassword { get; set; }
		
		/// <summary>
		///     Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand LeaveLobbyCommand => _leaveLobbyCommand ?? (_leaveLobbyCommand = new DelegateCommand(() => RequestWindowCloseEvent.Invoke(this, null)));

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
				LobbyName = _lobby.gameLobbyName;

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

				if (_lobby.gamePlayerList != null)
				{
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
						}

						Members.Add(playerViewModel);
					}
				}

				LobbyHasPassword = false;
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

			lobbyBrowserViewModel.IsRefreshing = true;

			lobbyBrowserViewModel.Refresh();

			var lobbyBrowserView = new LobbyBrowserView(lobbyBrowserViewModel);

			lobbyBrowserView.Show();

			return true;
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