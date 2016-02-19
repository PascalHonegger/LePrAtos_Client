﻿// Projekt: LePrAtos
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

			DataContext = dataContext;
			dataContext.RequestWindowCloseEvent += (sender, e) =>
			{
				(sender as LoginView)?.Show();
				Close();
			};
		}
	}
}
