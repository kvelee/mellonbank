using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MellonBank.Data;
using MellonBank.Models;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // 1. Βρίσκουμε τον τρέχοντα συνδεδεμένο χρήστη
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // 2. Φέρνουμε όλους τους λογαριασμούς που έχουν το ΑΦΜ του χρήστη
        var myAccounts = await _context.BankAccounts
            .Where(a => a.AFM == user.AFM)
            .ToListAsync();

        ViewBag.CustomerName = $"{user.Name} {user.LastName}";
        return View(myAccounts);
    }

    // ======== Tranfer Controllers ========
    [HttpGet]
    public async Task<IActionResult> Transfer()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new TransferViewModel
        {
            MyAccounts = await _context.BankAccounts.Where(a => a.AFM == user.AFM).ToListAsync()
        };
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Transfer(TransferViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        
        // 1. Έλεγχος αν ο λογαριασμός χρέωσης ανήκει όντως στον χρήστη
        var sourceAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.IBAN == model.FromIban && a.AFM == user.AFM);
        
        // 2. Έλεγχος αν υπάρχει ο λογαριασμός προορισμού
        var destinationAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.IBAN == model.ToIban);
    
        if (sourceAccount == null || destinationAccount == null)
        {
            ModelState.AddModelError("", "Ο λογαριασμός προορισμού δεν βρέθηκε ή η πηγή είναι άκυρη.");
        }
        else if (sourceAccount.Balance < model.Amount)
        {
            ModelState.AddModelError("", "Ανεπαρκές υπόλοιπο για τη συναλλαγή.");
        }
    
        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Αφαίρεση από την πηγή
                sourceAccount.Balance -= model.Amount;
                
                // Προσθήκη στον προορισμό
                destinationAccount.Balance += model.Amount;
    
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
    
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Σφάλμα κατά τη μεταφορά. Προσπαθήστε ξανά.");
            }
        }
    
        model.MyAccounts = await _context.BankAccounts.Where(a => a.AFM == user.AFM).ToListAsync();
        return View(model);
    }
}