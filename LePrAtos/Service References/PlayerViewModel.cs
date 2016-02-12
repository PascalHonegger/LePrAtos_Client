// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Linq;
using LePrAtos.Infrastructure;

namespace LePrAtos.Service_References
{
	/// <summary>
	/// ViewModel für einen Spieler
	/// </summary>
	public class PlayerViewModel : ViewModelBase
	{
		/// <summary>
		/// Gewählter Benutzername
		/// </summary>
		public string Username
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
		/// Die Spieler ID
		/// </summary>
		public string PlayerId
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