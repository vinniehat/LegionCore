using Microsoft.AspNetCore.Identity;

namespace LegionCore.Core.Models.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public string Prefix { get; set; } = "";
    public string Description { get; set; } = "";
    public int Priority { get; set; } = 100;
}