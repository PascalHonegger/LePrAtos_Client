// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

namespace LePrAtos.Startup.CustomSettings
{
	/// <summary>
	///     Interaction logic for SettingsView.xaml
	/// </summary>
	public partial class SettingsView
	{
		public SettingsView()
		{
			InitializeComponent();
			SettingsViewModel viewModel;
			DataContext = viewModel = new SettingsViewModel();
			viewModel.RequestWindowCloseEvent += (sender, e) => Close();
		}
	}
}