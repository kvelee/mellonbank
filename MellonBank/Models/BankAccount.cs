using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BankAccount {
    [Key]
    public required string IBAN {get;set;}

    [Column(TypeName = "decimal(18,2)")] // Setting the number to 2 decimals
    public decimal Balance {get;set;} = 0;
    public string? BranchName {get;set;}
    public string? AccountType {get;set;}

    public required string AFM { get; set; }

    public required string UserId {get;set;}
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
}