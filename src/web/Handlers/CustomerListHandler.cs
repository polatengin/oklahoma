public class CustomerListRequestModel
{
  public string PartitionKey { get; set; }
  public string RowKey { get; set; }
}

public class CustomerListHandler
{
  TableService<Customer> customerClient;
  HttpContextAccessor http;

  public CustomerListHandler(HttpContextAccessor _http, TableService<Customer> _customerClient)
  {
    customerClient = _customerClient;
    http = _http;
  }

  public async Task<IResult> HandleAsync()
  {
    var request = await http.HttpContext!.Request.ReadFromJsonAsync<CustomerListRequestModel>();

    var customer = await customerClient.GetAsync(request!.PartitionKey, request.RowKey);

    return Results.Ok(customer);
  }
}
