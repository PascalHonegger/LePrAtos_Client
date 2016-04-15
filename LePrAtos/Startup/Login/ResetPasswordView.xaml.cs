// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Windows;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     Interaction logic for ResetPasswordView.xaml
	/// </summary>
	public partial class ResetPasswordView : Window
	{
		public ResetPasswordView()
		{
			InitializeComponent();
			DataContext = new ResetPasswordViewModel();
		}

		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			DataContext = null;
			Close();
		}
	}
}