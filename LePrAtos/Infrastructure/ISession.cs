// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

namespace LePrAtos.Infrastructure
{
	/// <summary>
	/// Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die <see cref="Endpointconfiguration"/>
	/// </summary>
	public interface ISession
	{
		/// <summary>
		/// Die gewählte Endpunktkonfiguration
		/// </summary>
		string Endpointconfiguration { get; set; }
	}
}