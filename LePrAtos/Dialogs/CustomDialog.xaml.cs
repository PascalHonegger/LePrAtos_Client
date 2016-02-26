// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Collections.ObjectModel;
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
			Controls.ItemsSource = ItemSource;
		}

		/// <summary>
		/// The Instruction Text
		/// </summary>
		public string InstructionText
		{
			set { InstructionTextControl.Text = value; }
		}

		/// <summary>
		///     Die Quelle der Controls
		/// </summary>
		public ObservableCollection<UIElement> ItemSource { get; } = new ObservableCollection<UIElement>();
	}
}