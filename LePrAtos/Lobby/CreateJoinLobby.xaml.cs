﻿using System;
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
	/// Interaction logic for CreateJoinLobby.xaml
	/// </summary>
	public partial class CreateJoinLobby : Window
	{
		public CreateJoinLobby(string result)
		{
			InitializeComponent();
			Box.Text = result;
		}
	}
}
