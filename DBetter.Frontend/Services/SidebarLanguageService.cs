using DBetter.Frontend.Layout;
using Microsoft.Extensions.Localization;

namespace DBetter.Frontend.Services;

public class SidebarLanguageService(IStringLocalizer<Sidebar> localizer)
{
    public string Home => localizer["Home"];
    
    public string Connection => localizer["Connection"];
    
    public string Trips => localizer["Trips"];
}