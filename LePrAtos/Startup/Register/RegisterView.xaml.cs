// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System.Windows;
using System.Windows.Input;
using LePrAtos.Startup.Login;

namespace LePrAtos.Startup.Register
{
	/// <summary>
	/// Interaction logic for RegisterView.xaml
	/// </summary>
	public partial class RegisterView : Window
	{
		private const int PasswordMinLength = 5;
		public RegisterView()
		{
			InitializeComponent();
			var registerViewModel = new RegisterViewModel();
			DataContext = registerViewModel;
			registerViewModel.RequestWindowCloseEvent += (sender, e) => Close();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			RegisterButton.IsEnabled = false;
			if (Username.Text.Length < LoginViewModel.UsernameMaxLength &&
			    Username.Text.Length > LoginViewModel.UsernameMinLength && 
				Password.Password.Length > PasswordMinLength &&
				RepPassword.Password == Password.Password)
			{
				RegisterButton.IsEnabled = true;
			}
		}
	}
}
