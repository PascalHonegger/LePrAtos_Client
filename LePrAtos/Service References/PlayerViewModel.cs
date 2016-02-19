// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel.Composition;
using System.Linq;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;

namespace LePrAtos.Service_References
{
	/// <summary>
	/// ViewModel für einen Spieler
	/// </summary>
	[Export(typeof(PlayerViewModel))]
	public class PlayerViewModel : ViewModelBase
	{
		private player _player;

		/// <summary>
		/// Gewählter Benutzername
		/// </summary>
		public string Username { get; private set; }

		/// <summary>
		/// Die Spieler ID
		/// </summary>
		public string PlayerId { get; private set; }

		/// <summary>
		/// Der vom Server stammende Player
		/// </summary>
		public player Player
		{
			get { return _player; }
			set
			{
				if (Equals(_player, value))
				{
					return;
				}
				_player = value;
				PlayerId = _player.uuid;
				Username = _player.username;
			}
		}
	}
}