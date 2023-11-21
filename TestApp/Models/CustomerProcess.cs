namespace TestApp.Models;


public class CustomerProcess
{
    public string ApRegno { get; set; }
    public string Stage { get; set; }
    public string Name { get; set; }
    public string Prop { get; set; }
}

public class ResponseCustProcess
{
    public List<CustomerProcess> Data { get; set; }
}
