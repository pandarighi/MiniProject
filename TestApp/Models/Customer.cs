namespace TestApp.Models;

public class Customer
{
    public string ApRegno { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Alamat { get; set; }
    public string Ktp { get; set; }
    public string Prop { get; set; }
    public string Area { get; set; }
}

public class ResponseCust
{
    public List<Customer> Data { get; set; }
}
