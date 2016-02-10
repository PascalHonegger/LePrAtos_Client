﻿// Projekt: LePrAtos
// Copyright (c) LePrAtos 2016
// Author: Honegger, Pascal (ext)
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
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
		private CultureInfo UiCulture { get; set; } = new CultureInfo("de");

		private void OnStartup(object sender, StartupEventArgs e)
		{
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
				Height = 200
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

			var cultureSelection = new ComboBox();

			var supportedCultures = new List<CultureInfo>();

			var rm = new ResourceManager(typeof(Strings));

			var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
			foreach (var culture in cultures.Where(c => !string.IsNullOrEmpty(c.Name)))
			{
				try
				{
					var rs = rm.GetResourceSet(culture, true, false);
					
					if (rs != null)
					{
						supportedCultures.Add(culture);
					}
				}
				catch (CultureNotFoundException)
				{
					// Culture not supported
				}
			}

			cultureSelection.ItemsSource = supportedCultures;

			cultureSelection.SelectionChanged += (sender, e) => { UiCulture = cultureSelection.SelectedItem as CultureInfo; };

			dialog.AddControl(cultureSelection);

			dialog.Show();
		}

		private void StartApplication(string configuration)
		{
			Session.Instance.Endpointconfiguration = configuration;

			Strings.Culture = UiCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = UiCulture;

			var mainWindow = new LoginView();

			mainWindow.Show();
		}
	}
}