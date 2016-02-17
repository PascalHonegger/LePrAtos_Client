// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using LePrAtos.Infrastructure;

namespace LePrAtos.Service_References
{
	/// <summary>
	/// ViewModel für einen Spieler
	/// </summary>
	[Export(typeof(GameViewModel))]
	public class GameViewModel : ViewModelBase
	{
		/// <summary>
		/// Alle Mitglieder dieser Lobby
		/// </summary>
		private ObservableCollection<PlayerViewModel> Members => new ObservableCollection<PlayerViewModel>
		{
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel(),
			new PlayerViewModel()
		};

		/// <summary>
		/// Anzahl <see cref="Members"/> / maximum <see cref="MaxMemberCount"/> 
		/// </summary>
		public string MemberCount => $"{Members.Count} / {MaxMemberCount}";

		//TODO: Implementierung Server & Client
		private int MaxMemberCount => 13;

		/// <summary>
		/// Lobby ID
		/// </summary>
		public string LobbyName
		{
			get
			{
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
				var random = new Random();
				return new string(Enumerable.Repeat(chars, 20)
					.Select(s => s[random.Next(s.Length)]).ToArray());
			}
		}

		/// <summary>
		/// Lobby ID
		/// </summary>
		public string LobbyId
		{
			get
			{
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
				var random = new Random();
				return new string(Enumerable.Repeat(chars, 20)
					.Select(s => s[random.Next(s.Length)]).ToArray());
			}
		}
	}
}