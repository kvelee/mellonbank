public class TransferViewModel
{
    public string FromIban { get; set; } // Ο λογαριασμός χρέωσης
    public string ToIban { get; set; }   // Ο λογαριασμός πίστωσης
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    
    // Λίστα με τους λογαριασμούς του χρήστη για το dropdown
    public List<BankAccount>? MyAccounts { get; set; } 
}