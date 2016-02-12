// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)
using System;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRequestDialogCloseViewModel
	{
		/// <summary>
		/// Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		EventHandler RequestDialogCloseEventHandler { get; set; }
	}
}