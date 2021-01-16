using Karamel.Infrastructure;
using MediaLibrary.View;
using MediaLibrary.ViewModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace MediaLibrary
{
    /// <summary>
    /// Module class for the Media library
    /// </summary>
    public class MediaLibraryModule : IModule
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
        public MediaLibraryModule(IUnityContainer container, IRegionManager regionManager)
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

            // every time this module is asked for an IMediaLibraryView (through resolve) it creates a MediaLibraryView
            _container.RegisterType<IMediaLibraryView, MediaLibraryView>();
            // every time it is asked for a I Media view model it creates a MediaLibrary view model
            _container.RegisterType<IMediaLibraryViewModel, MediaLibraryViewModel>(new ContainerControlledLifetimeManager());
            // every time it is asked for a IMediaLibraryService, it returns the singleton MediaLibraryViewModel
            _container.RegisterType<IMediaLibraryService, MediaLibraryViewModel>(new ContainerControlledLifetimeManager());

            // View Injection
            IRegion region = _regionManager.Regions[RegionNames.MediaLibraryRegion];
            IMediaLibraryViewModel viewModel = _container.Resolve<IMediaLibraryViewModel>();
            region.Add(viewModel.View);
        }


        #endregion Public Methods
    }
}
