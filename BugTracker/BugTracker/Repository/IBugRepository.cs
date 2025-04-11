using BugTracker.Domain;

namespace BugTracker.Repository
{
    public interface IBugRepository : IRepository<int, Bug>
    {
    }
}
