// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Windows.Controls;
using LePrAtos.Startup.Login;
using LePrAtos_Test.Infrastructure;
using Moq;
using NUnit.Framework;

namespace LePrAtos_Test.Startup.Login
{
	public class LoginViewModelTest : IntegrationTestBase<LoginViewModel>
	{
		protected override void DoSetup()
		{
			UnitUnderTest = new LoginViewModel();
		}

		[Test]
		public void TestLoginWithUsername()
		{
			// Arrange
			UnitUnderTest.Username = "test";

			// Act
			UnitUnderTest.LoginUser("passwort");

			// Assert
			Assert.That(UnitUnderTest.CurrentSession.Player, Is.Not.Null);
		}
	}
}