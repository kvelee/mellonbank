using System.ComponentModel.DataAnnotations;

public class AddCustomerViewModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    [Required]
    [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Ο Αριθμός Φορολογικού Μητρώου (ΑΦΜ) πρέπει να περιέχει μόνο αριθμούς και να έχει μήκος 9git χαρακτήρες")]
    public required string AFM { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Το τηλέφωνο πρέπει να περιέχει μόνο αριθμούς και να έχει μήκος 10 χαρακτήρες")]
    public required string PhoneNumber {get;set;}
    public string? Address { get; set; }
}