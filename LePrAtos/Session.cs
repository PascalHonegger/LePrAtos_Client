// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePrAtos
{
	/// <summary>
	/// TODO
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

		public string Endpointconfiguration { get; set; }
	}
}
