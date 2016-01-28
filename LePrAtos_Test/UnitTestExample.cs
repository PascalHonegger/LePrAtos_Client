using LePrAtos.Trade;
using NUnit.Framework;

namespace LePrAtos_Test
{
	[TestFixture, Category("UnitTest")]
	internal class UnitTestExample
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
		[Test]
		public void TestExampleMethodWrong()
		{
			// Arrange
			var unitUnderTest = new Example();

			// Act
			var result = unitUnderTest.Tested(123);

			// Assert
			Assert.That(result, Is.EqualTo(123123123123));
		}
	}
}
