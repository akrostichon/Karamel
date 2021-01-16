using System.Windows;
using System.Windows.Controls;
using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Karamel
{
    /// <summary>
    /// Bootstrapper for the application
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Create the Shell
        /// </summary>
        /// <returns>A DependencyObject containing the shell</returns>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        /// <summary>
        /// Initializes the shell
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures the region adapter Mappings.
        /// </summary>
        /// <returns>region adapter mappings</returns>
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());
            return mappings;
        }

        /// <summary>
        /// Creates the Module catalog from the information given in App.config
        /// </summary>
        /// <returns>the module catalog</returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
