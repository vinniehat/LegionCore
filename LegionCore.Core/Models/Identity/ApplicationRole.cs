using Microsoft.AspNetCore.Identity;

namespace LegionCore.Core.Models.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public const string Admin = "Admin";
    public const string User = "User";
}