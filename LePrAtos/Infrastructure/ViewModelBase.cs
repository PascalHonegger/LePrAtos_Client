// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel;
using System.Runtime.CompilerServices;
using LePrAtos.Properties;
using Microsoft.Practices.Unity;
using UnityContainer;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Base-Klasse für alle ViewModels
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
	{
		private ISession _currentSession;

		/// <summary>
		///     Die jetzige Instanz der Session
		/// </summary>
		public ISession CurrentSession => _currentSession ?? (_currentSession = Container.Resolve<ISession>());

		/// <summary>
		///     Der UnityContainer
		/// </summary>
		protected static IUnityContainer Container => ContainerProvider.Container;

		/// <summary>
		///     Tritt ein, wenn sich ein Eigenschaftswert ändert.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Notifies the GUI, that the Property was changed
		/// </summary>
		/// <param name="propertyName">The Property, that changed</param>
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}