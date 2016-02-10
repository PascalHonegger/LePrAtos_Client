// Projekt: LePrAtos
// Copyright (c) LePrAtos 2016
// Author: Honegger, Pascal (ext)

using System.Windows.Controls;
using System.Windows.Input;
using LePrAtos.Service_References.HelloWorldService;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// ViewModel für LoginView
	/// </summary>
	public sealed class LoginViewModel : ViewModelBase
	{
		private string _output;

		/// <summary>
		/// Benutzername
		/// </summary>
		public string Username { get; set; }

		private void Login(PasswordBox passwordBox)
		{
			var client = new HelloWorldClient(Session.Instance.Endpointconfiguration);
			var response = client.sayHelloWorld($"{Username} ; {passwordBox.Password}");
			Output = response;
		}

		/// <summary>
		/// Ouput
		/// </summary>
		public string Output
		{
			get { return _output; }
			set
			{
				_output = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Command für <see cref="Login"/>
		/// </summary>
		public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(Login));

		private ICommand _loginCommand;
	}
}
