var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddSingleton<TableServiceClient>(sp =>
{
  var configuration = sp.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("StorageAccount");
  return new TableServiceClient(connectionString);
});

builder.Services.AddSingleton<TableService<Customer>>(sp =>
{
  var tableServiceClient = sp.GetRequiredService<TableServiceClient>();
  return new TableService<Customer>(tableServiceClient, "customers");
});

app.MapGet("/", () => "Hello World!");

app.Run();
