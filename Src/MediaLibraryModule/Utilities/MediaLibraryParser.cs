using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Dynamic;
using Business;
using Karamel.Infrastructure;
using MediaLibrary.Properties;

namespace MediaLibrary.Utilities
{
    /// <summary>
    /// class which parses the library folder and adds all songs (mp3+g or videos) to the media library
    /// </summary>
    public static class MediaLibraryParser
    {
        /// <summary>
        /// Parses the whole library
        /// </summary>
        /// <returns>a list of songs that have been found in the library</returns>
        public static ObservableCollection<Song> ParseLibrary()
        {
            var songsInLibrary = new ObservableCollection<Song>();

            foreach (string libraryFolderPath in SolutionWideSettings.Instance.LibraryFolderPaths)
            {
                string[] mp3Files = Directory.GetFiles(libraryFolderPath, "*.mp3", SearchOption.AllDirectories);
                string[] cdgFiles = Directory.GetFiles(libraryFolderPath, "*.cdg", SearchOption.AllDirectories);
                ParseMp3CdgFiles(cdgFiles, mp3Files, songsInLibrary);

                string[] wmvFiles = Directory.GetFiles(libraryFolderPath, "*.wmv", SearchOption.AllDirectories);
                ParseVideoFiles(wmvFiles, "wmv", songsInLibrary);
                string[] aviFiles = Directory.GetFiles(libraryFolderPath, "*.avi", SearchOption.AllDirectories);
                ParseVideoFiles(aviFiles, "avi", songsInLibrary);

                string[] zipFiles = Directory.GetFiles(libraryFolderPath, "*.zip", SearchOption.AllDirectories);
                ParseZipFiles(zipFiles, "zip", songsInLibrary);
            }

            return songsInLibrary;
        }


        /// <summary>
        /// Parses the given lists of video files and adds them to the library
        /// </summary>
        /// <param name="videoFiles"></param>
        /// <param name="extension">file extension</param>
        /// <param name="songsInLibrary">list is filled within this method</param>
        private static void ParseVideoFiles(IEnumerable<string> videoFiles, string extension, ObservableCollection<Song> songsInLibrary)
        {
            foreach (string videoFilePath in videoFiles)
            {
                var parsedSong = new Song { FilePath = videoFilePath, Extension = extension };

                ParseArtistAndTitle(videoFilePath, parsedSong);
                
                songsInLibrary.Add(parsedSong);
            }
        }

        
        /// <summary>
        /// Parses the given lists of video files and adds them to the library
        /// </summary>
        /// <param name="zipFiles"></param>
        /// <param name="extension">file extension</param>
        /// <param name="songsInLibrary">list is filled within this method</param>
        private static void ParseZipFiles(IEnumerable<string> zipFiles, string extension, ObservableCollection<Song> songsInLibrary)
        {
            foreach (string zipFilePath in zipFiles)
            {
                using (ZipFileHelper zipFileHelper = new ZipFileHelper())
                {
                    Song zipFileSong = new Song { FilePath = zipFilePath, Extension = extension };
                    Song tmpSong = zipFileHelper.ExtractFileTemporarily(zipFilePath);
                    if (string.IsNullOrEmpty(tmpSong.FilePath) == false &&
                        string.IsNullOrEmpty(tmpSong.CdgFilePath) == false)
                    {
                        ParseArtistAndTitle(tmpSong.FilePath, zipFileSong);
                        songsInLibrary.Add(zipFileSong);
                    }
                }
            }
        }
        
        /// <summary>
        /// Parses the given lists of mp3 and cdg files and adds them to the library
        /// </summary>
        /// <param name="cdgFiles"></param>
        /// <param name="mp3Files"></param>
        /// <param name="songsInLibrary">list is filled within this method</param>
        private static void ParseMp3CdgFiles(IEnumerable<string> cdgFiles, IEnumerable<string> mp3Files, ObservableCollection<Song> songsInLibrary)
        {
            var cdgDict = GetFilenameWithoutExtensionToCdgFileNameDict(cdgFiles);

            foreach (string mp3FilePath in mp3Files)
            {
                var parsedSong = new Song { FilePath = mp3FilePath, Extension = "mp3" };

                string pathWithoutExtension = mp3FilePath.Substring(0, mp3FilePath.LastIndexOf('.')).ToLowerInvariant();
                string cdgFilePath;
                if (cdgDict.TryGetValue(pathWithoutExtension, out cdgFilePath))
                {
                    parsedSong.CdgFilePath = cdgFilePath;
                    ParseArtistAndTitle(mp3FilePath, parsedSong);
                    songsInLibrary.Add(parsedSong);
                }
            }
        }


