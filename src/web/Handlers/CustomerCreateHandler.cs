public class CustomerCreateHandler
{
  TableService<Customer> customerClient;
  HttpContextAccessor http;

  public CustomerCreateHandler(HttpContextAccessor _http, TableService<Customer> _customerClient)
  {
    customerClient = _customerClient;
    http = _http!;
  }

  public async Task<IResult> HandleAsync()
  {
    var request = await http.HttpContext!.Request.ReadFromJsonAsync<Customer>();

    var customer = await customerClient.AddAsync(request);

    return Results.Created($"/customers/{customer.PartitionKey}/{customer.RowKey}", customer);
  }
}
