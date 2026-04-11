using System.ComponentModel.DataAnnotations;

public class AddStaffViewModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Το τηλέφωνο πρέπει να περιέχει μόνο αριθμούς και να έχει μήκος 10 χαρακτήρες")]
    public required string PhoneNumber {get;set;}
    public string? Address { get; set; }
}