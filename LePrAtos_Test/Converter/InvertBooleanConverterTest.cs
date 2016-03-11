// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Converter;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Converter
{
	public class InvertBooleanConverterTest : UnitTestBase<InvertBooleanConverter>
	{
		protected override void DoSetup()
		{
			UnitUnderTest = new InvertBooleanConverter();
		}

		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(null, false)]
		public void TestConvert(object input, bool expectedOutput)
		{
			Assert.That(UnitUnderTest.Convert(input, null, null, null), Is.EqualTo(expectedOutput));
		}

		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(null, false)]
		public void TestConvertBack(object input, bool expectedOutput)
		{
			Assert.That(UnitUnderTest.Convert(input, null, null, null), Is.EqualTo(expectedOutput));
		}
	}
}