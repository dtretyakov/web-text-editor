using AutoMapper;
using WebTextEditor.BLL.Mappings;

namespace WebTextEditor.BLL.Tests.Mocks
{
    public sealed class MockObjects
    {
        private static readonly IMappingEngine mapper;

        static MockObjects()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<DocumentsProfile>(); });
            mapper = Mapper.Engine;
        }

        /// <summary>
        ///     Returns a configured automapper instance.
        /// </summary>
        /// <returns>Automapper instance.</returns>
        public static IMappingEngine GetMapper()
        {
            return mapper;
        }
    }
}