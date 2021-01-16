using System.Collections.Generic;
using System.IO;
using Business;
using Karamel.Infrastructure;
using Microsoft.Win32;

namespace MediaLibrary.Utilities
{
    /// <summary>
    /// exports the whole media library to two csv files (artist to title and title to artist
    /// </summary>
    internal static class CsvExporter
    {
        /// <summary>
        /// exports songs to two csv files
        /// </summary>
        /// <param name="songs"></param>
        internal static void ExportSongs(IList<Song> songs)
        {
            string fileName = SelectFilename();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            string fileNameWithoutExtension = fileName.Substring(0, fileName.Length - 4);
            string artistTitleFilename = fileNameWithoutExtension + "-ArtistTitle.csv";
            string titleArtistFilename = fileNameWithoutExtension + "-TitleArtist.csv";

            var artistTitlesDict = ConstructArtistTitlesDict(songs);

            var artistTitleList = new List<string>();
            var titleArtistList = new List<string>();
            foreach (KeyValuePair<string, List<string>> artistTitlesPair in artistTitlesDict)
            {
                string artist = artistTitlesPair.Key;
                List<string> titles = artistTitlesPair.Value;
                foreach (string title in titles)
                {
                    artistTitleList.Add(artist + ":" + title);
                    titleArtistList.Add(title + ":" + artist);
                }
            }

            artistTitleList.Sort();
            titleArtistList.Sort();

            DeleteFileIfExists(artistTitleFilename);
            DeleteFileIfExists(titleArtistFilename);

            File.WriteAllLines(artistTitleFilename, artistTitleList);
            File.WriteAllLines(titleArtistFilename, titleArtistList);
        }

        /// <summary>
        /// Lets the user select a filename for the list file
        /// </summary>
        /// <returns></returns>
        private static string SelectFilename()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "Karaoke",
                DefaultExt = ".csv",
                Filter = "Comma Separated Value file (.csv)|*.csv",
            };
            if (string.IsNullOrEmpty(SolutionWideSettings.Instance.StandardListFolder) == false &&
                Directory.Exists(SolutionWideSettings.Instance.StandardListFolder))
            {
                dlg.InitialDirectory = SolutionWideSettings.Instance.StandardListFolder;
            }
            
            bool? dialogResult = dlg.ShowDialog();
            if (dialogResult.HasValue == false || dialogResult.Value == false)
            {
                return string.Empty;
            }

            SolutionWideSettings.Instance.StandardListFolder = Path.GetDirectoryName(dlg.FileName);
            return dlg.FileName;
        }

        /// <summary>
        /// Deletes a file if it exists already
        /// </summary>
        /// <param name="filename"></param>
        private static void DeleteFileIfExists(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        /// <summary>
        /// Constructs a dictionary mapping artist to titles
        /// </summary>
        /// <param name="songs">list of songs</param>
        /// <returns>dictionary mapping an artist to all his titles</returns>
        private static Dictionary<string, List<string>> ConstructArtistTitlesDict(IList<Song> songs)
        {
            Dictionary<string, List<string>> artistTitlesDict = new Dictionary<string, List<string>>();
            foreach (Song song in songs)
            {
                List<string> titles;
                if (artistTitlesDict.TryGetValue(song.Artist, out titles) == false)
                {
                    titles = new List<string>();
                    artistTitlesDict.Add(song.Artist, titles);
                }
                if (titles.Contains(song.Title) == false)
                {
                    titles.Add(song.Title);
                }
            }
            return artistTitlesDict;
        }
    }
}
