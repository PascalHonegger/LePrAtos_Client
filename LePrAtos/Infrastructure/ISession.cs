// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using LePrAtos.GameManagerService;
using LePrAtos.Service_References;
using System.Timers;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die
	///     <see cref="Endpointconfiguration" /> und den angemeldeten <see cref="Player"/>
	/// </summary>
	public interface ISession : IDisposable
	{
		/// <summary>
		///     Die gewählte Endpunktkonfiguration
		/// </summary>
		string Endpointconfiguration { set; }

		/// <summary>
		///     Der angemeldete Spieler
		/// </summary>
		PlayerViewModel Player { get; set; }

		/// <summary>
		///     Der Client für die Kommunikation mit dem Server
		/// </summary>
		GameManagerClient Client { get; }

		/// <summary>
		///     Der Timer, welcher allen Services sagt, dass sie erneut daten vom Server laden sollten
		/// </summary>
		Timer PollingTimer { get; }
	}
}