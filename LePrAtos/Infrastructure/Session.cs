// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel.Composition;
using System.Timers;
using LePrAtos.GameManagerService;
using LePrAtos.Service_References;
using UnityContainer;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Diese Klasse enthält informationen zur jetzigen Session, wie beispielsweise die
	///     <see cref="Endpointconfiguration" />
	/// </summary>
	[Export(typeof (ISession))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public sealed class Session : ISession
	{
		private const int PollingInterval = 2000;
		private string _endpointconfiguration;

		/// <summary>
		///     Setzt den <see cref="PollingTimer"/>
		/// </summary>
		public Session()
		{
			PollingTimer = new Timer(PollingInterval);

			ContainerProvider.ContainerReset += () =>
			{
				Player = null;
				PollingTimer?.Stop();
			};
		}

		/// <summary>
		///     Die gewählte Endpunktkonfiguration
		/// </summary>
		public string Endpointconfiguration
		{
			set
			{
				if (Equals(_endpointconfiguration, value))
				{
					return;
				}
				_endpointconfiguration = value;
				Client = new GameManagerClient(_endpointconfiguration);
			}
		}

		/// <summary>
		///     Der angemeldete Spieler
		/// </summary>
		public PlayerViewModel Player { get; set; }

		/// <summary>
		///     Der Client für die Kommunikation mit dem Server
		/// </summary>
		public GameManagerClient Client { get; private set; }

		/// <summary>
		///     Der Timer, welcher allen Services sagt, dass sie erneut daten vom Server laden sollten
		/// </summary>
		public Timer PollingTimer { get; }

		/// <summary>
		/// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
		/// </summary>
		/// <param name="disposing">Boolean ob Managed oder Native Resources freigegeben werden.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<PollingTimer>k__BackingField", Justification = "Code Analysis Rule CA2213 DisposableFieldsShouldBeDisposed doesn't understand read only auto properties being disposed. ")]
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
			}

			// free native resources

			PollingTimer.Dispose();
			Client.Close();
		}

		/// <summary>
		/// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}