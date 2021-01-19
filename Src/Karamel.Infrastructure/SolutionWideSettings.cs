using System.Collections.Generic;
using Karamel.Infrastructure.Properties;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Singleton class for settings that are required from all over the solution
    /// </summary>
    public class SolutionWideSettings
    {
        #region Singleton

        /// <summary>
        /// private default constructor
        /// </summary>
        private SolutionWideSettings()
        {
            
        }

        /// <summary>
        /// one and only instance
        /// </summary>
        private static SolutionWideSettings _instance;

        /// <summary>
        /// the one and only instance
        /// </summary>
        public static SolutionWideSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SolutionWideSettings();
                }
                return _instance;
            }
        }

        #endregion Singleton

        #region Properties

        /// <summary>
        /// folder paths of all media library folders
        /// </summary>
        public List<string> LibraryFolderPaths
        {
            get
            {
                if (Settings.Default.LibraryFolderPaths == null)
                {
                    return new List<string>();
                }
                return Settings.Default.LibraryFolderPaths;
            }
            set
            {
                Settings.Default.LibraryFolderPaths = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Stops playback after the current song has finished
        /// </summary>
        public bool StopPlaybackAfterSong
        {
            get
            {
                return Settings.Default.StopPlaybackAfterSong;
            }
            set
            {
                Settings.Default.StopPlaybackAfterSong = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// SequentialPlayback or shuffling
        /// </summary>
        public bool SequentialPlayback
        {
            get
            {
                return Settings.Default.SequentialPlayback;
            }
            set
            {
                Settings.Default.SequentialPlayback = value;
                Settings.Default.Save();
            }
        }
        
        /// <summary>
        /// Whether the remaining or the played time is shown
        /// </summary>
        public bool ShowRemainingTime
        {
            get
            {
                return Settings.Default.ShowRemainingTime;
            }
            set
            {
                Settings.Default.ShowRemainingTime = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Simple or expert mode
        /// </summary>
        public bool ExpertMode
        {
            get
            {
                return Settings.Default.ExpertMode;
            }
            set
            {
                Settings.Default.ExpertMode = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// volume of the playback
        /// </summary>
        public double Volume
        {
            get
            {
                return Settings.Default.Volume;
            }
            set
            {
                Settings.Default.Volume = value;
                Settings.Default.Save();
            }
        }
        
        /// <summary>
        /// separator between artist and title inside the filename
        /// </summary>
        public string ArtistTitleSeparator
        {
            get
            {
                return Settings.Default.ArtistTitleSeparator;
            }
            set
            {
                Settings.Default.ArtistTitleSeparator = value;
                Settings.Default.Save();
            }
        }
        
        /// <summary>
        /// Whether the tag has a higher priority to determine the artist and title than the filename
        /// </summary>
        public bool ParseTagBeforeName
        {
            get
            {
                return Settings.Default.ParseTagBeforeName;
            }
            set
            {
                Settings.Default.ParseTagBeforeName = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Whether the artist is before the title inside the filename
        /// </summary>
        public bool ArtistBeforeTitle
        {
            get
            {
                return Settings.Default.ArtistBeforeTitle;
            }
            set
            {
                Settings.Default.ArtistBeforeTitle = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// standard folder for csv lists
        /// </summary>
        public string StandardListFolder
        {
            get
            {
                return Settings.Default.StandardListFolder;
            }
            set
            {
                Settings.Default.StandardListFolder = value;
                Settings.Default.Save();
            }
        }

        #endregion Properties
    }
}
