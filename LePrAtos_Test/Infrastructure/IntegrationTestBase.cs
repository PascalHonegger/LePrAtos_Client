// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using LePrAtos.Infrastructure;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityContainer;

namespace LePrAtos_Test.Infrastructure
{
	[TestFixture, Category("IntegrationTest")]
	public abstract class IntegrationTestBase<T>
	{
		protected T UnitUnderTest;

		protected IUnityContainer Container;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			//Load Unity
			ContainerProvider.ResetForUnitTestingPurposesOnly();
			Container = ContainerProvider.Container;

			Container.RegisterType<ISession, Session>();
			Container.RegisterType<IBusyRunner, BusyRunner>();
		}

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
