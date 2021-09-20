using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Todos.Services
{
    public class TableService<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public TableService(string connectionString)
        {
            var tableName = typeof(T).Name;
            _tableClient = new TableClient(connectionString, tableName);
        }

        public IAsyncEnumerable<Page<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string continuationToken = null, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            AsyncPageable<T> query;

            if (filter == null)
                query = _tableClient.QueryAsync<T>((string)null, pageSize, null, cancellationToken);
            else
                query = _tableClient.QueryAsync<T>(filter, pageSize, null, cancellationToken);

            return query.AsPages(continuationToken, pageSize);
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            var response = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey, null, cancellationToken);
            return response.Value;
        }

        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.PartitionKey = Guid.NewGuid().ToString();
            entity.RowKey = Guid.NewGuid().ToString();
            return _tableClient.AddEntityAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Merge, cancellationToken);
        }

        public Task DeleteAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            return _tableClient.DeleteEntityAsync(partitionKey, rowKey, ETag.All, cancellationToken);
        }
    }
}