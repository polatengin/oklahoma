var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<TableServiceClient>(sp =>
{
  var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_NAME");
  var storageAccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_KEY");
  var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={storageAccountKey};EndpointSuffix=core.windows.net";

  return new TableServiceClient(connectionString);
});

builder.Services.AddSingleton<TableService<Customer>>(sp =>
{
  var tableServiceClient = sp.GetRequiredService<TableServiceClient>();
  return new TableService<Customer>(tableServiceClient, Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT_TABLE_NAME") ?? "customers");
});

builder.Services.AddScoped<CustomerCreateHandler>();
builder.Services.AddScoped<CustomerListHandler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/customer-create", async (CustomerCreateHandler handler) => await handler.HandleAsync());
app.MapPost("/customer-list", async (CustomerListHandler handler) => await handler.HandleAsync());

app.Run();
