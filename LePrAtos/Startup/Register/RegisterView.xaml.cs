// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using LePrAtos.Infrastructure;

namespace LePrAtos.Startup.Register
{
	/// <summary>
	///     Interaction logic for RegisterView.xaml
	/// </summary>
	public partial class RegisterView
	{
		/// <summary>
		///     Abonniert auf den <see cref="IRequestWindowClose.RequestWindowCloseEvent"/> und setzt den Datacontext
		/// </summary>
		/// <param name="viewModel"></param>
		public RegisterView(IRequestWindowClose viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
			viewModel.RequestWindowCloseEvent += (sender, e) => Close();
			
			UsernameValidationRule.MinLength = RegisterViewModel.UsernameMinLength;
			UsernameValidationRule.MaxLength = RegisterViewModel.UsernameMaxLength;
		}

		private RegisterViewModel ViewModel => DataContext as RegisterViewModel;

		private void TextChanged(object sender, object parameter)
		{
			RegisterButton.IsEnabled = ViewModel.CanRegister(Password.Password, RepPassword.Password);
		}
	}
}