namespace SharedTools.Repository.Interfaces
{
    using System;

    public interface IRepositoryTransaction : IDisposable
    {
        void CommitTransaction();
    }
}
