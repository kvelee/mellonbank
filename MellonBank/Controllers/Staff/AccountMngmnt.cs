using Microsoft.AspNetCore.Mvc;

public partial class StaffController : Controller
{
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

            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction("CustomerList");
        }
        return View(model);
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

}