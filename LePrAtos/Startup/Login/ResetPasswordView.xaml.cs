// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     Interaction logic for ResetPasswordView.xaml
	/// </summary>
	public partial class ResetPasswordView
	{
		public ResetPasswordView()
		{
			InitializeComponent();
			var viewModel = new ResetPasswordViewModel();
			DataContext = viewModel;
			viewModel.RequestWindowCloseEvent += (sender, e) => Close();
		}
	}
}