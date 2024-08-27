Console.WriteLine("Azure Storage Table - Getting Started sample\n");

var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_NAME");
var storageAccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_KEY");
var storageUri = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_URI") ?? $"https://{accountName}.table.core.windows.net";
var tableName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_TABLE_NAME") ?? "customers";

var tableClient = new TableClient(new Uri(storageUri), tableName, new TableSharedKeyCredential(accountName, storageAccountKey));

await tableClient.CreateIfNotExistsAsync();

Console.WriteLine("Table Storage is ready.");
