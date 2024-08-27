Console.WriteLine("Azure Storage Table - Getting Started sample\n");

var ReadFromTerminal = (string prompt) =>
{
  Console.Write(prompt);
  return Console.ReadLine();
};

var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_NAME") ?? ReadFromTerminal("Enter your Azure Storage Account name: ");
var storageAccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_KEY") ?? ReadFromTerminal("Enter your Azure Storage Account key: ");
var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={storageAccountKey};EndpointSuffix=core.windows.net";
var tableName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_TABLE_NAME") ?? ReadFromTerminal("Enter your Azure Storage Table name: ");

var tableClient = new TableClient(connectionString, tableName);

await tableClient.CreateIfNotExistsAsync();

Console.WriteLine("Table Storage is ready.");

var newCustomer = new Customer("Smith", "John")
{
  Age = 42
};

await tableClient.AddEntityAsync(newCustomer);
Console.WriteLine("Customer saved.");

var retrieved = await tableClient.GetEntityAsync<Customer>("Smith", "John");
var existingCustomer = retrieved.Value;
Console.WriteLine($"Retrieved customer: {existingCustomer.FirstName} {existingCustomer.LastName}, Age: {existingCustomer.Age}");

existingCustomer.Age = 43;
await tableClient.UpdateEntityAsync(existingCustomer, existingCustomer.ETag);
Console.WriteLine("Customer updated.");

Pageable<Customer> customers = tableClient.Query<Customer>(filter: $"PartitionKey eq '{existingCustomer.PartitionKey}'");
Console.WriteLine("Queried customers:");
foreach (var c in customers)
{
  Console.WriteLine($"- {c.FirstName}: {c.LastName}, Age: {c.Age}");
}
Console.WriteLine($"The query returned {customers.Count()} entities.");

await tableClient.DeleteEntityAsync(existingCustomer.PartitionKey, existingCustomer.RowKey, existingCustomer.ETag);
Console.WriteLine("Customer deleted.");

public class Customer : ITableEntity
{
  public string PartitionKey { get; set; }
  public string RowKey { get; set; }
  public DateTimeOffset? Timestamp { get; set; }
  public ETag ETag { get; set; }

  public string FirstName { get; set; }
  public string LastName { get; set; }

  public byte Age { get; set; } = 0;

  public Customer(string lastName, string firstName)
  {
    PartitionKey = lastName;
    RowKey = firstName;
    FirstName = firstName;
    LastName = lastName;
  }
}
