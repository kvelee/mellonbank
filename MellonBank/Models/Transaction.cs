using System.ComponentModel.DataAnnotations;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string SourceIban { get; set; }

    [Required]
    public required string DestinationIban { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public string? Description { get; set; }

    public string TransactionType { get; set; } // Π.χ. "Transfer", "Deposit", "Withdrawal"
}