// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Interface für den BusyRunner
	/// </summary>
	public interface IBusyRunner : INotifyPropertyChanged
	{
		/// <summary>
		///     True, falls eine Aktion ausgeführt wird.
		/// </summary>
		bool IsBusy { get; }

		/// <summary>
		///     Führt die <paramref name="asyncOperation"/> aus und setzt <see cref="IsBusy"/> während der Ausführung auf True
		/// </summary>
		/// <param name="asyncOperation">Auszuführende Operation</param>
		/// <param name="errorMessage">Nachricht, welche bei Fehler angezeigt wird</param>
		void RunAsync(Func<Task> asyncOperation, string errorMessage = null);

		/// <summary>
		///    Führt die <paramref name="asyncOperation"/> aus.
		/// </summary>
		/// <param name="asyncOperation">Auszuführende Operation</param>
		/// <param name="errorMessage">Nachricht, welche bei Fehler angezeigt wird</param>
		void RunSilent(Func<Task> asyncOperation, string errorMessage = null);
	}
}