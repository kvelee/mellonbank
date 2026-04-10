using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MellonBank.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*

    The StaffController class is separated in multiple files so we can
    better separation of logic. Each file corresponds with each Staff
    ability.
    Staff/CustomerMngmngt.cs --> Methods that are used by Staff to manage a customer entity (create,edit,delete)
    Staff/AccountMngmnt.cs --> Methods that are used by Staff to manage a customer's account (create,edit,delete)
    We have only kept the method CustomerList here, as explained below.
*/


[Authorize(Roles = "Staff")]
public partial class StaffController : Controller
{   

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ICurrencyService _currencyService;

    public StaffController(UserManager<ApplicationUser> usermanager
                          ,ApplicationDbContext context
                          ,ICurrencyService currencyService)
    {
        _userManager = usermanager;
        _context = context;
        _currencyService = currencyService;
    }

    /*
        We only keep the CustomerList method, since this is the main method being called.
        It's the first thing Staff users see, as they land to their page.
        
    */
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

    public async Task<IActionResult> DetailsCustomer(string id)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();

        var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (customer == null) return NotFound();

        var accounts = await _context.BankAccounts
            .Where(a => a.AFM == customer.AFM)
            .ToListAsync();

        decimal exchangeRate = await _currencyService.GetUsdRateAsync();

        var viewModel = new DetailsCustomerViewModel
        {
            FullName = $"{customer.Name} {customer.LastName}", // [cite: 38]
            AFM = customer.AFM, // [cite: 52]
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            UsdRate = exchangeRate, // Η τιμή από το API
            Accounts = accounts.Select(a => new CustomerAccountInfo
            {
                IBAN = a.IBAN, // [cite: 41]
                Balance = a.Balance, // [cite: 40]
                Branch = a.BranchName, // [cite: 41]
                AccountType = a.AccountType // [cite: 41]
            }).ToList()
        };

        return View(viewModel);
    }


    public IActionResult Index()
    {
        return View();
    }
}