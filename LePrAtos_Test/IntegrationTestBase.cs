// Projekt: LePrAtos
// Copyright (c) LePrAtos 2016
// Author: Honegger, Pascal (ext)
using NUnit.Framework;

namespace LePrAtos_Test
{
	[TestFixture, Category("IntegrationTest")]
	public abstract class IntegrationTestBase
	{
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
