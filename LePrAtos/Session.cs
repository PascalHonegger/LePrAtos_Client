// Projekt: LePrAtos
// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

namespace LePrAtos
{
	/// <summary>
	/// Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die <see cref="Endpointconfiguration"/>
	/// </summary>
	public class Session
	{
		private static Session _session;
		/// <summary>
		/// Singleton
		/// </summary>
		public static Session Instance => _session ?? (_session = new Session());
		private Session()
		{
			
		}

		/// <summary>
		/// Die gewählte Endpunktkonfiguration
		/// </summary>
		public string Endpointconfiguration { get; set; }
	}
}
