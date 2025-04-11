using BugTracker.Domain;

namespace BugTracker.Repository
{
    public interface IUserRepository : IRepository<int, User>
    {
        User? FindByUsername(string username);
    }
}
