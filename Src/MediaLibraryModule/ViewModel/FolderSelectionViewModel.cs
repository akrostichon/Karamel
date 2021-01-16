using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Karamel.Infrastructure;
using MediaLibrary.Properties;
using MediaLibrary.View;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace MediaLibrary.ViewModel
{
    /// <summary>
    /// view model for the folder selection
    /// </summary>
    internal class FolderSelectionViewModel : IViewModel
    {
        #region Attributes

        /// <summary>
        /// list of selected folders
        /// </summary>
        private readonly ObservableCollection<string> _selectedFolders = new ObservableCollection<string>();
        
        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor which initiates the commands and sets the view
        /// </summary>
        private FolderSelectionViewModel(IEnumerable<string> currentlySelectedFolders)
        {
            if (currentlySelectedFolders != null)
            {
                _selectedFolders.AddRange(currentlySelectedFolders);
            }

            AddFolderCommand = new DelegateCommand(AddFolder);
            RemoveFolderCommand = new DelegateCommand(RemoveCurrentlySelectedFolder);
            Separator = SolutionWideSettings.Instance.ArtistTitleSeparator;
            FileNameSchema = SolutionWideSettings.Instance.ArtistBeforeTitle
                ? NamingSchema.ArtistBeforeTitle
                : NamingSchema.TitleBeforeArtist;
            ParsingPriorities = SolutionWideSettings.Instance.ParseTagBeforeName
                ? ParsingPriority.TagBeforeFileName
                : ParsingPriority.FileNameBeforeTag;
            FolderSelectionView view = new FolderSelectionView();
            View = view;
            View.ViewModel = this;

        }

        #endregion Constructor

        #region Start Folder Selection

        /// <summary>
        /// Shows the folder selection dialog with the already selected folders 
        /// </summary>
        /// <param name="currentlySelectedFolders">folders that are now in the media folders list</param>
        /// <returns>list of currently selected folders</returns>
        public static List<string> ShowFolderSelectionAsDialog(List<string> currentlySelectedFolders)
        {
            FolderSelectionViewModel viewModel = new FolderSelectionViewModel(currentlySelectedFolders);
            Window view = viewModel.View as Window;
            bool? dialogResult = view?.ShowDialog();
            if (dialogResult != null && dialogResult.Value)
            {
                SolutionWideSettings.Instance.ArtistTitleSeparator = viewModel.Separator;
                SolutionWideSettings.Instance.ArtistBeforeTitle = viewModel.FileNameSchema ==
                                                                  NamingSchema.ArtistBeforeTitle;
                SolutionWideSettings.Instance.ParseTagBeforeName = viewModel.ParsingPriorities ==
                                                                   ParsingPriority.TagBeforeFileName;
                return viewModel._selectedFolders.ToList();
            }
            return currentlySelectedFolders;
        }

        #endregion Start Folder Selection

        #region Implementation of IFolderSelectionViewModel

        /// <summary>
        /// view for the folder selection
        /// </summary>
        public IView View { get; set; }
        
        /// <summary>
        /// selected folders for the media library
        /// </summary>
        public ObservableCollection<string> SelectedFolders => _selectedFolders;

        /// <summary>
        /// currently selected folder in the data grid
        /// </summary>
        public string CurrentlySelectedFolder {get; set; }

        /// <summary>
        /// parsing priority - which criterion gets the highest priority
        /// </summary>
        public ParsingPriority ParsingPriorities { get; set; }

        /// <summary>
        /// is the artist the first part or the second part of the filename
        /// </summary>
        public NamingSchema FileNameSchema { get; set; }

        /// <summary>
        /// Separator between artist and title
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// removes the currently selected folder from the list
        /// </summary>
        public DelegateCommand RemoveFolderCommand { get; set; }

        /// <summary>
        /// Removes the currently selected folder from the grid
        /// </summary>
        private void RemoveCurrentlySelectedFolder()
        {
            if (string.IsNullOrEmpty(CurrentlySelectedFolder) == false)
            {
                SelectedFolders.Remove(CurrentlySelectedFolder);
            }
        }
        
        /// <summary>
        ///  command for adding a folder - should show a standard folder selection dialog
        /// </summary>
        public DelegateCommand AddFolderCommand { get; set; }

        /// <summary>
        /// Adds a folder to the grid using the default folder browser dialog
        /// </summary>
        private void AddFolder()
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = Resources.SelectMediaLibraryFolder;
                DialogResult dialogResult = folderBrowser.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    SelectedFolders.Add(folderBrowser.SelectedPath);
                }
            }
        }


        #endregion Implementation of IFolderSelectionViewModel
    }
}
