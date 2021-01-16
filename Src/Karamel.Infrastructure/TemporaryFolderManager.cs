using System;
using System.IO;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// manages the temporary folder
    /// </summary>
    public class TemporaryFolderManager
    {
        #region Singleton

        /// <summary>
        /// private default constructor, creates the temporary folder if it doesn't exist,
        /// clears it, if it exists
        /// </summary>
        private TemporaryFolderManager()
        {
            if (Directory.Exists(GetTemporaryFolderPath()) == false)
            {
                Directory.CreateDirectory(GetTemporaryFolderPath());
            }
        }

        /// <summary>
        /// one and only instance
        /// </summary>
        private static TemporaryFolderManager _instance;

        /// <summary>
        /// the one and only instance, 
        /// </summary>
        public static TemporaryFolderManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TemporaryFolderManager();
                }
                return _instance;
            }
        }

        #endregion Singleton
        /// <summary>
        /// Gets the path for the temporary folder inside the application directory
        /// </summary>
        /// <returns></returns>
        public string GetTemporaryFolderPath()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return appDirectory + "Tmp\\";
        }

        /// <summary>
        /// Removes all files from the temporary folder
        /// </summary>
        public void ClearTemporaryFolder()
        {
            string[] temporaryFiles = Directory.GetFiles(GetTemporaryFolderPath());
            foreach (string temporaryFilePath in temporaryFiles)
            {
                File.Delete(temporaryFilePath);
            }
        }
    }
}
