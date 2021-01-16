using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using Business;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.ViewModel;
using Karamel.Infrastructure;
using Microsoft.Practices.Unity;
using Playlist.View;

namespace Playlist.ViewModel
{
    /// <summary>
    /// View model for the playlist
    /// </summary>
    public class PlaylistViewModel : NotificationObject, IPlaylistViewModel, IPlaylistService, IDropTarget
    {
        #region Attributes

        /// <summary>
        /// list of items witin the playlist
        /// </summary>
        private readonly ObservableCollection<PlaylistItem> _playlistItems;

        /// <summary>
        /// unity container
        /// </summary>
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// service for the media library
        /// </summary>
        private IMediaLibraryService _mediaLibraryService;

        /// <summary>
        /// Timer to refresh the "Time since Add)
        /// </summary>
        private readonly Timer _playlistItemRefreshTimer;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// View Model for the playlist hosting a list of playlist items
        /// </summary>
        /// <param name="playlistView">The playlist View</param>
        /// <param name="unityContainer">unity container</param>
        public PlaylistViewModel(IPlaylistView playlistView, IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;

            _playlistItems = new ObservableCollection<PlaylistItem>();
            View = playlistView;
            View.ViewModel = this;

            _playlistItemRefreshTimer = new Timer(RefreshTimeSinceAdd);
            _playlistItemRefreshTimer.Change(0, 10000);
        }

        #endregion Constructor

        #region Properties


        /// <summary>
        /// connected view
        /// </summary>
        public IView View { get; set; }

        /// <summary>
        /// List of playlist items (observable)
        /// </summary>
        public ObservableCollection<PlaylistItem> PlaylistItems => _playlistItems;

        /// <summary>
        /// service for the media library
        /// </summary>
        public IMediaLibraryService MediaLibraryService
        {
            get
            {
                if (_mediaLibraryService == null)
                {
                    _mediaLibraryService = _unityContainer.Resolve<IMediaLibraryService>();
                }
                return _mediaLibraryService;
            }
        }

        #endregion Properties

        #region Implementation of IPlaylistService

        /// <summary>
        /// Adds a single song in Simple (without singer) or KaraokeBar (with singer) mode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="startPlayingIfEmptyPlaylist"></param>
        /// <param name="singer">singer or null</param>
        public void EnqueueSong(Song song, bool startPlayingIfEmptyPlaylist = false, Singer singer = null)
        {
            PlaylistItem playlistItem = new PlaylistItem(song, singer);
            PlaylistItems.Add(playlistItem);
            if (PlaylistItems.Count == 1 && startPlayingIfEmptyPlaylist)
            {
                PlayItem(playlistItem);
            }
        }

        /// <summary>
        /// Adds a song which is performed by a list of singers in karaokeBarMode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="singers">singers performing the song</param>
        public void EnqueueSong(Song song, IEnumerable<Singer> singers)
        {
            PlaylistItem playlistItem = new PlaylistItem(song, singers);
            PlaylistItems.Add(playlistItem);

            SelectLastPlaylistItemInGrid();
        }

        /// <summary>
        /// Selects the last playlist item inside the grid
        /// </summary>
        private void SelectLastPlaylistItemInGrid()
        {
            if (PlaylistItems.Count > 0)
            {
                ((IPlaylistView) View).SelectItem(PlaylistItems[PlaylistItems.Count - 1]);
            }
        }

        /// <summary>
        /// Adds a number of songs without singers within the simple mode
        /// </summary>
        /// <param name="songs">songs that should be enqueued</param>
        public void EnqueueSongs(List<Song> songs)
        {
            foreach (Song song in songs)
            {
                PlaylistItem playlistItem = new PlaylistItem(song);
                PlaylistItems.Add(playlistItem);
            }
            SelectLastPlaylistItemInGrid();
        }

        /// <summary>
        /// Insert songs into the playlist at a given index
        /// </summary>
        /// <param name="insertIndex">index inside playlistItems</param>
        /// <param name="songs">songs to be inserted</param>
        private void InsertSongs(int insertIndex, List<Song> songs)
        {
            foreach (Song song in songs)
            {
                PlaylistItem playlistItem = new PlaylistItem(song);
                PlaylistItems.Insert(insertIndex++, playlistItem);
            }
        }

        /// <summary>
        /// Inserts a song into the playlist at a given index
        /// </summary>
        /// <param name="insertIndex">index inside playlistItems</param>
        /// <param name="song">song to be inserted</param>
        private void InsertSong(int insertIndex, Song song)
        {
            PlaylistItem playlistItem = new PlaylistItem(song);
            PlaylistItems.Insert(insertIndex, playlistItem);
        }

