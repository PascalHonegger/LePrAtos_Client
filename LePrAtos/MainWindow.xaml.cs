// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

using System.Windows;

namespace LePrAtos
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel();
		}

		MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.SayHelloWorld();
		}
	}
}
