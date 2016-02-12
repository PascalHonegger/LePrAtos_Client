// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)
using System.Windows.Input;
using LePrAtos.Infrastructure;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// Interface für <see cref="LoginViewModel"/>
	/// </summary>
	public interface ILoginViewModel : IRequestDialogCloseViewModel
	{
		/// <summary>
		/// Command für die Anmeldung
		/// </summary>
		ICommand LoginCommand { get; }

		/// <summary>
		/// Benutzername
		/// </summary>
		string Username { get; set; }
	}
}