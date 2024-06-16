using System;

namespace MongoDDD.Core.Exceptions
{
    public class ConcurrentUpdateException : Exception
    {
        public ConcurrentUpdateException() { }

        public ConcurrentUpdateException(string message) : base(message) { }

        public ConcurrentUpdateException(string message, Exception inner) : base(message, inner) { }

        public ConcurrentUpdateException(string aggregateName, int requestedVersion, int currentVersion) 
            : base($"Could not save {aggregateName} due to stale data. " +
                   $"Requested version {requestedVersion} while the current version is {currentVersion}.") 
        { 
        }
    }
}
