using BugTracker.Domain;
using System.Collections.Generic;

namespace BugTracker
{
    public interface IBugObserver
    {
        void OnBugListChanged(IEnumerable<Bug> bugs);
    }
}
