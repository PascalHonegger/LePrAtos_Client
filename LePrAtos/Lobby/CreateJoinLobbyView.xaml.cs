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
	/// Interaction logic for CreateJoinLobbyView.xaml
	/// </summary>
	public partial class CreateJoinLobbyView : Window
	{
		public CreateJoinLobbyView(string result)
		{
			InitializeComponent();
			Box.Text = result;
		}
	}
}
