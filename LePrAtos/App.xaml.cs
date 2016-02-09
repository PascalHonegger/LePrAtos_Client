// Projekt: LePrAtos
// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)
using System.Windows;
using System.Windows.Controls;
using LePrAtos.Properties;
using LePrAtos.Startup.Login;

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
			var dialog = new Dialogs.CustomDialog
			{
				InstructionText = "Bitte Konfiguration wählen:",
				Caption = "LePrAtos"
			};

			foreach (var serverSetting in Settings.Default.ConfiguredServers)
			{
				var configButton = new Button
				{
					Content = serverSetting,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch,
					Margin = new Thickness(3),
					Width = 110
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

			var mainWindow = new LoginView();

			mainWindow.Show();
		}
	}
}