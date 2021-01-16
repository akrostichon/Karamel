using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Business;
using Microsoft.Win32;

namespace MediaLibrary.Utilities
{
    /// <summary>
    /// Detects and reports duplicate songs
    /// </summary>
    internal static class DuplicateDetector
    {
        /// <summary>
        /// Detects and reports duplicate songs
        /// </summary>
        /// <param name="songs"></param>
        internal static void DetectDuplicates(IList<Song> songs)
        {
            Dictionary<string, List<string>> artistTitleFilePathsDict = new Dictionary<string, List<string>>();
            foreach (Song song in songs)
            {
                string artistTitle = song.Artist + ":" + song.Title;
                List<string> filePaths;
                if (artistTitleFilePathsDict.TryGetValue(artistTitle, out filePaths) == false)
                {
                    filePaths = new List<string>();
                    artistTitleFilePathsDict.Add(artistTitle, filePaths);
                }
                filePaths.Add(song.FilePath);
            }

            bool foundAtLeastOne = false;
            StringBuilder reportSb = new StringBuilder("The following duplicates have been found:");
            reportSb.AppendLine();
            List<string> sortedKeys = artistTitleFilePathsDict.Keys.ToList();
            sortedKeys.Sort();
            foreach (string artistTitle in sortedKeys)
            {
                List<string> filePaths = artistTitleFilePathsDict[artistTitle];
                if (filePaths.Count > 1)
                {
                    foundAtLeastOne = true;
                    string[] artistTitleArr = artistTitle.Split(':');
                    reportSb.AppendLine(artistTitleArr[0] + " - " + artistTitleArr[1]);
                    foreach (string filePath in filePaths)
                    {
                        reportSb.AppendLine("\t - " + filePath);
                    }
                }
            }

            if (foundAtLeastOne)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "DuplicatesReport",
                    DefaultExt = ".txt",
                    Filter = "Text file (.txt)|*.txt",
                };
                bool? dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    File.WriteAllText(saveFileDialog.FileName, reportSb.ToString());
                }
            }
            else
            {
                MessageBox.Show("No duplicates have been found.");
            }
        }
    }
}
