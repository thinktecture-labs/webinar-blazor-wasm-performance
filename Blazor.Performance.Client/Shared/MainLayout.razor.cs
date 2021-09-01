using MudBlazor;

namespace Blazor.Performance.Client.Shared
{
    public partial class MainLayout
    {
        private bool _useDarkTheme = false;
        private string icon = "fas fa-moon";

        protected override void OnInitialized()
        {
            currentTheme = defaultTheme;
        }

        void DarkMode()
        {
            _useDarkTheme = !_useDarkTheme;
            icon = _useDarkTheme ? "fas fa-sun" : "fas fa-moon";
            currentTheme = _useDarkTheme ? darkTheme : defaultTheme;
        }

        MudTheme currentTheme = new MudTheme();
        MudTheme defaultTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#ff584f",
                Black = "#272c34"
            }
        };

        MudTheme darkTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#3d6fb4",
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#27272f",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(255,255,255, 0.12)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.3)",
                TextDisabled = "rgba(255,255,255, 0.2)"
            }
        };
    }
}
