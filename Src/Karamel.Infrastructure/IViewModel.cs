namespace Karamel.Infrastructure
{
    /// <summary>
    /// Interface for the viewModel which holds the view
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// View belonging to the view model
        /// </summary>
        IView View { get; set; }
    }
}
