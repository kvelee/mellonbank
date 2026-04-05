using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MellonBank.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Identity;

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

    public async Task<IActionResult> CustomerList()
    {
        var allUsers = _userManager.Users.ToList();
        var customers = new List<ApplicationUser>();

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

    public IActionResult Index()
    {
        return View();
    }
}