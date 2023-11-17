namespace FinanceControl.Data
{
    public class AuthenticationService
    {
        private readonly string validUsername = "user";
        private readonly string validPassword = "123";

        public bool IsAuthenticated { get; set; } = false;

        public async Task<bool> Login(string username, string password)
        {
            if (username == validUsername && password == validPassword)
            {
                IsAuthenticated = true;
            }
            else
            {
                IsAuthenticated = false;
            }

            return IsAuthenticated;
        }
    }
}
