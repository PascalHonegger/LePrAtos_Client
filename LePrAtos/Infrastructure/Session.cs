// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel.Composition;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die
	///     <see cref="Endpointconfiguration" />
	/// </summary>
	[Export(typeof (ISession))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class Session : ISession
	{
		/// <summary>
		///     Die gewählte Endpunktkonfiguration
		/// </summary>
		public string Endpointconfiguration { get; set; }

		/// <summary>
		///     Der angemeldete Spieler
		/// </summary>
		public PlayerViewModel Player { get; set; }
	}
}