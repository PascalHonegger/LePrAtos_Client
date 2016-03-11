// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Infrastructure;
using NUnit.Framework;

namespace LePrAtos_Test.Infrastructure
{
	[TestFixture, Category("UnitTest")]
	public abstract class UnitTestBase<T>
	{
		protected T UnitUnderTest;

		[SetUp]
		public void SetUp()
		{
			DoSetup();
		}

		[TearDown]
		public void TearDown()
		{
			DoTearDown();
		}

		protected virtual void DoSetup()
		{
			//Optional
		}

		protected virtual void DoTearDown()
		{
			//Optional
		}
	}
}