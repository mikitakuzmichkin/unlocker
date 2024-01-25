using Dainty.Analytics;
using Dainty.Analytics.Provider;

namespace PuzzleUnlocker.Services.Analytics
{
    public class PuAnalyticsController : DaintyAnalyticsBase, IPuAnalyticsController
    {
        protected override IDaintyAnalyticsProvider GetProdProvider()
        {
            throw new System.NotImplementedException();
        }
    }
}