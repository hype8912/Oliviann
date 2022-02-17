namespace Oliviann.Web.Api.Unity
{
    #region Usings

    using System;
    using global::Unity;
    using global::Unity.Lifetime;
    using global::Unity.Registration;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IUnityContainer"/>.
    /// </summary>
    public static class IUnityContainerExtensions
    {
        /// <summary>
        /// Registers a type with a singleton lifetime.
        /// </summary>
        /// <param name="container">The dependency container.</param>
        /// <param name="injectionMembers">Optional. The constructor injection
        /// parameters.</param>
        /// <returns>The <see cref="IUnityContainer"/> that called this method.
        /// </returns>
        public static IUnityContainer RegisterSingleton<T>(
            this IUnityContainer container,
            params InjectionMember[] injectionMembers) => container.RegisterSingleton<T>(null, injectionMembers);

        /// <summary>
        /// Registers a type with a singleton lifetime.
        /// </summary>
        /// <param name="container">The dependency container.</param>
        /// <param name="name">The name of the registration.</param>
        /// <param name="injectionMembers">Optional. The constructor injection
        /// parameters.</param>
        /// <returns>The <see cref="IUnityContainer"/> that called this method.
        /// </returns>
        public static IUnityContainer RegisterSingleton<T>(
            this IUnityContainer container,
            string name,
            params InjectionMember[] injectionMembers)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return container.RegisterType(null, typeof(T), name, new ContainerControlledLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Registers a type with a singleton lifetime.
        /// </summary>
        /// <typeparam name="TFrom">Type that will be requested.</typeparam>
        /// <typeparam name="TTo">Type that will be returned.</typeparam>
        /// <param name="container">The dependency container.</param>
        /// <param name="injectionMembers">Optional. The constructor injection
        /// parameters.</param>
        /// <returns>The <see cref="IUnityContainer"/> that called this method.
        /// </returns>
        public static IUnityContainer RegisterSingleton<TFrom, TTo>(
            this IUnityContainer container,
            params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            return container.RegisterSingleton<TFrom, TTo>(null, injectionMembers);
        }

        /// <summary>
        /// Registers a type with a singleton lifetime.
        /// </summary>
        /// <typeparam name="TFrom">Type that will be requested.</typeparam>
        /// <typeparam name="TTo">Type that will be returned.</typeparam>
        /// <param name="container">The dependency container.</param>
        /// <param name="name">The name of the registration.</param>
        /// <param name="injectionMembers">Optional. The constructor injection
        /// parameters.</param>
        /// <returns>The <see cref="IUnityContainer"/> that called this method.
        /// </returns>
        public static IUnityContainer RegisterSingleton<TFrom, TTo>(
            this IUnityContainer container,
            string name,
            params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return container.RegisterType(typeof(TFrom), typeof(TTo), name, new ContainerControlledLifetimeManager(), injectionMembers);
        }
    }
}