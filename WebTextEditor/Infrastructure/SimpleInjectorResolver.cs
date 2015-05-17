using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using SimpleInjector;

namespace WebTextEditor.Infrastructure
{
    /// <summary>
    ///     Simple injector dependency resolver.
    /// </summary>
    public class SimpleInjectorResolver : DefaultDependencyResolver
    {
        private readonly Container _container;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="container"></param>
        public SimpleInjectorResolver(Container container)
        {
            _container = container;
        }

        /// <summary>
        ///     Gets a service by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object GetService(Type type)
        {
            return base.GetService(type) ?? _container.GetInstance(type);
        }

        /// <summary>
        ///     Gets a services by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override IEnumerable<object> GetServices(Type type)
        {
            return base.GetServices(type) ?? _container.GetAllInstances(type);
        }
    }
}