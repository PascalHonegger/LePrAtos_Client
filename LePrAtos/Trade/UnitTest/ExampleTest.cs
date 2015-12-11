using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LePrAtos.Trade.Source;
using NUnit.Framework;

namespace LePrAtos.Trade.UnitTest
{
	[TestFixture]
	[Category("UnitTest")]
	class ExampleTest
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
