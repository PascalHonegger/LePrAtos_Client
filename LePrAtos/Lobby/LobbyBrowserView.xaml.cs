// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain

using System.Windows;
using System.Windows.Input;
using LePrAtos.Infrastructure;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// Interaction logic for LobbyBrowserView.xaml
	/// </summary>
	public partial class LobbyBrowserView : Window
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewModel">ViewModel to be used as DataContext</param>
		public LobbyBrowserView(IRequestWindowClose viewModel)
		{
			InitializeComponent();
			
			viewModel.RequestWindowCloseEvent += (sender, e) => Close();

			DataContext = viewModel;
		}

		private LobbyBrowserViewModel ViewModel => DataContext as LobbyBrowserViewModel;

		private async void Row_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (ViewModel.JoinLobbyCommand.CanExecute())
			{
				await ViewModel.JoinLobbyCommand.Execute();
			}
		}
	}
}
