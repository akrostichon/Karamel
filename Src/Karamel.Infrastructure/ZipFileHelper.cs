using System;
using Business;
using Ionic.Zip;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Helper for zip files
    /// </summary>
    public class ZipFileHelper : IDisposable
    {
        /// <summary>
        /// Extracts a file temporarily
        /// </summary>
        /// <param name="zipFilePath">path of the input zip file</param>
        /// <returns></returns>
        public Song ExtractFileTemporarily(string zipFilePath)
        {
            Song tmpSong = new Song();
            using (ZipFile zip = ZipFile.Read(zipFilePath))
            {
                ZipEntry mp3File = null;
                ZipEntry cdgFile = null;
                foreach (ZipEntry zipEntry in zip)
                {
                    if (zipEntry.FileName.EndsWith("mp3"))
                    {
                        if (mp3File == null)
                        {
                            mp3File = zipEntry;
                        }
                        else
                        {
                            // can only read zips which contain exactly one mp3 and one cdg file
                            mp3File = null;
                            break;
                        }
                    }
                    else if (zipEntry.FileName.EndsWith("cdg"))
                    {
                        if (cdgFile == null)
                        {
                            cdgFile = zipEntry;
                        }
                        else
                        {
                            // can only read zips which contain exactly one mp3 and one cdg file
                            cdgFile = null;
                            break;
                        }
                    }
                }

                if (mp3File != null && cdgFile != null)
                {
                    string tmpFolderPath = TemporaryFolderManager.Instance.GetTemporaryFolderPath();
                    mp3File.Extract(tmpFolderPath, ExtractExistingFileAction.OverwriteSilently);
                    cdgFile.Extract(tmpFolderPath, ExtractExistingFileAction.OverwriteSilently);
                    tmpSong.FilePath =  tmpFolderPath + mp3File.FileName;
                    tmpSong.CdgFilePath = tmpFolderPath + cdgFile.FileName;
                }
            }
            return tmpSong;
        }

        /// <summary>
        /// Disposes the ZipFileHelper which clears the temporary folder inside the application folder
        /// </summary>
        public void Dispose()
        {
            TemporaryFolderManager.Instance.ClearTemporaryFolder();
        }
    }
}
