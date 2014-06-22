namespace SharedTools.Repository.DapperImplementation
{
    using BaseEntities;
    using Dapper;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseDapperRepository<T> : IRepository<T> where T : BaseEntity
    {
        public abstract string ConnectionStringName { get; }
        public abstract string TableName { get; }
        public abstract string SelectQuery { get; }
        public abstract string InsertStatement { get; }
        public abstract string UpdateStatement { get; }

        private SqlTransaction _transaction;
        public SqlTransaction Transaction
        {
            get { return _transaction ?? (_transaction = _sqlConnection.BeginTransaction()); }
        }

        private SqlConnection _sqlConnection;

        public SqlConnection GetConnection()
        {
            _sqlConnection = _sqlConnection ?? (_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString));

            if (_sqlConnection.State != ConnectionState.Open)
            {
                if (string.IsNullOrEmpty(_sqlConnection.ConnectionString))
                {
                    _sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }

                _sqlConnection.Open();
            }

            return _sqlConnection;
        }

        public async Task<SqlConnection> GetConnectionAsync()
        {
            _sqlConnection = _sqlConnection ?? (_sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString));

            if (_sqlConnection.State != ConnectionState.Open)
            {
                if (string.IsNullOrEmpty(_sqlConnection.ConnectionString))
                {
                    _sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }

                await _sqlConnection.OpenAsync();
            }

            return _sqlConnection;
        }

        private string WrapInsertToReturnId()
        {
            const string declarationPart = "DECLARE @InsertedRows AS TABLE (Id int); ";
            const string outputPart = " OUTPUT Inserted.Id INTO @InsertedRows ";
            const string returnPart = "SELECT Id FROM @InsertedRows;";

            var injectedInsert = InsertStatement.Insert(InsertStatement.IndexOf(')') + 1, outputPart);

            return string.Format("{0} {1} {2}", declarationPart, injectedInsert, returnPart);
        }

        public void Add(T entity)
        {
            var connection = GetConnection();
            entity.Id = connection.Query<int>(WrapInsertToReturnId(), entity).FirstOrDefault();
        }

        public async Task AddAsync(T entity)
        {
            var connection = await GetConnectionAsync();
            entity.Id = (await connection.QueryAsync<int>(WrapInsertToReturnId(), entity)).FirstOrDefault();
        }

        public void Delete(T entity)
        {
            GetConnection().Execute(string.Format("UPDATE {0} SET [IsActive] = 0, [DeletedDate] = @now, [DeletedByUserId] = @userId WHERE Id = @id", TableName),
                                          new { now = DateTime.Now, userId = entity.DeletedByUserId, id = entity.Id });
        }

        public async Task DeleteAsync(T entity)
        {
            var connection = await GetConnectionAsync();
            await connection.ExecuteAsync(string.Format("UPDATE {0} SET [IsActive] = 0, [DeletedDate] = @now, [DeletedByUserId] = @userId WHERE Id = @id", TableName),
                                          new { now = DateTime.Now, userId = entity.DeletedByUserId, id = entity.Id });
        }

        public void Update(T entity)
        {
            GetConnection().Execute(UpdateStatement, entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var connection = await GetConnectionAsync();
            await connection.ExecuteAsync(UpdateStatement, entity);
        }

        public T FindById(int id)
        {
            return GetConnection().Query<T>(SelectQuery, new { id }).FirstOrDefault();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var connection = await GetConnectionAsync();
            return (await connection.QueryAsync<T>(SelectQuery, new { id })).FirstOrDefault();
        }

        public IEnumerable<T> FindAll()
        {
            return GetConnection().Query<T>("SELECT * FROM " + TableName);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            var connection = await GetConnectionAsync();
            return await connection.QueryAsync<T>("SELECT * FROM " + TableName);
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_sqlConnection != null)
            {
                _sqlConnection.Dispose();
            }
        }
    }
}
