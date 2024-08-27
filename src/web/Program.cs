var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddScoped<HttpContextAccessor, HttpContextAccessor>();

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
app.MapPost("/customer-create", async (CustomerCreateHandler handler) => await handler.HandleAsync());
app.MapPost("/customer-list", async (CustomerListHandler handler) => await handler.HandleAsync());

app.Run();
