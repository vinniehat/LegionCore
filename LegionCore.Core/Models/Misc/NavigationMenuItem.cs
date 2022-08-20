namespace LegionCore.Core.Models.Misc;

public class NavigationMenuItem
{
    public string Name { get; set; }
    public bool IsHeader { get; set; }
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string[] Role { get; set; }
    public string Icon { get; set; }
    public string Link { get; set; }
    public List<NavigationMenuItem> Children { get; set; }
}