using LePrAtos.Startup.Login;
using Microsoft.Practices.Unity;

namespace LePrAtos.Unity
{
	/// <summary>
	/// A Class which provides a <see cref="IUnityContainer"/>-Instance
	/// </summary>
	public static class UnityContainerProvider
	{
		private static IUnityContainer _container;

		/// <summary>
		/// The current <see cref="IUnityContainer"/>
		/// </summary>
		public static IUnityContainer Container => _container ?? (_container = new UnityContainer());

		/// <summary>
		/// Itializes the <see cref="Container"/>
		/// </summary>
		public static void InitializeContainer()
		{
			//TODO Fancy
			Container.RegisterType<ISession, Session>(new ExternallyControlledLifetimeManager());
			Container.RegisterType<ILoginViewModel, LoginViewModel>();
		}
	}
}
