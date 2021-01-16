using System.Collections.Generic;
using Business;
using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Playlist.View;
using Playlist.ViewModel;

namespace Playlist
{
    /// <summary>
    /// Module class for the playlist
    /// </summary>
    public class PlaylistModule : IModule, IPlaylistService
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
        public PlaylistModule(IUnityContainer container, IRegionManager regionManager)
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
            // every time this module is asked for an IPlaylistView (through resolve) it creates a PlaylistView
            _container.RegisterType<IPlaylistView, PlaylistView>();
            // every time it is asked for a IPlaylistViewModel it returns the singleton PlaylistViewModel
            _container.RegisterType<IPlaylistViewModel, PlaylistViewModel>(new ContainerControlledLifetimeManager());
            // every time it is asked for a IPlaylistService, it returns the singleton PlaylistViewModel
            _container.RegisterType<IPlaylistService, PlaylistViewModel>(new ContainerControlledLifetimeManager());

            // View Injection
            IRegion region = _regionManager.Regions[RegionNames.PlaylistRegion];
            var viewModel = _container.Resolve<IPlaylistViewModel>();
            region.Add(viewModel.View);
        }

        #endregion Public Methods

        #region Implementation of IPlaylistService

        /// <summary>
        /// Adds a single song in Simple (without singer) or KaraokeBar (with singer) mode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="startPlayingIfEmptyPlaylist"></param>
        /// <param name="singer">singer or null</param>
        public void EnqueueSong(Song song, bool startPlayingIfEmptyPlaylist = false, Singer singer = null)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            viewModel.EnqueueSong(song, singer: singer);
        }

        /// <summary>
        /// Adds a song which is performed by a list of singers in karaokeBarMode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="singers">singers performing the song</param>
        public void EnqueueSong(Song song, IEnumerable<Singer> singers)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            viewModel.EnqueueSong(song, singers);
        }

        /// <summary>
        /// Adds a number of songs without singers within the simple mode
        /// </summary>
        /// <param name="songs">songs that should be enqueued</param>
        public void EnqueueSongs(List<Song> songs)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            viewModel.EnqueueSongs(songs);
        }

        /// <summary>
        /// Gets the next playlist item
        /// </summary>
        /// <param name="randomPlayback"></param>
        /// <param name="currentPlaylistItem"></param>
        /// <param name="fetchFromMediaLibraryIfEndReached"></param>
        /// <returns>
        /// the next playlist item
        /// </returns>
        public PlaylistItem GetNextPlaylistItem(bool randomPlayback, PlaylistItem currentPlaylistItem, bool fetchFromMediaLibraryIfEndReached)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            return viewModel.GetNextPlaylistItem(randomPlayback, currentPlaylistItem, fetchFromMediaLibraryIfEndReached);
        }

        /// <summary>
        /// Gets the previous playlist item
        /// </summary>
        /// <param name="currentPlaylistItem"></param>
        /// <returns>
        /// the previous playlist item
        /// </returns>
        public PlaylistItem GetPreviousPlaylistItem(PlaylistItem currentPlaylistItem)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            return viewModel.GetPreviousPlaylistItem(currentPlaylistItem);
        }


        /// <summary>
        /// Checks whether there is a playlist item before the current one
        /// </summary>
        /// <param name="currentPlaylistItem"></param>
        /// <returns></returns>
        public bool HasPreviousPlaylistItem(PlaylistItem currentPlaylistItem)
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            return viewModel.HasPreviousPlaylistItem(currentPlaylistItem);
        }

        /// <summary>
        /// Clears the playlist
        /// </summary>
        public void ClearPlaylist()
        {
            var viewModel = _container.Resolve<PlaylistViewModel>();
            viewModel.ClearPlaylist();
        }

        #endregion Implementation of IPlaylistService
    }
}
