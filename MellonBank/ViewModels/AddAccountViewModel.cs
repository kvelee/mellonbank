using System.ComponentModel.DataAnnotations;

public class AddAccountViewModel
{
    public required string AFM { get; set; }

    public string? CustomerName { get; set; } 
    public required string IBAN { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Το αρχικό ποσό πρέπει να είναι αριθμός και όχι αρνητικό")]
    [Required(ErrorMessage = "Πρέπει να εισάγετε ένα ποσό")]
    public decimal InitialBalance { get; set; } = 0;
    public string? BranchName { get; set; }
    public string? AccountType { get; set; }
}