// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel.Composition;
using LePrAtos.Infrastructure;

namespace LePrAtos
{
	/// <summary>
	/// Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die <see cref="Endpointconfiguration"/>
	/// </summary>
	[Export(typeof(ISession))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class Session : ISession
	{
		/// <summary>
		/// Die gewählte Endpunktkonfiguration
		/// </summary>
		public string Endpointconfiguration { get; set; }
	}
}
