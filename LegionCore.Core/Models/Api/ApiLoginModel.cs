using System.ComponentModel.DataAnnotations;

namespace LegionCore.Core.Models.Api;

public class ApiLoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}