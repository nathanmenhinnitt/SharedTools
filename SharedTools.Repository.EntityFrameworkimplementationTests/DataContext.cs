namespace SharedTools.Repository.EntityFrameworkimplementationTests
{
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext()
            : base("Test")
        {
            
        }

        public IDbSet<TestEntity> TestEntities { get; set; }
    }
}
