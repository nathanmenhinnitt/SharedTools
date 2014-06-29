namespace SharedTools.Repository.DapperImplementation
{
    using System;
    using System.Data;
    using Interfaces;

    public class DapperTransaction : IRepositoryTransaction
    {
        private readonly IDbTransaction _transaction;

        public DapperTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            _transaction = transaction;
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
