// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Converter;
using LePrAtos_Test.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Converter
{
	public class InvertBooleanConverterTest : UnitTestBase
	{
		private InvertBooleanConverter _unitUnderTest;

		protected override void DoSetup()
		{
			_unitUnderTest = new InvertBooleanConverter();
		}

		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(null, false)]
		public void TestConvert(object input, bool expectedOutput)
		{
			Assert.That(_unitUnderTest.Convert(input, null, null, null), Is.EqualTo(expectedOutput));
		}

		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(null, false)]
		public void TestConvertBack(object input, bool expectedOutput)
		{
			Assert.That(_unitUnderTest.Convert(input, null, null, null), Is.EqualTo(expectedOutput));
		}
	}
}