using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Business;
using Karamel.Infrastructure;
using MediaLibrary.Utilities;
using MediaLibrary.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace MediaLibrary.ViewModel
{
    /// <summary>
    /// View model for the media library
    /// </summary>
    public class MediaLibraryViewModel : NotificationObject, IMediaLibraryService, IMediaLibraryViewModel
    {
        #region Attributes

        /// <summary>
        /// list of items witin the media library
        /// </summary>
        private readonly ObservableCollection<Song> _mediaLibraryItems;

        /// <summary>
        /// list of currently selected items witin the media library
        /// </summary>
        private readonly ObservableCollection<Song> _selectedMediaLibraryItems = new ObservableCollection<Song>();

        /// <summary>
        /// Service for the playlist
        /// </summary>
        private readonly IPlaylistService _playlistService;
        
        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor which initiates the commands and sets the view
        /// </summary>
        /// <param name="mediaLibraryView">view</param>
        /// <param name="playlistService">playlist service from the unity container</param>
        public MediaLibraryViewModel(IMediaLibraryView mediaLibraryView, IPlaylistService playlistService)
        {
            View = mediaLibraryView;
            _playlistService = playlistService;

            AddToLibraryCommand = new DelegateCommand(AddToLibrary);
            RefreshLibraryCommand = new DelegateCommand(RefreshLibrary, CanRefreshLibrary);
            ExportListCommand = new DelegateCommand(ExportList);
            DetectDuplicatesCommand = new DelegateCommand(DetectDuplicates);
            EnqueueSelectedSongsCommand = new DelegateCommand<object>(EnqueueSelectedSongs);
            View.ViewModel = this;
            _mediaLibraryItems = ParseLibrary();
        }
        
        #endregion Constructor

        #region Properties

        /// <summary>
        /// media library view
        /// </summary>
        public IView View{ get; set; }

        /// <summary>
        /// List of media library items (observable)
        /// </summary>
        public ObservableCollection<Song> MediaLibraryItems
        {
            get
            {
                return _mediaLibraryItems;
            }
        }

        /// <summary>
        /// List of media library items (observable)
        /// </summary>
        public IList<Song> SelectedMediaLibraryItems { get; set; }
        
        #endregion Properties

        #region Commands

        /// <summary>
        /// Delegate command for adding songs to the library
        /// </summary>
        public DelegateCommand AddToLibraryCommand { get; set; }

        /// <summary>
        /// Adds folders to the library
        /// </summary>
        private void AddToLibrary()
        {
            List<string> oldSelectedFolders = SolutionWideSettings.Instance.LibraryFolderPaths;
            List<string> selectedFolders = FolderSelectionViewModel.ShowFolderSelectionAsDialog(oldSelectedFolders);

            if (oldSelectedFolders == null || oldSelectedFolders.SequenceEqual(selectedFolders) == false)
            {
                SolutionWideSettings.Instance.LibraryFolderPaths = selectedFolders;
                _mediaLibraryItems.Clear();

                RefreshLibraryCommand.RaiseCanExecuteChanged();
                RefreshLibrary();
            }
        }
        
        /// <summary>
        /// Delegate command for adding songs to the library
        /// </summary>
        public DelegateCommand RefreshLibraryCommand { get; set; }

        /// <summary>
        /// Enqueues a lists of songs in the playlist
        /// </summary>
        /// <param name="selectedSongs"></param>
        /// <param name="startPlayingIfListIsEmpty"></param>
        public void Enqueue(IList selectedSongs, bool startPlayingIfListIsEmpty = false)
        {
            if (selectedSongs.Count == 1 && startPlayingIfListIsEmpty)
            {
                _playlistService.EnqueueSong(selectedSongs[0] as Song, true);
            }
            else
            {
                _playlistService.EnqueueSongs(selectedSongs.Cast<Song>().ToList());    
            }
        }

        /// <summary>
        /// Delegate command for exporting lists
        /// </summary>
        public DelegateCommand ExportListCommand { get; set; }

        /// <summary>
        /// Exports the whole mediaLibrary to a csv file
        /// </summary>
        private void ExportList()
        {
            CsvExporter.ExportSongs(_mediaLibraryItems.ToList());
        }

        /// <summary>
        /// Delegate command for detecting duplicates
        /// </summary>
        public DelegateCommand DetectDuplicatesCommand { get; set; }

        /// <summary>
        /// Detects duplicates inside the media library
        /// </summary>
        public void DetectDuplicates()
        {
            DuplicateDetector.DetectDuplicates(_mediaLibraryItems.ToList());
        }

        /// <summary>
        /// Enqueues one or multiple songs
        /// </summary>
        private void EnqueueSelectedSongs(object songsToEnqueue)
        {
            _playlistService.EnqueueSongs(_selectedMediaLibraryItems.ToList());
        }

        /// <summary>
        /// Command that enqueues one or multiple songs
        /// </summary>
        public DelegateCommand<object> EnqueueSelectedSongsCommand { get; set; }

        /// <summary>
        /// Clears the library
        /// </summary>
        private void RefreshLibrary()
        {
            Cursor originalCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ObservableCollection<Song> refreshedSongs = ParseLibrary();
                foreach (Song refreshedSong in refreshedSongs)
                {
                    int idxOfSong = _mediaLibraryItems.IndexOf(refreshedSong);
                    if (idxOfSong >= 0)
                    {
                        RefreshSongInfo(_mediaLibraryItems[idxOfSong], refreshedSong);
                    }
                    else
                    {
                        _mediaLibraryItems.Add(refreshedSong);
                    }
                }
                var songsToRemove =
                    _mediaLibraryItems.Where(oldSong => refreshedSongs.Contains(oldSong) == false).ToList();
                foreach (Song song in songsToRemove)
                {
                    _mediaLibraryItems.Remove(song);
                }
            }
            finally
            {
                Cursor.Current = originalCursor;
            }
        }

        /// <summary>
        /// Refreshes the artist title and extension of a song
        /// </summary>
        /// <param name="oldSong"></param>
        /// <param name="refreshedSong"></param>
        private void RefreshSongInfo(Song oldSong, Song refreshedSong)
        {
            oldSong.Artist = refreshedSong.Artist;
            oldSong.Extension = refreshedSong.Extension;
            oldSong.Title = refreshedSong.Title;
        }

        /// <summary>
        /// Parses the whole library
        /// </summary>
        /// <returns>a list of songs that have been found in the library</returns>
        private ObservableCollection<Song> ParseLibrary()
        {
            var songsInLibrary = new ObservableCollection<Song>();
            if (CanRefreshLibrary() == false)
            {
                return songsInLibrary;
            }

            return MediaLibraryParser.ParseLibrary();
            
        }

        /// <summary>
        /// Whether the library can be refreshed
        /// </summary>
        /// <returns>true if the library can be refreshed</returns>
        private bool CanRefreshLibrary()
        {
            if (SolutionWideSettings.Instance.LibraryFolderPaths.Count > 0)
            {
                return true;
            }
            return false;
        }
        
        #endregion Commands
        
        #region Implementation of IMediaLibraryService

        /// <summary>
        /// Gets a random next song
        /// </summary>
        /// <returns></returns>
        public Song GetRandomNextSong()
        {
            if (_mediaLibraryItems.Count == 0)
            {
                return null;
            }
            var rand = new Random();
            int randomEntryIdx = rand.Next(_mediaLibraryItems.Count - 1);
            return _mediaLibraryItems[randomEntryIdx];
        }


        /// <summary>
        /// Gets the next song inside the sequence
        /// </summary>
        /// <returns></returns>
        public Song GetNextSong()
        {
            if (_mediaLibraryItems.Count == 0)
            {
                return null;
            }
            return ((IMediaLibraryView) View).GetNextSongAfterSelection();
        }

        #endregion Implementation of IMediaLibraryService
    }
}
