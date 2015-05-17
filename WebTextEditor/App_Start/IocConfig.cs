using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using WebActivator;
using WebTextEditor;
using WebTextEditor.BLL.Services;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.Infrastructure;

[assembly: PostApplicationStartMethod(typeof (IocConfig), "Initialize")]

namespace WebTextEditor
{
    /// <summary>
    ///     Dependency injection initializer.
    /// </summary>
    public static class IocConfig
    {
        /// <summary>
        ///     Initialize the container and register it as MVC3 Dependency Resolver.
        /// </summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            var container = new Container();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        /// <summary>
        ///     Constructs a SignalR dependency resolver.
        /// </summary>
        /// <returns>Dependency resolver.</returns>
        public static IDependencyResolver GetDependencyResolver()
        {
            var container = new Container();

            InitializeContainer(container);

            container.Verify();

            return new SimpleInjectorResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            // Automapper engine
            container.Register(() => Mapper.Engine);

            // BLL
            container.Register<IDocumentsService, DocumentsService>();
            container.Register<IDocumentContentService, DocumentContentService>();
            container.Register<IDocumentCollaboratorsService, DocumentCollaboratorsService>();

            // DAL
            container.Register<IDocumentsRepository, DocumentsRepository>();
            container.Register<IDocumentContentRepository, DocumentContentRepository>();
            container.Register<IDocumentCollaboratorsRepository, DocumentCollaboratorsRepository>();

            // SignalR stuff
            container.Register<IDependencyResolver, SimpleInjectorResolver>();
            container.Register<IJavaScriptMinifier, NullJavaScriptMinifier>();
            container.Register<IJavaScriptProxyGenerator>(() =>
            {
                var resolver = container.GetInstance<IDependencyResolver>();
                return new DefaultJavaScriptProxyGenerator(resolver);
            });
            container.Register<IHubManager, DefaultHubManager>();
            container.Register<IHubActivator, DefaultHubActivator>();
            container.Register<IParameterResolver, DefaultParameterResolver>();
            container.Register<IProtectedData, DefaultProtectedData>();
        }
    }
}