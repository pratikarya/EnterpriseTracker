using EnterpriseTracker.Api;

namespace EnterpriseTracker.Api.Common
{
    public abstract class BaseRepository
    {
        protected EntrepriseTrackerDataContext GetDataContext()
        {
            return new EntrepriseTrackerDataContext();
        }
    }
}