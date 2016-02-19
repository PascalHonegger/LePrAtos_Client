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
	/// ViewModel für einen Spieler
	/// </summary>
	[Export(typeof(PlayerViewModel))]
	public class PlayerViewModel : ViewModelBase
	{
		/// <summary>
		/// Gewählter Benutzername
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Die Spieler ID
		/// </summary>
		public string PlayerId { get; set; }
	}
}