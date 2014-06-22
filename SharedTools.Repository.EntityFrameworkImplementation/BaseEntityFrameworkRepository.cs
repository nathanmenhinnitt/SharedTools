namespace SharedTools.Repository.EntityFrameworkImplementation
{
    using System;
    using BaseEntities;
    using Interfaces;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseEntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public readonly DbContext Context;

        public DbSet<TEntity> DbSet;

        protected BaseEntityFrameworkRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public async Task AddAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            entity.IsActive = false;
            entity.DeletedDate = DateTime.Now;
            await Context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            Context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Context.SaveChangesAsync();
        }

        public TEntity FindById(int id)
        {
            return DbSet.FirstOrDefault(x => x.Id == id && x.DeletedDate == null);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null);
        }

        public IEnumerable<TEntity> FindAll()
        {
            return DbSet.Where(x => x.DeletedDate == null).ToArray();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await DbSet.Where(x => x.DeletedDate == null).ToArrayAsync();
        }

        public void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
    }
}
