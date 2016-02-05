// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

using LePrAtos.HelloWorldService;

namespace LePrAtos
{
	/// <summary>
	/// ViewModel für MainWindow
	/// </summary>
	public sealed class MainWindowViewModel : ViewModelBase
	{
		private string _output;

		/// <summary>
		/// Benutzername
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		public string Password{ get; set; }

		/// <summary>
		/// Sagt Hallo Welt
		/// </summary>
		public void SayHelloWorld()
		{
			var client = new HelloWorldClient(Session.Instance.Endpointconfiguration);
			var response = client.sayHelloWorld(Username);
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
	}
}
