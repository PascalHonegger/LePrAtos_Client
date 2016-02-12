// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Implementationen können die dazugehörige View anfragen, sich zu schliessen
	/// </summary>
	public interface IRequestWindowClose
	{
		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		EventHandler RequestWindowCloseEvent { get; set; }
	}
}