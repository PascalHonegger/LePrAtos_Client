// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)
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
		public LoginView(ILoginViewModel dataContext)
		{
			InitializeComponent();
			DataContext = dataContext;
		}
	}
}
