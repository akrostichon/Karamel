using System;
using System.ComponentModel;

namespace Business
{
    /// <summary>
    /// data object for a singer
    /// </summary>
    public class Singer : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

        /// <summary>
        /// unique identifier for the singer
        /// </summary>
        private int _id;
        /// <summary>
        /// Name of the singer shown in the next-singer display
        /// (may be non-unique)
        /// </summary>
        private string _screenName;
        /// <summary>
        /// private description of the singer (maybe last name)
        /// helps to identify singers
        /// </summary>
        private string _description;
        /// <summary>
        /// the last time this singer sang a song
        /// </summary>
        private DateTime _lastSong;
        /// <summary>
        /// number of songs sung today by this singer
        /// </summary>
        private int _songsSungToday;
        
        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
                    case "ScreenName" :
                        if (string.IsNullOrEmpty("_screenName"))
                        {
                            error = "Screen name required!";
                        }
                        break;
                }
                return error;
            }
        }

        /// <summary>
        /// unique identifier for the singer
        /// </summary>
        public int Id
        {
            get { return _id; }
            set 
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        /// <summary>
        /// Name of the singer shown in the next-singer display
        /// (may be non-unique)
        /// </summary>
        public string ScreenName
        {
            get { return _screenName; }
            set 
            {
                if (string.IsNullOrEmpty(_screenName) || _screenName.Equals(value) == false)
                {
                    _screenName = value;
                    OnPropertyChanged("ScreenName");
                }
            }
        }

        /// <summary>
        /// private description of the singer (maybe last name)
        /// helps to identify singers
        /// </summary>
        public string Description
        {
            get { return _description; }
            set 
            {
                if (string.IsNullOrEmpty(_description) || _description.Equals(value) == false)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// the last time this singer sang a song
        /// </summary>
        public DateTime LastSong
        {
            get { return _lastSong; }
            set
            {
                if (_lastSong.Equals(value) == false)
                {
                    _lastSong = value;
                    OnPropertyChanged("LastSong");
                }
            }
        }

        /// <summary>
        /// number of songs sung today by this singer
        /// </summary>
        public int SongsSungToday
        {
            get { return _songsSungToday; }
            set
            {
                if (_songsSungToday != value)
                {
                    _songsSungToday = value;
                    OnPropertyChanged("SongsSungToday");
                }
            }
        }
    }
}
