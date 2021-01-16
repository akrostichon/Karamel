namespace Karamel.Infrastructure
{
    /// <summary>
    /// Interface for the view, holding the view model
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// View model belonging to the view
        /// </summary>
        IViewModel ViewModel { get; set; }
    }
}
