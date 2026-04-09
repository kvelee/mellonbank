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

    public StaffController(UserManager<ApplicationUser> usermanager, ApplicationDbContext context)
    {
        _userManager = usermanager;
        _context = context;
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

    public IActionResult Index()
    {
        return View();
    }
}