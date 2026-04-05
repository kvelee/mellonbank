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

    private readonly SignInManager<ApplicationUser> _signInManager;

    public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
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
        
        var sourceAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.IBAN == model.FromIban && a.AFM == user.AFM);
        
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
        else if (model.Amount <= 0)
        {
            ModelState.AddModelError("", "Cannot transfer this ammount (zero or negative ammount error)");
        }
        else if (sourceAccount == destinationAccount)
        {
            ModelState.AddModelError("","Cannot transfer to the same account");
        }
    
        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                sourceAccount.Balance -= model.Amount;
                destinationAccount.Balance += model.Amount;

                var log = new Transaction
                {
                    SourceIban = model.FromIban,
                    DestinationIban = model.ToIban,
                    Amount = model.Amount,
                    TransactionType = "Transfer",
                    Description = model.Description,
                    TransactionDate = DateTime.Now
                };

                _context.Transactions.Add(log);

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

    public async Task<IActionResult> History(string iban)
    {
        var user = await _userManager.GetUserAsync(User);
        // Επιβεβαίωση ότι ο λογαριασμός ανήκει στον χρήστη
        var account = await _context.BankAccounts.AnyAsync(a => a.IBAN == iban && a.AFM == user.AFM);
        if (!account) return Unauthorized();
    
        var transactions = await _context.Transactions
            .Where(t => t.SourceIban == iban || t.DestinationIban == iban)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    
        ViewBag.Iban = iban;
        return View(transactions);
    }

    [HttpGet]
public IActionResult ChangePassword() => View();

[HttpPost]
public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
{
    if (!ModelState.IsValid) return View(model);

    var user = await _userManager.GetUserAsync(User);
    if (user == null) return RedirectToAction("Index", "Home");

    // Η μέθοδος ChangePasswordAsync ελέγχει αν το OldPassword είναι σωστό
    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

    if (result.Succeeded)
    {
        // Αποσύνδεση και επανασύνδεση για να ανανεωθεί το security stamp
        await _signInManager.RefreshSignInAsync(user);
        TempData["SuccessMessage"] = "Ο κωδικός σας άλλαξε με επιτυχία!";
        return RedirectToAction("Index");
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError(string.Empty, error.Description);
    }

    return View(model);
}
}