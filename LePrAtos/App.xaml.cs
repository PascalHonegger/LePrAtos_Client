// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)

using System.Windows;
using System.Windows.Controls;
using LePrAtos.Properties;

namespace LePrAtos
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			SelectEnvironment();
		}

		/// <summary>Suchen möglicher App.config-Dateien im Programmverzeichnis, anzeigen einer Auswahl.</summary>
		private static void SelectEnvironment()
		{
			var dialog = new CustomDialog
			{
				InstructionText = "Bitte Konfiguration wählen:",
				Caption = "LePrAtos"
			};

			foreach (var serverSetting in Settings.Default.ConfiguredServers)
			{
				var configButton = new Button
				{
					Content = serverSetting
				};
				configButton.Click += (b, args) =>
				{
					StartApplication(serverSetting);
					dialog.Close();
				};
				dialog.AddControl(configButton);
			}

			dialog.Show();
		}

		private static void StartApplication(string configuration)
		{
			Session.Instance.Endpointconfiguration = configuration;

			var mainWindow = new MainWindow();

			mainWindow.Show();
		}
	}
}