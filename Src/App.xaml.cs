using System;
using System.Windows;

namespace Karamel
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Override the OnStartup to use the Bootstrapper
        /// </summary>
        /// <param name="e">startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                Bootstrapper bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }
    }
}
