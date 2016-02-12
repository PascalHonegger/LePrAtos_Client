// Projekt: LePrAtos
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

		[TestCase("SuperFancyUsername", true)]
		[TestCase("WayTooLongUsernameThatWouldDisturbEveryoneBecauseItIsTooDamnLong", false)]
		public void TestExampleMethod(string username, bool expectedResult)
		{
			// Arrange


			// Act
			_unitUnderTest.Username = username;

			// Assert
			Assert.That(Equals(_unitUnderTest.Username, username), Is.EqualTo(expectedResult));
		}
	}
}