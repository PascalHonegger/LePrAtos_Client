// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Infrastructure;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// Interaction logic for LobbyView.xaml
	/// </summary>
	public partial class LobbyView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LobbyView(IRequestWindowClose viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			viewModel.RequestWindowCloseEvent += (sender, e) => Close();
		}
	}
}
