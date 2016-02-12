// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

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
			DataContext = dataContext;
			dataContext.RequestWindowCloseEvent += (sender, e) => Close();
		}
	}
}
