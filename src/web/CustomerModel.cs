public record Customer : ITableEntity
{
  public string PartitionKey { get; set; }
  public string RowKey { get; set; }
  public DateTimeOffset? Timestamp { get; set; }
  public ETag ETag { get; set; }

  public string FirstName { get; set; }
  public string LastName { get; set; }
  public int Age { get; set; } = 0;

  public Customer()
  {
  }

  public Customer(string lastName, string firstName)
  {
    PartitionKey = lastName;
    RowKey = firstName;
    FirstName = firstName;
    LastName = lastName;
  }
}
