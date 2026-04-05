public class AddAccountViewModel
{
    public string UserId { get; set; } // Το ID του πελάτη στον οποίο ανήκει ο λογαριασμός
    public string CustomerName { get; set; } // Μόνο για εμφάνιση στη φόρμα
    public string IBAN { get; set; }
    public decimal InitialBalance { get; set; } = 0;
    public string BranchName { get; set; }
    public string AccountType { get; set; } // Π.χ. Ταμιευτηρίου, Τρεχούμενος
}