        /// <summary>
        /// Gets the next playlist item
        /// </summary>
        /// <param name="randomPlayback">
        ///     when the playlist is empty should a random song be played?
        /// </param>
        /// <param name="currentPlaylistItem"></param>
        /// <param name="fetchFromMediaLibraryIfEndReached">
        /// fetches the next song from the media library if the end of the playlist has been reached
        /// </param>
        /// <returns>
        /// the next playlist item
        /// </returns>
        public PlaylistItem GetNextPlaylistItem(bool randomPlayback, PlaylistItem currentPlaylistItem, bool fetchFromMediaLibraryIfEndReached)
        {
            int idxOfPlaylistItem = -1;
            if (_playlistItems.Count > 0 && currentPlaylistItem != null)
            {
                idxOfPlaylistItem = _playlistItems.IndexOf(currentPlaylistItem);
            }
            if (fetchFromMediaLibraryIfEndReached)
            {
                bool lastItemSelected = idxOfPlaylistItem == _playlistItems.Count - 1;
                if ((PlaylistItems.Count == 0 || lastItemSelected))
                {
                    FetchNextSongFromMediaLibrary(randomPlayback);
                }
            }

            if (idxOfPlaylistItem < _playlistItems.Count - 1)
            {
                PlaylistItem nextPlaylistItem = _playlistItems[idxOfPlaylistItem + 1];
                if (SolutionWideSettings.Instance.RemoveSongFromPlaylistAfterFetch)
                {
                    _playlistItems.Remove(nextPlaylistItem);
                }
                else
                {
                    ((IPlaylistView)View).SelectItem(nextPlaylistItem);
                }
                return nextPlaylistItem;
            }

            return null;
        }

        /// <summary>
        /// Fetches the next song from the media library - either by random or next to the selected item
        /// </summary>
        /// <param name="randomPlayback"></param>
        private void FetchNextSongFromMediaLibrary(bool randomPlayback)
        {
            if (MediaLibraryService != null)
            {
                Song nextSong;
                if (randomPlayback)
                {
                    nextSong = MediaLibraryService.GetRandomNextSong();
                }
                else
                {
                    nextSong = MediaLibraryService.GetNextSong();
                }
                EnqueueSong(nextSong);
            }
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
            int idxOfPlaylistItem = _playlistItems.IndexOf(currentPlaylistItem);
            if (idxOfPlaylistItem > 0)
            {
                PlaylistItem previousSong = _playlistItems[idxOfPlaylistItem - 1];
                ((IPlaylistView) View).SelectItem(previousSong);
                return previousSong;
            }
            return null;
        }


        /// <summary>
        /// Checks whether there is a playlist item before the current one
        /// </summary>
        /// <param name="currentPlaylistItem"></param>
        /// <returns></returns>
        public bool HasPreviousPlaylistItem(PlaylistItem currentPlaylistItem)
        {
            int idxOfPlaylistItem = _playlistItems.IndexOf(currentPlaylistItem);
            return idxOfPlaylistItem > 0;
        }

        /// <summary>
        /// Clears the playlist
        /// </summary>
        public void ClearPlaylist()
        {
            PlaylistItems.Clear();
        }

        #endregion Implementation of IPlaylistService

        /// <summary>
        /// As a song is dragged over it should be shown that it is copied to the playlist.
        /// If a PlaylistItem is dragged it should be shown that it is moved inside the list.
        /// </summary>
        /// <param name="dropInfo"></param>
        public void DragOver(IDropInfo dropInfo)
        {
            // drag within this grid
            PlaylistItem sourcePlaylistItem = dropInfo.Data as PlaylistItem;
            if (sourcePlaylistItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.DragInfo.Effects = DragDropEffects.Move;
            }

            // drag from media library
            Song sourceSongItem = dropInfo.Data as Song;
            if (sourceSongItem != null || dropInfo.Data is List<Song>)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DragInfo.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// If a playlist item is dropped it shall be moved to its new position.
        /// If a song is dropped it shall be added to the playlist
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            // drag within grid
            PlaylistItem sourcePlaylistItem = dropInfo.Data as PlaylistItem;
            if (sourcePlaylistItem != null)
            {
                int oldIdx = _playlistItems.IndexOf(sourcePlaylistItem);
                _playlistItems.Remove(sourcePlaylistItem);
                if (dropInfo.InsertIndex > oldIdx)
                {
                    _playlistItems.Insert(dropInfo.InsertIndex - 1, sourcePlaylistItem);
                }
                else
                {
                    _playlistItems.Insert(dropInfo.InsertIndex, sourcePlaylistItem);
                }
            }

            // drag from media library
            Song song = dropInfo.Data as Song;
            if (song != null)
            {
                InsertSong(dropInfo.InsertIndex, song);
            }
            else if (dropInfo.Data is List<Song>)
            {
                InsertSongs(dropInfo.InsertIndex, dropInfo.Data as List<Song>);
            }
        }
        
        #region Commands

        /// <summary>
        /// Plays a playlist item
        /// Additionall removes it from the playlist if RemoveSongFromPlaylistAfterFetch is true
        /// </summary>
        /// <param name="selectedItem"></param>
        public void PlayItem(PlaylistItem selectedItem)
        {
            IPlayerService playerService = _unityContainer.Resolve<IPlayerService>();
            if (playerService != null)
            {
                playerService.Play(selectedItem);
                if (SolutionWideSettings.Instance.RemoveSongFromPlaylistAfterFetch)
                {
                    _playlistItems.Remove(selectedItem);
                }
            }
        }

        #endregion Commands

        #region Refresh Time Since Add

        /// <summary>
        /// Refreshes the time since the playlist item was added
        /// </summary>
        /// <param name="state"></param>
        private void RefreshTimeSinceAdd(object state)
        {
            try
            {
                foreach (PlaylistItem playlistItem in PlaylistItems)
                {
                    playlistItem.UpdateTimeSinceItemWasAdded();
                }
            }
            catch
            {
                // not important if the collection is changed while the update runs
                // will be updated in the next 10 seconds
            }
        }

        #endregion Refresh Time Since Add
    }
}
