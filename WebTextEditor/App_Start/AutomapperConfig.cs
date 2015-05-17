using AutoMapper;
using WebTextEditor.BLL.Mappings;

namespace WebTextEditor
{
    /// <summary>
    ///     Automapper configuration.
    /// </summary>
    public static class AutomapperConfig
    {
        /// <summary>
        ///     Perform mapping configuration.
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<DocumentsProfile>(); });
        }
    }
}