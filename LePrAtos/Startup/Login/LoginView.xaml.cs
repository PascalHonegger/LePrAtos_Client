// Projekt: LePrAtos
// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

using System.Windows;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LoginView()
		{
			InitializeComponent();
			DataContext = new LoginViewModel();
		}

		LoginViewModel ViewModel => DataContext as LoginViewModel;

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.SayHelloWorld();
		}
	}
}
