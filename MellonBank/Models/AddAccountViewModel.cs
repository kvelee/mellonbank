using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class AddAccountViewModel
{
    public required string AFM { get; set; }

    public string? CustomerName { get; set; } 
    public required string IBAN { get; set; }
    public decimal InitialBalance { get; set; } = 0;
    public string? BranchName { get; set; }
    public string? AccountType { get; set; }
}