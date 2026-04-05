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
}