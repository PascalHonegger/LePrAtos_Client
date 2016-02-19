// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel.Composition;
using System.Linq;
using LePrAtos.Infrastructure;

namespace LePrAtos.Service_References
{
	/// <summary>
	/// ViewModel f�r einen Spieler
	/// </summary>
	[Export(typeof(PlayerViewModel))]
	public class PlayerViewModel : ViewModelBase
	{
		/// <summary>
		/// Gew�hlter Benutzername
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Die Spieler ID
		/// </summary>
		public string PlayerId { get; set; }
	}
}