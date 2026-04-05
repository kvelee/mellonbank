using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MellonBank.Models;

public class HomeController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // Η Index θα σερβίρει τη φόρμα
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Staff")) return RedirectToAction("Index", "Staff");
            return RedirectToAction("Index", "Customer");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

            var userName = user != null ? user.UserName : model.Email;

            var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); 
            }

            ModelState.AddModelError("", "Λάθος στοιχεία σύνδεσης.");
        }
        return View(model);
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
}