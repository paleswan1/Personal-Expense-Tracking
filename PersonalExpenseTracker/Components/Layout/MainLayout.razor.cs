using MudBlazor;
using Shadow = MudBlazor.Shadow;
using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Components.Layout;

public partial class MainLayout
{
    public string PageTitle { get; set; } = "Personal Expense Tracker";
    
    private UserDetailsDto? UserDetails { get; set; }
    
    private bool DrawerOpen { get; set; } = true;
    
    private MudTheme Theme { get; } = new ()
    {
        ZIndex = new ZIndex
        {
            Drawer = 1300
        },
        PaletteLight = new PaletteLight()
        {
            Primary = "#F1973C",
            PrimaryLighten = "#FFE8C8",
            Secondary = "#005399",
            Success = "#00cc29",
            Error = "#ff0000",
            Tertiary = "#ff00001a",
            TertiaryContrastText = "#ff0000",
            TertiaryDarken = "#fff",
            Info = "#0bc5ea",
            Background = "#f8f8fa",
            AppbarBackground = "#fff",
            AppbarText = "#141414",
            DrawerBackground = "#fff",
            DrawerText = "rgba(0,0,0, 0.7)",
            TableLines = "#ebebeb",
            OverlayDark = "hsl(0deg 0% 0% / 75%)",
            Divider = "#ebebeb",
            TextPrimary = "#141414",
            TextSecondary = "#5c5c5c",
            GrayLight = "#858585",
            White = "#fff"
        },
        Shadows = new Shadow(),
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "5px"
        }
    };

    private static bool RightToLeft => false;

    protected override async Task OnInitializedAsync()
    {
        SeedData();
        
        await GetUserDetails();
    }

    private async Task GetUserDetails()
    {
        var userDetails = await UserService.GetUserDetails();

        if (userDetails != null)
        {
            UserDetails = userDetails;
        }
        else
        {
            var users = UserService.GetAllUsers();
        
            NavigationManager.NavigateTo(users.Count > 0 ? "/login" : "/register");
        }
    }

    private void SeedData()
    {
        SeedService.SeedDefaultTags();
    }
    
    private void LogoutHandler()
    {
        AuthenticationService.Logout();

        SnackbarService.ShowSnackbar("User successfully logged out.", Severity.Success, Variant.Outlined);
        
        NavigationManager.NavigateTo("/login", true);
    }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}