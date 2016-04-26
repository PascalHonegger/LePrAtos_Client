// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Windows;
using LePrAtos.Infrastructure;
using LePrAtos.Properties;
using LePrAtos.Startup.Register;
using Microsoft.Practices.Prism.Commands;

namespace LePrAtos.Startup.Login
{
	/// <summary>
	///     ViewModel für das Zurücksetzen des Passworts
	/// </summary>
	public class ResetPasswordViewModel : ViewModelBase, IRequestWindowClose
	{
		private DelegateCommand _requestPasswordResetCommand;
		private DelegateCommand _setNewPasswordCommand;
		private string _resetCode;
		private string _newPassword;
		private string _mailToReset;
		private bool _isFirstStage = true;

		/// <summary>
		///     Wird im GUI verwendet um gewisse Elemente hervorzuheben
		/// </summary>
		public bool IsFirstStage
		{
			get { return _isFirstStage; }
			set
			{
				_isFirstStage = value;
				OnPropertyChanged();
			}
		}

		public DelegateCommand RequestPasswordResetCommand => _requestPasswordResetCommand ?? (_requestPasswordResetCommand = new DelegateCommand(RequestPasswordReset, () => !string.IsNullOrEmpty(MailToReset)));
		public DelegateCommand SetNewPasswordCommand => _setNewPasswordCommand ?? (_setNewPasswordCommand = new DelegateCommand(SetNewPassword, () => !HasErrors));

		private void SetNewPassword()
		{
			BusyRunner.RunAsync(async () =>
				{
					await CurrentSession.Client.setPasswordFromResetAsync(MailToReset, ResetCode, PasswordHasher.HashPasswort(NewPassword));
					MessageBox.Show(Strings.ResetPassword_PasswordChanged);
					RequestWindowCloseEvent?.Invoke(this, EventArgs.Empty);
				}
			);
		}

		public void RequestPasswordReset()
		{
			BusyRunner.RunAsync(async () =>
				{
					var result = await CurrentSession.Client.requestPasswordResetAsync(MailToReset);
					IsFirstStage = false;
					NewPassword = MailToReset = string.Empty;

					//Remove once Mail is active
					ResetCode = result.@return;
				}
			);
		}

		public string MailToReset
		{
			get { return _mailToReset; }
			set
			{
				if (Equals(_mailToReset, value)) return;
				_mailToReset = value;
				OnPropertyChanged();
				RequestPasswordResetCommand.RaiseCanExecuteChanged();
			}
		}

		public string ResetCode
		{
			get { return _resetCode; }
			set
			{
				if (Equals(_resetCode, value)) return;
				_resetCode = value;

				SetErrorForProperty(string.IsNullOrEmpty(_resetCode) ? Strings.TextValidationRule_Mandatory : string.Empty);

				OnPropertyChanged();
				SetNewPasswordCommand.RaiseCanExecuteChanged();
			}
		}

		public string NewPassword
		{
			get { return _newPassword; }
			set
			{
				if (Equals(_newPassword, value)) return;
				_newPassword = value;

				SetErrorForProperty(RegisterViewModel.PasswordValid(_newPassword) ? string.Empty : string.Format(Strings.TextValidationRule_Length, RegisterViewModel.PasswordMinLength, RegisterViewModel.PasswordMaxLength));

				OnPropertyChanged();
				SetNewPasswordCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		///     Event, welcher das schliessen des Dialoges anfordert
		/// </summary>
		public EventHandler RequestWindowCloseEvent { get; set; }
	}
}