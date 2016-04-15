// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Threading.Tasks;
using LePrAtos.Startup.Login;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Startup.Login
{
	public class LoginViewModelTest : IntegrationTestBase<LoginViewModel>
	{
		protected override void DoSetup()
		{
			UnitUnderTest = new LoginViewModel();
		}

		[Test, Ignore("User ist nicht garantiert abgemeldet")]
		public async Task TestLoginWithUsername()
		{
			// Arrange
			const string username = "Test";
			const string passwort = "passwort";
			UnitUnderTest.UsernameOrMail = username;

			// Act
			await UnitUnderTest.LoginUser(passwort);

			// Assert
			Assert.That(UnitUnderTest.CurrentSession.Player, Is.Not.Null);
			Assert.That(UnitUnderTest.CurrentSession.Player.Username, Is.EqualTo(username));
			Assert.That(UnitUnderTest.CurrentSession.Player, Is.Not.Null);

			//Cleanup
			await UnitUnderTest.CurrentSession.Client.logoutAsync(UnitUnderTest.CurrentSession.Player.PlayerId);
		}
	}
}