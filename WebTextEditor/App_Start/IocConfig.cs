using System.Configuration;
using System.Web.Http;
using WindowsAzure.Table;
using WindowsAzure.Table.EntityConverters.TypeData;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using WebActivator;
using WebTextEditor;
using WebTextEditor.BLL.Services;
using WebTextEditor.DAL.Models;
using WebTextEditor.DAL.Repositories;
using WebTextEditor.DAL.Tables.Mappings;
using WebTextEditor.DAL.Tables.Repositories;
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
        ///     Performs type data initialization.
        /// </summary>
        static IocConfig()
        {
            EntityTypeMap.RegisterAssembly(typeof (DocumentMap).Assembly);
        }

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
            container.Register<IDocumentService, DocumentService>();
            container.Register<IDocumentContentService, DocumentContentService>();
            container.Register<IDocumentCollaboratorService, DocumentCollaboratorService>();

            // DAL
            container.Register<IDocumentRepository, DocumentRepository>();
            container.Register<IDocumentContentRepository, DocumentContentRepository>();
            container.Register<IDocumentCollaboratorRepository, DocumentCollaboratorRepository>();

            // DAL Azure Tables
            var storageConnection = ConfigurationManager.ConnectionStrings["AzureStorageConnection"];
            container.Register(() => CloudStorageAccount.Parse(storageConnection.ConnectionString).CreateCloudTableClient());
            container.Register<ITableSet<DocumentEntity>>(() =>
            {
                var table = new TableSet<DocumentEntity>(container.GetInstance<CloudTableClient>());
                table.CreateIfNotExists();
                return table;
            });
            container.Register<ITableSet<DocumentContentEntity>>(() =>
            {
                var table = new TableSet<DocumentContentEntity>(container.GetInstance<CloudTableClient>());
                table.CreateIfNotExists();
                return table;
            });
            container.Register<ITableSet<DocumentCollaboratorEntity>>(() =>
            {
                var table = new TableSet<DocumentCollaboratorEntity>(container.GetInstance<CloudTableClient>());
                table.CreateIfNotExists();
                return table;
            });

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
            container.Register(() => GlobalHost.ConnectionManager);
        }
    }
}