Console.WriteLine("Azure Storage Table - Getting Started sample\n");

var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_NAME");
var storageAccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_KEY");
var storageUri = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_URI") ?? $"https://{accountName}.table.core.windows.net";
var tableName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_TABLE_NAME") ?? "customers";

var tableClient = new TableClient(new Uri(storageUri), tableName, new TableSharedKeyCredential(accountName, storageAccountKey));

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
