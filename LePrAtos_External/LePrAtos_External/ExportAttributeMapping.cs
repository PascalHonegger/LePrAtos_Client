// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.Unity;

namespace UnityContainer
{
	public class ScannerModule
	{
		private static int _initialized;
		static ScannerModule()
		{
			ContainerProvider.ContainerReset += () => Interlocked.Decrement(ref _initialized);
		}

		public void Initialize()
		{
			if (Interlocked.CompareExchange(ref _initialized, 1, 0) != 0)
			{
				return;
			}
			Configure();
		}

		public static bool AlreadyConfigured
		{
			get { return _initialized != 0; }
		}

		private static void Configure()
		{
			var unityContainer = ContainerProvider.Container;
			//TODO unityContainer.AddNewExtension<PostResolvedExtension>();
			unityContainer.RegisterTypes(new WorkbenchScannerConvention());
		}
	}


	public static class ContainerProvider
	{
		private static readonly object Lock = new object();
		private static IUnityContainer _container;

		public static IUnityContainer Container
		{
			get
			{
				if (_container == null)
				{
					lock (Lock)
					{
						if (_container == null)
							_container = new Microsoft.Practices.Unity.UnityContainer();
					}
				}
				return _container;
			}
		}

		public static event Action ContainerReset = () => { };

		public static void ResetForUnitTestingPurposesOnly()
		{
			lock (Lock)
			{
				_container = null;
				ContainerReset();
			}
		}
	}


	public class WorkbenchScannerConvention : RegistrationConvention
	{
		private static Assembly GetAssembliesInBasePath()
		{
			return Assembly.GetEntryAssembly();
		}

		public override IEnumerable<Type> GetTypes()
		{
			var assembly = GetAssembliesInBasePath();

			return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ExportAttribute), false).Length > 0);
		}

		public override Func<Type, IEnumerable<Type>> GetFromTypes()
		{
			return OnGetFromTypes;
		}

		private static IEnumerable<Type> OnGetFromTypes(Type type)
		{
			return type.GetCustomAttributes<ExportAttribute>().Select(export => export.ContractType);
		}

		public override Func<Type, string> GetName()
		{
			return WithName.Default;
		}

		public override Func<Type, LifetimeManager> GetLifetimeManager()
		{
			return OnGetLifetimeManager;
		}

		private static LifetimeManager OnGetLifetimeManager(Type type)
		{
			PartCreationPolicyAttribute policy = type.GetCustomAttributes<PartCreationPolicyAttribute>().SingleOrDefault();
			if (policy != null && policy.CreationPolicy == CreationPolicy.Shared)
			{
				return WithLifetime.ContainerControlled(type);
			}
			return WithLifetime.Transient(type);
		}

		public override Func<Type, IEnumerable<InjectionMember>> GetInjectionMembers()
		{
			return OnGetInjectionMembers;
		}

		private static IEnumerable<InjectionMember> OnGetInjectionMembers(Type x)
		{
			return new List<InjectionMember>();

			/*if (x is ICommunicationObject)
			{
				yield return new InjectionConstructor();
			}*/
		}
	}
}
