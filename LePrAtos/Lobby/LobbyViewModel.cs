// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

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
	///     ViewModel für <see cref="LobbyView" />
	/// </summary>
	[Export(typeof (LobbyViewModel))]
	public class LobbyViewModel : ViewModelBase, IRequestWindowClose
	{
		private ICommand _joinLobbyCommand;

		/// <summary>
		///     Constructor
		/// </summary>
		public LobbyViewModel()
		{
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
		public PlayerViewModel LobbyLeader { get; set; }

		/// <summary>
		/// Command zum beitreten der ausgewählten Lobby
		/// </summary>
		public ICommand LeaveLobbyCommand => _joinLobbyCommand ?? (_joinLobbyCommand = new DelegateCommand(LeaveLobby));

		private void LeaveLobby()
		{
			var lobbyBrowserViewModel = Container.Resolve<LobbyBrowserViewModel>();
			
			var lobbyBrowserView = new LobbyBrowserView(lobbyBrowserViewModel);

			lobbyBrowserView.Show();

			RequestWindowCloseEvent.Invoke(this, null);
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }
	}
}