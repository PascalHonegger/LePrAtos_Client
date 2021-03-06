﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using LePrAtos.Properties;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Standardimplementation des BusyRunners
	/// </summary>
	[Export(typeof(IBusyRunner))]
	public sealed class BusyRunner : IBusyRunner
	{
		private bool _isBusy;

		/// <summary>
		///     Sagt aus, ob das ViewModel gerade beschäftigt ist (Bsp. Serverabfrage)
		/// </summary>
		public bool IsBusy
		{
			get { return _isBusy; }
			private set
			{
				if (_isBusy == value)
				{
					return;
				}

				_isBusy = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Führt die <paramref name="asyncOperation"/> aus und setzt <see cref="IBusyRunner.IsBusy"/> während der Ausführung auf True
		/// </summary>
		/// <param name="asyncOperation">Auszuführende Operation</param>
		/// <param name="errorMessage">Nachricht, welche bei Fehler angezeigt wird</param>
		public async void RunAsync(Func<Task> asyncOperation, string errorMessage = null)
		{
			try
			{
				IsBusy = true;

				await asyncOperation();
			}
			catch (Exception e)
			{
				DisplayError(e, errorMessage);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private static void DisplayError(Exception e, string customErrorMessage)
		{
			var exceptionMessage = typeof(Strings).GetProperty(e.Message)?.GetValue(typeof(Strings), null) as string ?? e.Message;
			MessageBox.Show(string.IsNullOrEmpty(customErrorMessage) ? exceptionMessage : customErrorMessage, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		///    Führt die <paramref name="asyncOperation"/> aus.
		/// </summary>
		/// <param name="asyncOperation">Auszuführende Operation</param>
		/// <param name="errorMessage">Nachricht, welche bei Fehler angezeigt wird</param>
		public async void RunSilent(Func<Task> asyncOperation, string errorMessage = null)
		{
			try
			{
				await asyncOperation();
			}
			catch (Exception e)
			{
				DisplayError(e, errorMessage);
			}
		}

		/// <summary>
		///     Tritt ein, wenn sich ein Eigenschaftswert ändert.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Notifies the GUI, that the Property has changed
		/// </summary>
		/// <param name="propertyName">The Property, that changed</param>
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}