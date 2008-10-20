using System;

namespace ProjectTracker.Library
{
    public interface IGenericTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}