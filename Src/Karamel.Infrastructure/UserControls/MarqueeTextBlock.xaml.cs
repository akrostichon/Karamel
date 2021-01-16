using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Karamel.Infrastructure.UserControls
{
    /// <summary>
    /// Interaction logic for MarqueeText.xaml
    /// </summary>
    public partial class MarqueeTextBlock : UserControl
    {
        #region Attributes
        
        /// <summary>
        /// Marquee speed
        /// </summary>
        private double _marqueeTimeInSeconds;
        
        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MarqueeTextBlock()
        {
            InitializeComponent();
            canMain.Height = Height;
            canMain.Width = Width;
            Loaded += MarqueeText_Loaded;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Setter for the marquee content
        /// </summary>
        public string Text
        {
            set { marqueeTextBlock.Text = value; }
        }
        
        /// <summary>
        /// Gets/Sets the marquee speed
        /// </summary>
        public double MarqueeTimeInSeconds
        {
            get { return _marqueeTimeInSeconds; }
            set { _marqueeTimeInSeconds = value; }
        }

        /// <summary>
        /// Gets/Sets the duration in seconds till the marquee starts marqueeing
        /// after its text is changed
        /// </summary>
        public double MarqueeStopTimeInBeginning { get; set; }
        
        #endregion Properties

        #region Events

        /// <summary>
        /// once the marquee has been loaded it should start its movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MarqueeText_Loaded(object sender, RoutedEventArgs e)
        {
            StartMarqueeing();
        }

        #endregion Events

        #region Marqueeing

        /// <summary>
        /// Starts a marquee from right to left
        /// </summary>
        private void StartMarqueeing()
        {
            double height = canMain.ActualHeight - marqueeTextBlock.ActualHeight;
            marqueeTextBlock.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation
            {
                From = -marqueeTextBlock.ActualWidth,
                To = canMain.ActualWidth,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds)),
                BeginTime = TimeSpan.FromSeconds(MarqueeStopTimeInBeginning)
            };
            marqueeTextBlock.BeginAnimation(Canvas.RightProperty, doubleAnimation);
            
            
        }
        
        #endregion Marqueeing
    }
}
