// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel.Composition;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;

namespace LePrAtos.Service_References
{
	/// <summary>
	///     ViewModel für einen Spieler
	/// </summary>
	[Export(typeof (PlayerViewModel))]
	public class PlayerViewModel : ViewModelBase
	{
		private bool _isReady;
		private player _player;

		/// <summary>
		///     Gewählter Benutzername
		/// </summary>
		public string Username { get; private set; }

		/// <summary>
		///     Die Spieler ID
		/// </summary>
		public string PlayerId { get; private set; }

		/// <summary>
		///     Der vom Server stammende Player
		/// </summary>
		public player Player
		{
			set
			{
				if (Equals(_player, value))
				{
					return;
				}
				_player = value;
				PlayerId = _player.playerID;
				Username = _player.username;
				IsReady = false;
				IsLeader = false;
			}
		}

		/// <summary>
		///     True, wenn der Spieler bereit ist, das jetzige Spiel zu starten
		/// </summary>
		public bool IsReady
		{
			get { return _isReady; }
			set
			{
				if (Equals(_isReady, value))
				{
					return;
				}
				//TODO Say Server I'm ready
				_isReady = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     True, wenn der Spieler ein Lobby-Leiter ist. Da er nur in einem Spiel zur gleichen zeit sein kann, wird dies zur
		///     bestimmung des Leiters pro Lobby verwendet
		/// </summary>
		public bool IsLeader { get; set; }

		private bool Equals(PlayerViewModel other)
		{
			return string.Equals(Username, other.Username);
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
			return obj.GetType() == GetType() && Equals((PlayerViewModel) obj);
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
			return Username?.GetHashCode() ?? 0;
		}
	}
}