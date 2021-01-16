using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Region Adapter for the Grid
    /// </summary>
    public class GridRegionAdapter : RegionAdapterBase<Grid>
    {
        /// <summary>
        /// Constructor calling base constructor
        /// </summary>
        /// <param name="regionBehaviorFactory"></param>
        public GridRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        /// <summary>
        /// Adapts the grid layout when a region is added or removed
        /// </summary>
        /// <param name="region">added or removed region</param>
        /// <param name="regionTarget">grid in which the region has to be placed</param>
        protected override void Adapt(IRegion region, Grid regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    {
                        foreach (FrameworkElement element in e.NewItems)
                        {
                            regionTarget.Children.Add(element);
                        }
                    }
                    //Removal
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        foreach (FrameworkElement element in e.OldItems)
                        {
                            if (regionTarget.Children.Contains(element))
                            {
                                regionTarget.Children.Remove(element);
                            }
                        }
                    }
                };
        }

        /// <summary>
        /// Creates a new region
        /// </summary>
        /// <returns></returns>
        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
