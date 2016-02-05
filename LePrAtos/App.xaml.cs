using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using LePrAtos.Properties;

namespace LePrAtos
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private string _currentAssemblyFolder;

		private void OnStartup(object sender, StartupEventArgs e)
		{
			//Rekursion: Laden der Assembly in einer neuen AppDomain (mit geänderter App.config)
			var currentAssembly = Assembly.GetExecutingAssembly();
			var currentAssemblyLocation = currentAssembly.Location;

			_currentAssemblyFolder = Path.GetDirectoryName(currentAssemblyLocation);

			SelectEnvironment();
		}

		/// <summary>Suchen möglicher App.config-Dateien im Programmverzeichnis, anzeigen einer Auswahl.</summary>
		private void SelectEnvironment()
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

		private void StartApplication(string configuration)
		{
			var configFile = Path.Combine(_currentAssemblyFolder, "App.config");
			var xmlConfig = new XmlDocument();
			xmlConfig.Load(configFile);
			var addNodes = xmlConfig.SelectNodes("//baseAddresses/add");
			var baseAddressAttribute = addNodes?.Item(0)?.Attributes?["baseAddress"];
			if (baseAddressAttribute != null) baseAddressAttribute.Value = configuration;
			xmlConfig.Save(configFile);

			var mainWindow = new MainWindow();

			mainWindow.Show();
		}
	}
}