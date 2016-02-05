using System.Windows.Controls;

namespace LePrAtos
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
		/// The Caption Text
		/// </summary>
		public string Caption
		{
			set { CaptionControl.Text = value; }
		}

		/// <summary>
		/// Add a Control to the Gui
		/// </summary>
		/// <param name="configButton">UIElement to add</param>
		public void AddControl(Button configButton)
		{
			ItemsControl.Items.Add(configButton);
		}
	}
}
