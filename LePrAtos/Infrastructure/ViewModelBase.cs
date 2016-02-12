﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel;
using System.Runtime.CompilerServices;
using LePrAtos.Properties;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	/// Base-Klasse für alle ViewModels
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
	{
		/// <summary>
		/// Tritt ein, wenn sich ein Eigenschaftswert ändert.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the GUI, that the Property was changed
		/// </summary>
		/// <param name="propertyName">The Property, that changed</param>
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Die jetzige Instanz der Session
		/// </summary>
		[Microsoft.Practices.Unity.Dependency]
		protected ISession CurrentSession { get; set; }
	}
}