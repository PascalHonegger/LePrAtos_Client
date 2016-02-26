// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System.Windows;
using LePrAtos.Startup.Login;

namespace LePrAtos.Startup.Register
{
	/// <summary>
	///     Interaction logic for RegisterView.xaml
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

		private void TextChanged(object sender, object jigger)
		{
			RegisterButton.IsEnabled = false;
			if (Username.Text.Length < LoginViewModel.UsernameMaxLength &&
			    Username.Text.Length > LoginViewModel.UsernameMinLength &&
			    Password.Password.Length > PasswordMinLength &&
			    Equals(RepPassword.Password, Password.Password))
			{
				RegisterButton.IsEnabled = true;
			}
		}
	}
}