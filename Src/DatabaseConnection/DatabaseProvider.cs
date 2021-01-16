using System.Collections.Generic;
using Business;

namespace DatabaseConnection
{
    public class DatabaseProvider
    {

        public void CreateDatabase()
        {
            
        }

        #region Songs

        /// <summary>
        /// Gets all songs from the data provider
        /// </summary>
        /// <returns>all songs in the media library</returns>
        public List<Song> GetAllSongs()
        {
            return new List<Song>();
        }

        /// <summary>
        /// Updates a song inside the database
        /// </summary>
        /// <param name="song">song to be updated</param>
        public void UpdateSong(Song song)
        {
        }

        /// <summary>
        /// Inserts a number of songs into the database
        /// </summary>
        /// <param name="songs">songs to be inserted</param>
        public void InsertSongs(IEnumerable<Song> songs)
        {
        }

        /// <summary>
        /// Deletes songs from the database
        /// </summary>
        /// <param name="songs">songs to be deleted</param>
        public void DeleteSongs(IEnumerable<Song> songs)
        {
        }

        #endregion Songs

        #region Singers

        /// <summary>
        /// Gets all singers from the database
        /// </summary>
        /// <returns>list of singers</returns>
        public List<Singer> GetAllSingers()
        {
            return new List<Singer>();
        } 

        /// <summary>
        /// Inserts a singer into the database
        /// </summary>
        /// <param name="singer">new singer</param>
        public void InsertSinger(Singer singer)
        {
        }

        /// <summary>
        /// Updates a singer within the database
        /// </summary>
        /// <param name="singer">singer to be updated</param>
        public void UpdateSinger(Singer singer)
        {
            
        }

        /// <summary>
        /// Deletes a singer from the database
        /// </summary>
        /// <param name="singer">singer to be deleted</param>
        public void DeleteSinger(Singer singer)
        {
            
        }
        
        #endregion Singers
    }
}
