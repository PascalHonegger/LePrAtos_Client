// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LePrAtos_Test.Infrastructure
{
	public class ViewModelBaseTest : UnitTestBase<ExampleViewModel>
	{
		protected override void DoSetup()
		{
			UnitUnderTest = new ExampleViewModel();
		}

		[Test]
		public void GetErrorsReturnsError()
		{
			//Arrange
			const string errorMessage = "Very bad Input";

			// Act
			UnitUnderTest.NeverHappyProperty = errorMessage;

			// Assert
			Assert.That(UnitUnderTest.GetErrors(nameof(ExampleViewModel.NeverHappyProperty)), Contains.Item(errorMessage));
			Assert.That(UnitUnderTest.HasErrors, Is.True);
		}

		[Test]
		public void GetErrorsReturnsMultipleErrors()
		{
			//Arrange
			var errorMessages = new List<string> {"Very bad Input", "Even worse output"};

			// Act
			UnitUnderTest.NeverHappyPropertyList = errorMessages;

			// Assert
			var correctlyFormattedErrorMessage = "Very bad Input" + Environment.NewLine +
			                                     "Even worse output";

			Assert.That(UnitUnderTest.GetErrors(nameof(ExampleViewModel.NeverHappyPropertyList)), Contains.Item(correctlyFormattedErrorMessage));
			Assert.That(UnitUnderTest.HasErrors, Is.True);
		}

		[Test]
		public void GetErrorsReturnsNoError()
		{
			//Arrange
			const string errorMessage = "Very random Input";

			// Act
			UnitUnderTest.NeverHappyProperty = errorMessage;

			//Remove Error again
			UnitUnderTest.NeverHappyProperty = string.Empty;

			// Assert
			Assert.That(UnitUnderTest.GetErrors(nameof(ExampleViewModel.NeverHappyProperty)), Is.Null);
			Assert.That(UnitUnderTest.HasErrors, Is.False);
		}
	}
}
