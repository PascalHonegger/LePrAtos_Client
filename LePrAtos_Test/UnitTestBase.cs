﻿// Projekt: LePrAtos
// Copyright (c) LePrAtos 2016
// Author: Honegger, Pascal (ext)
using NUnit.Framework;

namespace LePrAtos_Test
{
	[TestFixture, Category("UnitTest")]
	public abstract class UnitTestBase
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