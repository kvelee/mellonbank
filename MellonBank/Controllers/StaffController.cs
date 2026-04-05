using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MellonBank.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Staff")]
public class StaffController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public StaffController(UserManager<ApplicationUser> usermanager, ApplicationDbContext context)
    {
        _userManager = usermanager;
        _context = context;
    }

    [HttpGet]
    public IActionResult AddCustomer() => View();

    [HttpPost]
    public async Task<IActionResult> AddCustomer(AddCustomerViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.FirstName,
                LastName = model.LastName,
                AFM = model.AFM,
                Address = model.Address,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, "Customer");
                return RedirectToAction("Index"); 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> CustomerList(string searchAfm)
{
    var query = _userManager.Users.AsQueryable();

    if (!string.IsNullOrEmpty(searchAfm))
    {
        query = query.Where(u => u.AFM.Contains(searchAfm));
    }

    var customers = new List<ApplicationUser>();
    var allUsers = await query.ToListAsync();

    foreach (var user in allUsers)
    {
        if (await _userManager.IsInRoleAsync(user, "Customer"))
        {
            customers.Add(user);
        }
    }
    return View(customers);
}

    // GET: Staff/AddAccount?userId=...
    public async Task<IActionResult> AddAccount(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var model = new AddAccountViewModel
        {
            AFM = user.AFM,
            CustomerName = $"{user.Name} {user.LastName}",
            IBAN = "GR" + new Random().Next(10, 99) + "0172" + new Random().Next(1000, 9999) + "0000" + new Random().Next(1000, 9999)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount(AddAccountViewModel model)
    {
        ModelState.Remove("CustomerName");

        if (ModelState.IsValid)
        {
            var account = new BankAccount
            {
                IBAN = model.IBAN,
                Balance = model.InitialBalance,
                BranchName = model.BranchName,
                AccountType = model.AccountType,
                AFM = model.AFM
            };

            _context.BankAccounts.Add(account); // Τώρα αυτό θα δουλέψει!
            await _context.SaveChangesAsync();

            return RedirectToAction("CustomerList");
        }
        return View(model);
    }

    // GET: Staff/EditCustomer/999999999
    public async Task<IActionResult> EditCustomer(string afm)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.AFM == afm);
        if (user == null) return NotFound();

        // Χρησιμοποιούμε το ίδιο ViewModel με την εγγραφή ή ένα νέο EditCustomerViewModel
        return View(user); 
    }

    [HttpPost]
    public async Task<IActionResult> EditCustomer(ApplicationUser model)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.AFM == model.AFM);
        if (user == null) return NotFound();

        // Ενημέρωση στοιχείων
        user.Name = model.Name;
        user.LastName = model.LastName;
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber; // Το ζητάει η εκφώνηση

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("CustomerList");
        }

        return View(user);
    }

    // POST: Staff/DeleteCustomer/999999999
    [HttpPost]
    public async Task<IActionResult> DeleteCustomer(string afm)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.AFM == afm);
        if (user == null) return NotFound();

        // Προσοχή: Η διαγραφή χρήστη θα διαγράψει και τους λογαριασμούς του 
        // λόγω του Foreign Key (Cascade Delete)
        await _userManager.DeleteAsync(user);
        return RedirectToAction("CustomerList");
    }

    // GET: Staff/EditAccount/GR123...
    public async Task<IActionResult> EditAccount(string iban)
    {
        var account = await _context.BankAccounts.FindAsync(iban);
        if (account == null) return NotFound();
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> EditAccount(BankAccount model)
    {
        if (ModelState.IsValid)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("CustomerList");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount(string iban)
    {
        var account = await _context.BankAccounts.FindAsync(iban);
        if (account != null)
        {
            _context.BankAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("CustomerList");
    }

    public IActionResult Index()
    {
        return View();
    }
}