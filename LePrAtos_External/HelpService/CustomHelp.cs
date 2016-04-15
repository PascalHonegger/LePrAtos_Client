// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace HelpService
{
	public static class CustomHelp
	{
		public static readonly DependencyProperty HelpTopicProperty =
		DependencyProperty.RegisterAttached("HelpId", typeof(int), typeof(CustomHelp));

		static CustomHelp()
		{
			CommandManager.RegisterClassCommandBinding(
				typeof(FrameworkElement),
				new CommandBinding(
					ApplicationCommands.Help,
					ShowHelpExecuted,
					ShowHelpCanExecute));
		}

		/// <summary>
		/// Getter for <see cref="HelpTopicProperty"/>. Get a help topic that's attached to an object. 
		/// </summary>
		/// <param name="obj">The object that the help topic is attached to.</param>
		/// <returns>The help topic.</returns>
		public static int? GetHelpTopic(DependencyObject obj)
		{
			return obj.GetValue(HelpTopicProperty) as int?;
		}

		/// <summary>
		/// Setter for <see cref="HelpTopicProperty"/>. Attach a help topic value to an object. 
		/// </summary>
		/// <param name="obj">The object to which to attach the help topic.</param>
		/// <param name="value">The value of the help topic.</param>
		public static void SetHelpTopic(DependencyObject obj, int value)
		{
			obj.SetValue(HelpTopicProperty, value);
		}

		/// <summary>
		/// Whether the F1 help command can execute. 
		/// </summary>
		private static void ShowHelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;

			if (GetHelpTopic(senderElement).HasValue)
				e.CanExecute = true;
		}

		/// <summary>
		/// Execute the F1 help command. 
		/// </summary>
		/// <remarks>Calls ShowHelpTopic to show the help topic attached to the framework element that's the 
		/// source of the call.</remarks>
		private static void ShowHelpExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var helpTopic = GetHelpTopic(sender as FrameworkElement);
			if (helpTopic.HasValue) ShowHelp(helpTopic.Value);
		}

		/// <summary>
		///     Zeigt die Hilfe in der jetzigen Sprache an.
		/// </summary>
		/// <param name="topic"></param>
		public static void ShowHelp(int topic)
		{
			try
			{
				var currentDir = Environment.CurrentDirectory;
				var path = Path.GetFullPath(new Uri(currentDir + $"/Resources/{Thread.CurrentThread.CurrentCulture.Name}.chm").LocalPath);
				Help.ShowHelp(null, path, HelpNavigator.TopicId, topic.ToString());
			}
			catch (Exception)
			{
				MessageBox.Show("Couldn't load Help!");
			}
		}
	}
}