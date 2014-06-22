namespace SharedTools.Repository.EntityFrameworkimplementationTests
{
    using EntityFrameworkImplementation;

    public class TestRepository : BaseEntityFrameworkRepository<TestEntity>
    {
        public TestRepository(DataContext context)
            : base(context)
        {

        }

        public void HardDelete(TestEntity entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }
    }
}
