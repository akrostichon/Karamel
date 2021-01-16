using System.ComponentModel;

namespace Business
{
    /// <summary>
    /// data object for a singer
    /// </summary>
    public class Song : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

        /// <summary>
        /// unique identifier for the song
        /// </summary>
        private int _id;
        /// <summary>
        /// Name of the artist
        /// </summary>
        private string _artist;
        /// <summary>
        /// title of the song
        /// </summary>
        private string _title;
        /// <summary>
        /// path of the mp3/wmv/zip in the file system
        /// </summary>
        private string _filePath;
        /// <summary>
        /// path of the cdg file if this is a cdg,mp3 file combination
        /// </summary>
        private string _cdgFilePath;
        /// <summary>
        /// file extension (zip, mp3)
        /// </summary>
        private string _extension;
        /// <summary>
        /// number of times that this song was played (more than 50%)
        /// </summary>
        private int _timesPlayed;

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
                    case "Artist":
                        if (string.IsNullOrEmpty("_artist"))
                        {
                            error = "Artist required!";
                        }
                        break;
                    case "Title":
                        if (string.IsNullOrEmpty("_title"))
                        {
                            error = "Title required!";
                        }
                        break;
                }
                return error;
            }
        }

        /// <summary>
        /// unique identifier for the song
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
        /// Name of the artist
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            set
            {
                if (string.IsNullOrEmpty(_artist) || _artist.Equals(value) == false)
                {
                    _artist = value;
                    OnPropertyChanged("Artist");
                }
            }
        }

        /// <summary>
        /// title of the song
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrEmpty(_title) || _title.Equals(value) == false)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// path of the mp3/wmv/zip file
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (string.IsNullOrEmpty(_filePath) || _filePath.Equals(value) == false)
                {
                    _filePath = value;
                    OnPropertyChanged("FilePath");
                }
            }
        }


        /// <summary>
        /// path of the cdg file if this is a cdg,mp3 file combination
        /// </summary>
        public string CdgFilePath
        {
            get { return _cdgFilePath; }
            set
            {
                if (string.IsNullOrEmpty(_cdgFilePath) || _cdgFilePath.Equals(value) == false)
                {
                    _cdgFilePath = value;
                    OnPropertyChanged("CdgFilePath");
                }
            }
        }

        /// <summary>
        /// number of times that (over 50% of) this song has been played
        /// </summary>
        public int TimesPlayed
        {
            get { return _timesPlayed; }
            set
            {
                if (_timesPlayed != value)
                {
                    _timesPlayed = value;
                    OnPropertyChanged("TimesPlayed");
                }
            }
        }

        /// <summary>
        /// file extension (zip, mp3)
        /// </summary>
        public string Extension
        {
            get { return _extension; }
            set 
            {
                if (string.IsNullOrEmpty(_extension) || _extension.Equals(value) == false)
                {
                    _extension = value;
                    OnPropertyChanged("Extension");
                }
            }
        }

        /// <summary>
        /// a song is equal if it has the same file paths
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Song song2 = obj as Song;
            if (song2 != null)
            {
                bool equals = song2.FilePath.Equals(FilePath);
                if (equals && string.IsNullOrEmpty(song2.CdgFilePath) == false &&
                    string.IsNullOrEmpty(CdgFilePath) == false)
                {
                    equals = song2.CdgFilePath.Equals(CdgFilePath);
                }
                return equals;
            }
            return false;
        }

        /// <summary>
        /// hash code for a song
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(CdgFilePath))
            {
                return FilePath.GetHashCode();
            }
            return (FilePath + ":" + CdgFilePath).GetHashCode();    
        }
    }
}
