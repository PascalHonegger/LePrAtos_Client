// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Threading.Tasks;
using LePrAtos.Startup.Register;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Startup.Register
{
	public class RegisterViewModelText : IntegrationTestBase<RegisterViewModel>
	{
		protected override void DoSetup()
		{
			UnitUnderTest = new RegisterViewModel();
		}

		[Test, Ignore("Name und Mail sind noch nicht garantiert frei")]
		public async Task TestRegister()
		{
			// Arrange

			while (!UnitUnderTest.CanRegister("passwort", "password"))
			{
				UnitUnderTest.Username = "TODO Not taken Username";
				UnitUnderTest.MailAddress = "TODONot@taken.Email";
			}

			// Act
			await UnitUnderTest.RegisterUser("passwort");

			// Assert
			Assert.That(UnitUnderTest.CurrentSession.Player, Is.Not.Null);
		}
	}
}