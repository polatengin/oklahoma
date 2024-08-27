public class CustomerListHandler
{
  TableService<Customer> customerClient;
  HttpContext http;

  public CustomerListHandler(HttpContextAccessor _http, TableService<Customer> _customerClient)
  {
    customerClient = _customerClient;
    http = _http.HttpContext!;
  }

  public async Task<IResult> HandleAsync()
  {
    var customer = await http.Request.ReadFromJsonAsync<Customer>();
    if (customer == null)
    {
      return Results.BadRequest();
    }

    await customerClient.GetAsync(customer.PartitionKey, customer.RowKey);
    return Results.Created($"/customers/{customer.PartitionKey}/{customer.RowKey}", customer);
  }
}
