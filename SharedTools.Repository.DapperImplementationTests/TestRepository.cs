namespace SharedTools.Repository.DapperImplementationTests
{
    using Dapper;
    using DapperImplementation;

    public class TestRepository : BaseDapperRepository<TestEntity>
    {
        public override string ConnectionStringName
        {
            get { return "Test"; }
        }

        public override string TableName
        {
            get { return "AddTest"; }
        }

        public override string SelectQuery
        {
            get { return "SELECT * FROM [AddTest] WHERE [Id] = @id"; }
        }

        public override string InsertStatement
        {
            get { return "INSERT INTO [AddTest]([DateCreated],[CreatedByUserId],[LastUpdated],[LastUpdatedByUserId],[DeletedDate],[DeletedByUserId],[IsActive],[name])VALUES(@DateCreated,@CreatedByUserId,@LastUpdated,@LastUpdatedByUserId,@DeletedDate,@DeletedByUserId,@IsActive,@Name);"; }
        }

        public override string UpdateStatement
        {
            get { return "UPDATE [AddTest] SET [DateCreated] = @DateCreated ,[CreatedByUserId] = @CreatedByUserId ,[LastUpdated] = @LastUpdated ,[LastUpdatedByUserId] = @LastUpdatedByUserId ,[DeletedDate] = @DeletedDate ,[DeletedByUserId] = @DeletedByUserId ,[IsActive] = @IsActive ,[Name] = @Name WHERE [Id] = @Id"; }
        }

        public void HardDelete(TestEntity entity)
        {
            GetConnection().Execute("DELETE FROM AddTest WHERE Id = @id", new { id = entity.Id });
        }
    }
}
