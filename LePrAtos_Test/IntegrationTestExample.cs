// Projekt: LePrAtos
// Copyright (c) LePrAtos 2016
// Author: Honegger, Pascal (ext)
using LePrAtos;
using NUnit.Framework;

namespace LePrAtos_Test
{
	public class IntegrationTestExample : IntegrationTestBase
	{
		private Example _unitUnderTest;

		protected override void DoSetup()
		{
			_unitUnderTest = new Example();
		}

		protected override void DoTearDown()
		{
			_unitUnderTest = null;
		}

		[Test]
		public void TestExampleMethod()
		{
			// Arrange


			// Act
			var result = _unitUnderTest.Tested(2);

			// Assert
			Assert.That(result, Is.EqualTo(4));
		}
	}
}
