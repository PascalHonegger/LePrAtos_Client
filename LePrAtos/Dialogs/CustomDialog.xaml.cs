// Projekt: LePrAtos
// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

using System.Windows.Controls;

namespace LePrAtos.Dialogs
{
	/// <summary>
	/// Interaction logic for CustomDialog.xaml
	/// </summary>
	public partial class CustomDialog
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomDialog()
		{
			InitializeComponent();
			var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
			var windowWidth = Width;
			var windowHeight = Height;
			Left = (screenWidth / 2) - (windowWidth / 2);
			Top = (screenHeight / 2) - (windowHeight / 2);
		}

		/// <summary>
		/// The Instruction Text
		/// </summary>
		public string InstructionText
		{
			set { InstructionTextControl.Text = value; }
		}

		/// <summary>
		/// The Caption Text
		/// </summary>
		public string Caption
		{
			set { Title = value; }
		}

		/// <summary>
		/// Add a Control to the Gui
		/// </summary>
		/// <param name="configButton">UIElement to add</param>
		public void AddControl(Button configButton)
		{
			Controls.Items.Add(configButton);
		}
	}
}
