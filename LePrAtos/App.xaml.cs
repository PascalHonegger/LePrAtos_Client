// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LePrAtos.Dialogs;
using LePrAtos.GameManagerService;
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
			var culture = new CultureInfo(Settings.Default.SelectedCulture);
			Strings.Culture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			new ScannerModule().Initialize();

			_session = ContainerProvider.Container.Resolve<ISession>();

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

				configButton.Focus();

				configButton.Click += (b, args) =>
				{
					StartApplication(serverSetting);
					dialog.Close();
				};
				dialog.AddControl(configButton);
			}

			dialog.Show();
		}

		private static ISession _session;

		private static void StartApplication(string configuration)
		{
			_session.Endpointconfiguration = configuration;

			_session.PollingTimer.Start();

			if (!string.IsNullOrEmpty(Settings.Default.SavedUser))
			{
				// var player = CurrentSession.Client.getUserFromId(Settings.Default.SavedUser);

				var playerViewModel = ContainerProvider.Container.Resolve<PlayerViewModel>();

				playerViewModel.Player = new player
				{
					username = "Get from Server!",
					playerID = Settings.Default.SavedUser
				};

				_session.Player = playerViewModel;

				var lobbyBrowser = new LobbyBrowserView(ContainerProvider.Container.Resolve<LobbyBrowserViewModel>());

				lobbyBrowser.Show();
			}
			else
			{
				var loginWindow = new LoginView(ContainerProvider.Container.Resolve<LoginViewModel>());

				loginWindow.Show();
			}
		}

		private static ISession CurrentSession => ContainerProvider.Container.Resolve<ISession>();

		private void App_OnExit(object sender, ExitEventArgs e)
		{
			try
			{
				CurrentSession?.PollingTimer?.Dispose();
				//CurrentSession?.Client?.Logout();
			}
			catch (System.Exception)
			{
				// ignored
			}
		}

		private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(string.Format(Strings.ExceptionHandling_Message, e.Exception.Message, e.Exception.StackTrace), Strings.ExceptionHandling_Caption);
			Environment.Exit(1);
		}
	}
}