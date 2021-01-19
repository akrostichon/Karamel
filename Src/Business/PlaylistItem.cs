using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business
{
    /// <summary>
    /// data object for a singer
    /// </summary>
    public class PlaylistItem : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Attributes

        /// <summary>
        /// Singer(s) of the enqueued song (may be empty)
        /// </summary>
        private readonly List<Singer> _singers = new List<Singer>();
        /// <summary>
        /// the enqueued song
        /// </summary>
        private readonly Song _song;
        /// <summary>
        /// whether the song is currently being played
        /// </summary>
        private bool _isCurrentlyPlaying;
        /// <summary>
        /// Time when the playlist item was added
        /// </summary>
        private DateTime _addedTime;
        /// <summary>
        /// Time passed since the song was added
        /// </summary>
        private string _timePassedSinceAdd;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor of the playlist item with either one or no specified singer.
        /// Within the karaoke-bar mode there has to be at least one singer.
        /// </summary>
        /// <param name="song">song to be sung</param>
        /// <param name="singer">either a single singer for the song or null</param>
        public PlaylistItem(Song song, Singer singer = null)
        {
            _song = song;
            if (singer != null)
            {
                _singers.Add(singer);
            }
            _addedTime = DateTime.Now;
            UpdateTimeSinceItemWasAdded();
        }

        /// <summary>
        /// Constructor of the playlist item with either one or no specified singer.
        /// Within the karaoke-bar mode there has to be at least one singer.
        /// </summary>
        /// <param name="song">song to be sung</param>
        /// <param name="singers">list of singers for the next song</param>
        public PlaylistItem(Song song, IEnumerable<Singer> singers)
        {
            _song = song;
            _singers.AddRange(singers);
            _addedTime = DateTime.Now;
            UpdateTimeSinceItemWasAdded();
        }

        public static explicit operator PlaylistItem(Song song)
        {
            return new PlaylistItem(song);
        }

        #endregion Constructor

        public void ResetTimeSinceAdd()
        {
            _addedTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "Song":
                        if (string.IsNullOrEmpty("_song"))
                        {
                            error = "Song required!";
                        }
                        break;
                }
                return error;
            }
        }

        /// <summary>
        /// Name of the singer shown in the next-singer display
        /// (may be non-unique)
        /// </summary>
        public Song Song => _song;
        

        /// <summary>
        /// Whether the playlist item is currently being played
        /// </summary>
        public bool IsCurrentlyPlaying
        {
            get
            {
                return _isCurrentlyPlaying;
            }
            set
            {
                if (value != IsCurrentlyPlaying)
                {
                    _isCurrentlyPlaying = value;
                    OnPropertyChanged("IsCurrentlyPlaying");
                }
            }
        }

        /// <summary>
        /// Time passed since the song was added
        /// </summary>
        public string TimePassedSinceAdd
        {
            get { return _timePassedSinceAdd; }
            set
            {
                _timePassedSinceAdd = value;
                OnPropertyChanged("TimePassedSinceAdd");
            }
        }

        #region Display helpers

        /// <summary>
        /// gets the singer screen names as a comma separated list
        /// </summary>
        /// <returns>screen names of singers as comma separated list</returns>
        public string GetSingersAsText()
        {
            StringBuilder sb = new StringBuilder(100);
            foreach (Singer singer in _singers)
            {
                sb.Append(singer.ScreenName + ", ");
            }
            return sb.ToString(0, sb.Length - 2);
        }

        /// <summary>
        /// updates the string representation of the time that passed
        /// since the playlist item has been added
        /// </summary>
        public void UpdateTimeSinceItemWasAdded()
        {
            TimeSpan timeSinceAdd = DateTime.Now - _addedTime;
            var timeSinceAddSb = new StringBuilder();
            if (timeSinceAdd.Hours > 0)
            {
                timeSinceAddSb.Append(timeSinceAdd.Hours);
                timeSinceAddSb.Append(" hour(s) ");
            }
            if (timeSinceAdd.Minutes > 0)
            {
                timeSinceAddSb.Append(timeSinceAdd.Minutes);
                timeSinceAddSb.Append(" minute(s) ");
            }
            else if (timeSinceAdd.TotalMinutes < 1)
            {
                timeSinceAddSb.Append("< 1 minute ");
            }

            TimePassedSinceAdd = timeSinceAddSb.ToString();
        }

        #endregion Display helpers
    }
}
