using LegionCore.Core.Identity;

namespace LegionCore.Core.Models.Identity;

public class ApplicationUserRole
{
    public ApplicationUser ApplicationUser { get; set; }
    public ApplicationRole ApplicationRole { get; set; }
    public string ApplicationUserEmail { get; set; }
    public string ApplicationRoleName { get; set; }
}