public class TableService<T> where T : class, ITableEntity
{
  private readonly TableClient _tableClient;

  public TableService(TableServiceClient tableServiceClient, string tableName)
  {
    _tableClient = tableServiceClient.GetTableClient(tableName);
    _tableClient.CreateIfNotExists();
  }

  public async Task<T> AddAsync(T entity)
  {
    await _tableClient.AddEntityAsync(entity);

    return entity;
  }

  public async Task<T?> GetAsync(string partitionKey, string rowKey)
  {
    try
    {
      var response = await _tableClient.GetEntityIfExistsAsync<T>(partitionKey, rowKey);
      return response.Value;
    }
    catch (RequestFailedException)
    {
      return null;
    }
  }

  public async Task UpdateAsync(T entity)
  {
    await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
  }

  public async Task DeleteAsync(string partitionKey, string rowKey)
  {
    await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
  }
}
