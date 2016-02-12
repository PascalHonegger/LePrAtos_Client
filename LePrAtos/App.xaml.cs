// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Windows;
using System.Windows.Controls;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Startup.Login;
using LePrAtos.Unity;
using Microsoft.Practices.Unity;

namespace LePrAtos
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			UnityContainerProvider.InitializeContainer();

			SelectEnvironment();
		}

		/// <summary>Suchen möglicher App.config-Dateien im Programmverzeichnis, anzeigen einer Auswahl.</summary>
		private void SelectEnvironment()
		{
			var dialog = new Dialogs.CustomDialog
			{
				InstructionText = "Bitte Konfiguration wählen",
				Caption = "Wähle Konfiguration",
				Width = 430,
				Height = 150
			};

			foreach (var serverSetting in Settings.Default.ConfiguredServers)
			{
				var configButton = new Button
				{
					Content = serverSetting,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch,
					Margin = new Thickness(3),
					Width = 90
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
			UnityContainerProvider.Container.Resolve<ISession>().Endpointconfiguration = configuration;

			var loginWindow = new LoginView(UnityContainerProvider.Container.Resolve<ILoginViewModel>());

			loginWindow.Show();
		}
	}
}