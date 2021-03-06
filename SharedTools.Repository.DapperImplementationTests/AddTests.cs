﻿namespace SharedTools.Repository.DapperImplementationTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading.Tasks;

    [TestClass]
    public class AddTests
    {
        public TestEntity Entity { get; set; }

        public TestRepository Repository { get; set; }

        [TestCleanup]
        public void Cleanup()
        {
            Repository.HardDelete(Entity);
            Repository.Dispose();
        }

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

            Repository = new TestRepository();
        }

        [TestMethod]
        public void should_add()
        {
            Repository.Add(Entity);
            Assert.IsTrue(Entity.Id > 0);
        }

        [TestMethod]
        public async Task should_addAsync()
        {
            await Repository.AddAsync(Entity);
            Assert.IsTrue(Entity.Id > 0);
        }
    }
}
