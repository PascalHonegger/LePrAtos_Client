// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

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
		public LobbyView(LobbyViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
