// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel.Composition;
using LePrAtos.GameManagerService;
using LePrAtos.Infrastructure;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Service_References
{
	/// <summary>
	///     ViewModel f�r einen Spieler
	/// </summary>
	[Export(typeof (PlayerViewModel))]
	public class PlayerViewModel : ViewModelBase
	{
		private bool _isReady;
		private playerIdentification _identification;
		private bool _isLeader;
		private DelegateCommand _removeCommand;
		
		/// <summary>
		///     Benutzt f�r <see cref="RemoveCommand"/>
		/// </summary>
		public Action<PlayerViewModel> RemoveAction;

		/// <summary>
		///     Gew�hlter Benutzername
		/// </summary>
		public string Username { get; private set; }

		/// <summary>
		///     Die Spieler ID
		/// </summary>
		public string PlayerId { get; private set; }

		/// <summary>
		///     Der vom Server stammende Player
		/// </summary>
		public playerIdentification Identification
		{
			get { return _identification; }
			set
			{
				if (Equals(_identification, value))
				{
					return;
				}
				_identification = value;
				Username = _identification.username;
				_isReady = _identification.status;
			}
		}

		/// <summary>
		///     Der vom Server stammende Player
		/// </summary>
		public player Player
		{
			set
			{
				Identification = value;
				PlayerId = value.playerID;
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

				if (Equals(this, CurrentSession.Player))
				{
					CurrentSession.Client.setPlayerStatus(PlayerId, value);
				}

				_isReady = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     True, wenn der Spieler ein Lobby-Leiter ist. Da er nur in einem Spiel zur gleichen zeit sein kann, wird dies zur
		///     bestimmung des Leiters pro Lobby verwendet
		/// </summary>
		public bool IsLeader
		{
			get { return _isLeader; }
			set
			{
				if (value == _isLeader) return;

				_isLeader = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Command zum entfernen dieses Spieler aus seiner Lobby. F�hrt die <see cref="RemoveAction"/> aus.
		/// </summary>
		public DelegateCommand RemoveCommand
		{
			get
			{
				return _removeCommand ??
				       (_removeCommand = new DelegateCommand(() => RemoveAction?.Invoke(this), () => RemoveAction != null && CurrentSession.Player.IsLeader));
			}
		}

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
		///     Ein Hashcode f�r das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Username?.GetHashCode() ?? 0;
		}
	}
}