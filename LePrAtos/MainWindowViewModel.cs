// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LePrAtos.Annotations;
using LePrAtos.HelloWorldService;

namespace LePrAtos
{
	/// <summary>
	/// ViewModel für MainWindow
	/// </summary>
	public sealed class MainWindowViewModel : INotifyPropertyChanged
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

		/// <summary>
		/// Event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notify Property Changed
		/// </summary>
		/// <param name="propertyName"></param>
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
