using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class AppUser: IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}