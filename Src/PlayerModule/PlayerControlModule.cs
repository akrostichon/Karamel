using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PlayerControl.View;
using PlayerControl.ViewModel;

namespace PlayerControl
{
    /// <summary>
    /// Module class for the MediaPlayer Control
    /// </summary>
    public class PlayerControlModule : IModule
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
        public PlayerControlModule(IUnityContainer container, IRegionManager regionManager)
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
            // register view and viewModel
            _container.RegisterType<IPlayerView, PlayerView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPlayerViewModel, PlayerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPlayerService, PlayerViewModel>(new ContainerControlledLifetimeManager());

            // get region and place new view in region
            IRegion region = _regionManager.Regions[RegionNames.PlayerControlRegion];
            IPlayerViewModel viewModel = _container.Resolve<IPlayerViewModel>();
            region.Add(viewModel.View);
        }

        #endregion Public Methods
    }
}
