using MudBlazor;

namespace Cashify.Application.DTOs.Theme;

public class LightTheme : MudTheme
{
    public LightTheme()
    {
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
        };
        Shadows = new Shadow();
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "5px"
        };
        ZIndex = new ZIndex
        {
            Drawer = 1300
        };
    }
}