﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Keller, Alain
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LePrAtos.Lobby
{
	/// <summary>
	/// Interaction logic for LobbyBrowserView.xaml
	/// </summary>
	public partial class LobbyBrowserView : Window
	{

		public LobbyBrowserView(string result)
		{
			InitializeComponent();
			Box.Text = result;

			var viewModel = new LobbyBrowserViewModel();

			viewModel.RequestDialogCloseEventHandler += (sender, e) => Close();

			DataContext = viewModel;
		}
	}
}
