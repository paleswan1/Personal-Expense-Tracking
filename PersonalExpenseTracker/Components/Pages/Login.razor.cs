namespace PersonalExpenseTracker.Components.Pages
{
    public partial class Login
    {
        private string username = "";
        private string password = "";
        private string errorMessage = "";

        private async Task LoginHandler()
        {
            // Mock validation (replace with real authentication logic)
            if (username == "user" && password == "password")
            {
                await LocalStorage.SetItemAsStringAsync("authToken", "exampleAuthToken123");
                Navigation.NavigateTo("/home");
            }
            else
            {
                errorMessage = "Invalid username or password.";
            }
        }
    }
}