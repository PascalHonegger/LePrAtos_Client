using System.Windows.Input;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// Interface für <see cref="LoginViewModel"/>
	/// </summary>
	public interface ILoginViewModel
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