// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using LePrAtos.Infrastructure;
using LePrAtos.Service_References;

namespace LePrAtos.Lobby
{
	/// <summary>
	///     ViewModel für <see cref="LobbyView" />
	/// </summary>
	[Export(typeof (LobbyViewModel))]
	public class LobbyViewModel : ViewModelBase
	{
		/// <summary>
		///     Alle Mitglieder dieser Lobby
		/// </summary>
		public ObservableCollection<PlayerViewModel> Members { get; } = new ObservableCollection<PlayerViewModel>();

		/// <summary>
		///     Anzahl <see cref="Members" /> / maximum <see cref="MaxMemberCount" />
		/// </summary>
		public string MemberCount => $"{Members.Count} / {MaxMemberCount}";

		//TODO: Implementierung Server & Client
		private int MaxMemberCount => 13;

		/// <summary>
		///     Lobby ID
		/// </summary>
		public string LobbyName { get; set; }

		/// <summary>
		///     Lobby ID
		/// </summary>
		public string LobbyId { get; set; }
	}
}