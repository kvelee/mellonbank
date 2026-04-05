using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class AddAccountViewModel
{
    public string AFM { get; set; }

    public string? CustomerName { get; set; } // Μόνο για εμφάνιση στη φόρμα
    public string IBAN { get; set; }
    public decimal InitialBalance { get; set; } = 0;
    public string BranchName { get; set; }
    public string AccountType { get; set; } // Π.χ. Ταμιευτηρίου, Τρεχούμενος
}