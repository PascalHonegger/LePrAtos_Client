// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System.Windows;
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
		}

		private RegisterViewModel ViewModel => DataContext as RegisterViewModel;

		private void TextChanged(object sender, RoutedEventArgs e)
		{
			RegisterButton.IsEnabled = ViewModel.CanRegister(Password.Password, RepPassword.Password);
		}
	}
}