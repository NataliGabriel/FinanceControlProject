using System.Text.Json;

namespace FinanceControl.Data
{
    public class AuthenticationService
    {
        public bool IsAuthenticated { get; set; } = false;
        HttpClient _client = new HttpClient();
        public Authentication? _auth { get; private set; }
        

        public async Task<bool> Login(string username, string password)
        {
            var response =
                await _client.GetAsync(
                    @$"https://localhost:7289/api/Users/login?email={username}&password={password}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                _auth = JsonSerializer.Deserialize<Authentication>(jsonResponse);
                IsAuthenticated = true;
            }

            return IsAuthenticated;
        }

        public async Task<int> BuscaIdUser(string email)
        {
            var response =
                await _client.GetAsync(
                    @$"https://localhost:7289/api/Users/Email?email={email}");

            if (response.IsSuccessStatusCode)
            {
                return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                
            }

            return 0;
        }
        public async Task<bool> Cadastrar(Authentication userRegister)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7289/api/Users");
            request.Content = new StringContent(JsonSerializer.Serialize(userRegister), null, "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                _auth = JsonSerializer.Deserialize<Authentication>(jsonResponse);
                return true;
            }

            return false;
        }
        public async Task<int> RetornaId()
        {
            var response = await _client.GetAsync(
                @$"https://localhost:7289/api/Users/UserID");
            return Convert.ToInt32(response.Content.ReadAsStringAsync().Result) + 1;
        }
    }
}