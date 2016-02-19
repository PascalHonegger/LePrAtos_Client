// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)
using System.Windows;

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
		}

		/// <summary>
		/// The Instruction Text
		/// </summary>
		public string InstructionText
		{
			set { InstructionTextControl.Text = value; }
		}

		/// <summary>
		/// Add a Control to the Gui
		/// </summary>
		/// <param name="configButton">UIElement to add</param>
		public void AddControl(UIElement configButton)
		{
			Controls.Items.Add(configButton);
		}
	}
}
