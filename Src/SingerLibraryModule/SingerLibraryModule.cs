using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using SingerLibrary.View;
using SingerLibrary.ViewModel;

namespace SingerLibrary
{
    /// <summary>
    /// Module class for the singer library
    /// </summary>
    public class SingerLibraryModule : IModule
    {
        #region Attributes

        /// <summary>
        /// unity container of this application
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// region manager of this application
        /// </summary>
        private readonly IRegionManager _regionManager;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="container">unity container</param>
        /// <param name="regionManager">region manager</param>
        public SingerLibraryModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Initialize the module by registering types of the module in the unity container and adding them to the correct region
        /// </summary>
        public void Initialize()
        {
            RegisterViewAndViewModel();
        }

        /// <summary>
        /// Registers the view and viewModel as the response objects for their interfaces
        /// </summary>
        private void RegisterViewAndViewModel()
        {
            _container.RegisterType<ISingerLibraryView, SingerLibraryView>();
            _container.RegisterType<ISingerLibraryViewModel, SingerLibraryViewModel>(new ContainerControlledLifetimeManager());
        }

        #endregion Public Methods
    }
}
