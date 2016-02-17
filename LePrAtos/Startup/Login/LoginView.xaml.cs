// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Windows;
using LePrAtos.Infrastructure;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LoginView(IRequestWindowClose dataContext)
		{
			InitializeComponent();

			var screenWidth = SystemParameters.PrimaryScreenWidth;
			var screenHeight = SystemParameters.PrimaryScreenHeight;
			var windowWidth = Width;
			var windowHeight = Height;
			Left = screenWidth / 2 - windowWidth / 2;
			Top = screenHeight / 2 - windowHeight / 2;

			DataContext = dataContext;
			dataContext.RequestWindowCloseEvent += (sender, e) =>
			{
				(sender as LoginView)?.Show();
				Close();
			};
		}
	}
}
