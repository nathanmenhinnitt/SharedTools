namespace SharedTools.Repository.EntityFrameworkimplementationTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading.Tasks;

    [TestClass]
    public class UpdateTests
    {
        public TestEntity Entity { get; set; }

        public TestRepository Repository { get; set; }

        [TestInitialize]
        public void Init()
        {
            Entity = new TestEntity
                {
                    Id = 0,
                    DateCreated = DateTime.Now,
                    CreatedByUserId = 1,
                    IsActive = true,
                    Name = "Some Test Name"
                };

            Repository = new TestRepository(new DataContext());

            Repository.Add(Entity);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Repository.HardDelete(Entity);
            Repository.Dispose();
        }

        [TestMethod]
        public async Task should_updateAsync()
        {
            var updatedName = Guid.NewGuid().ToString();

            Entity.Name = updatedName;

            await Repository.UpdateAsync(Entity);
            
            var updatedEntity = await Repository.FindByIdAsync(Entity.Id);

            Assert.AreEqual(updatedEntity.Name, updatedName);
        }

        [TestMethod]
        public void should_update()
        {
            var updatedName = Guid.NewGuid().ToString();

            Entity.Name = updatedName;

            Repository.Update(Entity);

            var updatedEntity = Repository.FindById(Entity.Id);

            Assert.AreEqual(updatedEntity.Name, updatedName);
        }
    }
}
