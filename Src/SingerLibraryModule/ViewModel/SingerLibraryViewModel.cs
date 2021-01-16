using System.Collections.ObjectModel;
using Karamel.Infrastructure;
using SingerLibrary.View;

namespace SingerLibrary.ViewModel
{
    public class SingerLibraryViewModel : ISingerLibraryViewModel
    {
        #region Attributes
        
        /// <summary>
        /// all singers 
        /// </summary>
        private readonly KaramelDatabaseContext _databaseContext = new KaramelDatabaseContext("Data Source=Karamel.sdf;Password=Some!KaraokePw");

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor using fields
        /// </summary>
        /// <param name="singerLibraryView"></param>
        public SingerLibraryViewModel(ISingerLibraryView singerLibraryView)
        {
            View = singerLibraryView;
        }

        #endregion Constructor

        #region Properties

        public IView View { get; set; }


        /// <summary>
        /// all known singers
        /// </summary>
        public KaramelDatabaseContext DatabaseContext => _databaseContext;

        #endregion Properties

        #region Methods

        /// <summary>
        /// saves a singer
        /// </summary>
        private void Save()
        {
            // somewhere there has to be a subscriber
            // _eventAggregator.GetEvent<SingerUpdatedEvent>().Subscribe(CalledEventHandler)

            // shared services

        }

        #endregion Methods
    }
}
