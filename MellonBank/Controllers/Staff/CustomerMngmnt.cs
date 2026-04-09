using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MellonBank.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*
    In this file, as explained at StaffController.cs, we implement the methods 
    for the Add/Edit/Delete Customer functionality.
*/

public partial class StaffController : Controller {

    // Returns the page(View) for the AddCustomer form
    [HttpGet]
    public IActionResult AddCustomer() => View();

    // Post method to Add a Customer
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

}