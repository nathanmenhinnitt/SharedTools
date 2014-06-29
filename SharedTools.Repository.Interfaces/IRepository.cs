namespace SharedTools.Repository.Interfaces
{
    using BaseEntities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        IRepositoryTransaction BeginTransaction();
        void Add(T entity);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        IEnumerable<T> FindAll();
        Task<IEnumerable<T>> FindAllAsync();
    }
}
