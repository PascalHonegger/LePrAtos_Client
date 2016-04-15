// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LePrAtos.Dialogs;
using LePrAtos.Infrastructure;
using LePrAtos.Lobby;
using LePrAtos.Properties;
using LePrAtos.Service_References;
using LePrAtos.Startup.Login;
using Microsoft.Practices.Unity;
using UnityContainer;

namespace LePrAtos
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			//Load Culture
			CultureInfo culture;
			try
			{
				culture = new CultureInfo(Settings.Default.SelectedCulture);
			}
			catch (Exception)
			{
				Settings.Default.SelectedCulture = CultureInfo.CurrentCulture.Name;
				culture = CultureInfo.CurrentCulture;
			}
			Strings.Culture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;

			//Load Unity
			new ScannerModule().Initialize();
			_session = ContainerProvider.Container.Resolve<ISession>();

			//Load Selected Theme
			try
			{
				Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"Themes/{Settings.Default.SelectedTheme}.xaml", UriKind.Relative) });
				Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Infrastructure/DefaultResources.xaml", UriKind.Relative) });
			}
			catch (Exception)
			{
				Settings.Default.SelectedTheme = string.Empty;
				Settings.Default.Save();
			}

			//Start Application
#if DEBUG
			SelectEnvironment();
#else
			StartApplication(Settings.Default.ConfiguredServers[0]);
#endif
		}

		/// <summary>Durchsucht alle konfigurierten Server und zeigt eine Auswahl dieser an.</summary>
		private static void SelectEnvironment()
		{
			var dialog = new CustomDialog
			{
				InstructionText = "Bitte Konfiguration wählen",
				Title = "Wähle Konfiguration",
				Width = 335,
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
				dialog.ItemSource.Add(configButton);
			}

			dialog.Show();

			dialog.ItemSource.First().Focus();
		}

		private static ISession _session;

		private static void StartApplication(string configuration)
		{
			_session.Endpointconfiguration = configuration;

			if (!string.IsNullOrEmpty(Settings.Default.SavedUser))
			{
				var player = CurrentSession.Client.getPlayerByID(Settings.Default.SavedUser);

				if (player != null)
				{
					var playerViewModel = ContainerProvider.Container.Resolve<PlayerViewModel>();

					playerViewModel.Player = player;

					_session.Player = playerViewModel;

					var lobbyBrowser = new LobbyBrowserView(ContainerProvider.Container.Resolve<LobbyBrowserViewModel>());

					lobbyBrowser.Show();

					return;
				}
			}

			var loginWindow = new LoginView(new LoginViewModel());
			loginWindow.Show();
		}

		private static ISession CurrentSession => ContainerProvider.Container.Resolve<ISession>();

		private void App_OnExit(object sender, ExitEventArgs e)
		{
			try
			{
				CurrentSession?.PollingTimer?.Dispose();
				if (!string.IsNullOrEmpty(CurrentSession?.Player?.PlayerId))
				{
					CurrentSession.Client.logout(CurrentSession.Player.PlayerId);
				}
			}
			catch (Exception)
			{
				// ignored
			}
		}

		private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			//Try to say server, that I closed
			App_OnExit(sender, null);
			MessageBox.Show(string.Format(Strings.ExceptionHandling_Message, e.Exception.Message, e.Exception.StackTrace), Strings.ExceptionHandling_Caption);
			Current.Shutdown(666);
		}
	}
}