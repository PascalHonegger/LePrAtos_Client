// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LePrAtos.Properties;
using Microsoft.Practices.Unity;
using UnityContainer;

namespace LePrAtos.Infrastructure
{
	/// <summary>
	///     Base-Klasse für alle ViewModels
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo, IViewModel
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

		#region INotifyDataErrorInfo

		private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

		/// <summary>
		///     Setzt die Fehler einer Property
		/// </summary>
		/// <param name="errors">Die zu setzenden Fehler</param>
		/// <param name="property">Das Property, bei welchem die Fehler gesetzt werden</param>
		protected void SetErrorForProperty(List<string> errors, [CallerMemberName] string property = null)
		{
			if (property == null || errors == null) return;

			List<string> existingErrors;
			if (_propertyErrors.TryGetValue(property, out existingErrors))
			{
				existingErrors.Clear();
				existingErrors.AddRange(errors);
			}
			else
			{
				_propertyErrors.Add(property, errors);
			}
		}

		/// <summary>
		///     Tritt auf, wenn sich die Validierungsfehler für eine Eigenschaft oder die gesamte Entität geändert haben.
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		///     Ruft die Validierungsfehler für eine angegebene Eigenschaft oder für die gesamte Entität ab.
		/// </summary>
		/// <returns>
		///     Die Validierungsfehler für die Eigenschaft oder Entität.
		/// </returns>
		/// <param name="propertyName">
		///     Der Name der Eigenschaft, für die Validierungsfehler abgerufen werden sollen, oder null oder
		///     <see cref="F:System.String.Empty" />, um Fehler auf Entitätsebene abzurufen.
		/// </param>
		public IEnumerable GetErrors(string propertyName)
		{
			if (propertyName == null) return null;

			List<string> errors;
			_propertyErrors.TryGetValue(propertyName, out errors);
			return errors;
		}

		/// <summary>
		///     Ruft einen Wert ab, der angibt, ob die Entität Validierungsfehler aufweist.
		/// </summary>
		/// <returns>
		///     true, wenn die Entität derzeit Validierungsfehler aufweist, andernfalls false.
		/// </returns>
		public bool HasErrors => _propertyErrors.Values.Any(v => v.Any());

		#endregion

		#region PropertyChanged

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

		#endregion
	}
}