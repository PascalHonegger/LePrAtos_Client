// Projekt: LePrAtos
// Copyright (c) LePrAtos
// Author: Honegger, Pascal (ext)
using LePrAtos;
using NUnit.Framework;

namespace LePrAtos_Test
{
	[TestFixture]
	public class NoCategoryTestExample
	{
		[Test]
		public void TestExampleMethod()
		{
			// Arrange
			var unitUnderTest = new Example();

			// Act
			var result = unitUnderTest.Tested(2);

			// Assert
			Assert.That(result, Is.EqualTo(4));
		}
	}
}
