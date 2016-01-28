using LePrAtos.Trade;
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
