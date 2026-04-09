public class DetailsCustomerViewModel
{
    public string FullName { get; set; }
    public string AFM { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public decimal UsdRate { get; set; }

    public List<CustomerAccountInfo> Accounts { get; set; } = new List<CustomerAccountInfo>();
}

public class CustomerAccountInfo
{
    public string IBAN { get; set; } 
    public string AccountType {get;set;}
    public decimal Balance { get; set; }
    public string Branch { get; set; } 
    public string Type { get; set; }        
}