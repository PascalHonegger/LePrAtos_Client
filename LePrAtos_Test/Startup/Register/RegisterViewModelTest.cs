﻿// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Startup.Login;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Startup.Register
{
	public class RegisterViewModelText : IntegrationTestBase
	{
		private LoginViewModel _unitUnderTest;

		protected override void DoSetup()
		{
			_unitUnderTest = new LoginViewModel();
		}

		[Test]
		//TODO Pascal
		public void TestRegister()
		{
			// Arrange

			// Act

			// Assert
			Assert.That(true, Is.True);
		}
	}
}