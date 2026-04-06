using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Ο παλιός κωδικός είναι υποχρεωτικός")]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "Ο νέος κωδικός είναι υποχρεωτικός")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Ο κωδικός πρέπει να είναι τουλάχιστον 6 χαρακτήρες")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Οι κωδικοί δεν ταιριάζουν")]
    public string? ConfirmPassword { get; set; }
}