        /// <summary>
        /// Parses the song information from the media file
        /// </summary>
        /// <param name="mediaFilePath">path of the media file</param>
        /// <param name="songWithEmptyInfo">song into which the information should be filled</param>
        private static void ParseArtistAndTitle(string mediaFilePath, Song songWithEmptyInfo)
        {
            string mediaFileName = new FileInfo(mediaFilePath).Name;

            bool parsedSongInfo;

            if (SolutionWideSettings.Instance.ParseTagBeforeName)
            {
                parsedSongInfo = FillArtistAndTitleFromTagLib(mediaFilePath, songWithEmptyInfo);
                if (parsedSongInfo == false)
                {
                    parsedSongInfo = FillArtistAndTitleFromFilename(mediaFileName, songWithEmptyInfo);
                }
            }
            else
            {
                parsedSongInfo = FillArtistAndTitleFromFilename(mediaFileName, songWithEmptyInfo);
                if (parsedSongInfo == false)
                {
                    parsedSongInfo = FillArtistAndTitleFromTagLib(mediaFilePath, songWithEmptyInfo);
                }
            }
            
            if (parsedSongInfo == false)
            {
                FillTitleWithFullFilename(mediaFileName, songWithEmptyInfo);
            }
        }

        /// <summary>
        /// Tries to parse artist and title from filename. This only works if the artist and filename are separated by a dash
        /// and the artist comes before the title.
        /// </summary>
        /// <param name="fileName">file name of the media file</param>
        /// <param name="parsedSong">information on the parsed song. Caution: Extension has to be filled!</param>
        /// <returns>true if parsed successfully</returns>
        private static bool FillArtistAndTitleFromFilename(string fileName, Song parsedSong)
        {
            bool parsedFileName = false;
            string separator = SolutionWideSettings.Instance.ArtistTitleSeparator;

            var fileNameWithoutExt = RemoveExtension(fileName, parsedSong);

            string[] artistTitleArr = fileNameWithoutExt.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (artistTitleArr.Length == 2)
            {
                int idxOfArtist = SolutionWideSettings.Instance.ArtistBeforeTitle ? 0 : 1;
                int idxOfTitle = SolutionWideSettings.Instance.ArtistBeforeTitle ? 1 : 0;
                parsedSong.Artist = artistTitleArr[idxOfArtist].TrimEnd();
                parsedSong.Title = artistTitleArr[idxOfTitle].TrimStart();

                parsedFileName = true;
            }
            return parsedFileName;
        }

        /// <summary>
        /// Removes the extension from the file name,
        /// throws an exception if the extension inside the parsedSong is not set
        /// </summary>
        /// <param name="fileName">full filename</param>
        /// <param name="parsedSong">song that is currently being parsed, extension has to be set</param>
        /// <returns>file name without extension</returns>
        private static string RemoveExtension(string fileName, Song parsedSong)
        {
            string fileNameWithoutExt = fileName;
            if (string.IsNullOrEmpty(parsedSong.Extension))
            {
                string message = string.Format(Resources.Error_ParsedSongExtensionNotFilled, fileName);
                throw new ParseException(message, 1);
            }
            if (fileNameWithoutExt.ToLowerInvariant().EndsWith(parsedSong.Extension))
            {
                fileNameWithoutExt = fileNameWithoutExt.Substring(0, fileNameWithoutExt.Length - 4);
            }
            return fileNameWithoutExt;
        }

        /// <summary>
        /// Fills the artist and title for the parsed song from its ID tag
        /// </summary>
        /// <param name="mediaFilePath"></param>
        /// <param name="parsedSong"></param>
        /// <returns>true if the ID3 tag was filled</returns>
        private static bool FillArtistAndTitleFromTagLib(string mediaFilePath, Song parsedSong)
        {
            
            TagLib.File f = TagLib.File.Create(mediaFilePath);
            parsedSong.Artist = f.Tag.FirstAlbumArtist;
            if (string.IsNullOrEmpty(parsedSong.Artist) && f.Tag.Performers.Length > 0)
            {
                parsedSong.Artist = f.Tag.Performers[0];
            }

            parsedSong.Title = f.Tag.Title;

            bool idTagFilled = string.IsNullOrEmpty(parsedSong.Artist) == false &&
                               string.IsNullOrEmpty(parsedSong.Title) == false;
            return idTagFilled;
        }


        /// <summary>
        /// Sets the full filename as the title since all parsing mechanisms failed
        /// </summary>
        /// <param name="fileName">file name of the media file</param>
        /// <param name="parsedSong">information on the parsed song</param>
        private static void FillTitleWithFullFilename(string fileName, Song parsedSong)
        {
            parsedSong.Artist = string.Empty;
            parsedSong.Title = fileName;
            if (parsedSong.Title.ToLowerInvariant().EndsWith(parsedSong.Extension))
            {
                parsedSong.Title = parsedSong.Title.Substring(0, parsedSong.Title.Length - 4);
            }
        }

        /// <summary>
        /// Gets the dictionary mapping file paths without extension to their cdg filenames
        /// </summary>
        /// <param name="cdgFiles">all cdg files inside the directory</param>
        /// <returns>a dictionary mapping file paths without extension to their cdg filenames</returns>
        private static Dictionary<string, string> GetFilenameWithoutExtensionToCdgFileNameDict(IEnumerable<string> cdgFiles)
        {
            var cdgDict = new Dictionary<string, string>();
            foreach (string cdgFile in cdgFiles)
            {
                string pathWithoutExtension = cdgFile.Substring(0, cdgFile.LastIndexOf('.')).ToLowerInvariant();
                if (cdgDict.ContainsKey(pathWithoutExtension) == false)
                {
                    cdgDict.Add(pathWithoutExtension, cdgFile);
                }
            }
            return cdgDict;
        }
    }
}
