
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ApplicationUser : IdentityUser
{
    // IdentityUser already supports fields:
    // - Id
    // - UserName
    // - PhoneNumber
    // - Email
    // - PasswordHash

    public string? Name {get;set;}        
    public string? LastName {get;set;}
    public string? Address {get;set;}
    public string? AFM {get;set;}

    public ICollection<BankAccount>? BankAccounts {get;set;} // = new List<BankAccount>();


}