// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.ComponentModel;
using LePrAtos.Infrastructure;

namespace LePrAtos.Lobby
{
	/// <summary>
	///     Interaction logic for LobbyView.xaml
	/// </summary>
	public partial class LobbyView
	{
		/// <summary>
		///     Abonniert auf den <see cref="IRequestWindowClose.RequestWindowCloseEvent"/>
		/// </summary>
		public LobbyView(IRequestWindowClose viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			viewModel.RequestWindowCloseEvent += (sender, e) => Close();
		}

		private LobbyViewModel ViewModel => DataContext as LobbyViewModel;

		private void LobbyView_OnClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = !ViewModel.LeaveLobby();
		}
	}
}