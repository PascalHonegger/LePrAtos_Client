﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Startup.Login;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Startup.Login
{
	public class LoginViewModelTest : IntegrationTestBase
	{
		private LoginViewModel _unitUnderTest;

		protected override void DoSetup()
		{
			_unitUnderTest = new LoginViewModel();
		}

		[Test]
		//TODO Pascal
		public void TestLogin()
		{
			// Arrange

			// Act

			// Assert
			Assert.That(true, Is.True);
		}
	}
}