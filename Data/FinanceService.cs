using System.Net;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace FinanceControl.Data
{
    public class FinanceService
    {
        private List<Finance> Finances = new();
        private readonly Authentication user;
        public HttpClient client = new();

        private readonly AuthenticationService _authenticationService;

        public FinanceService(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<List<Finance>> GetFinances()
        {
            var response =
                await client.GetAsync(
                    @$"https://localhost:7289/api/Finance/ListFinances?idUser={_authenticationService._auth.id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Finance>>(jsonResponse);
            }

            return null;
        }


        public async Task AddFinance(Finance finance)
        {
            finance.userId = _authenticationService._auth.id;
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7289/api/Finance");
            request.Content = new StringContent(JsonSerializer.Serialize(finance), null, "application/json");

            await client.SendAsync(request);
        }

        public async Task<int> RetornaId()
        {
            var response = await client.GetAsync(
                @$"https://localhost:7289/api/Finance/FinanceID");
            return Convert.ToInt32(response.Content.ReadAsStringAsync().Result) + 1;
        }

        public async Task<List<Finance>> FilterByMonthAndYear(int month, int year)
        {
            try
            {
                var response =
                    await client.GetAsync(
                        @$"https://localhost:7289/api/Finance/ListFinancesByMonth?month={month}&year={year}&id={_authenticationService._auth.id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var financeResponse = JsonSerializer.Deserialize<List<Finance>>(jsonResponse);

                    return financeResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Finance> GetFinanceById(int id)
        {
            var response =
                await client.GetAsync(
                    @$"https://localhost:7289/api/Finance/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                var financeResponse = JsonSerializer.Deserialize<Finance>(jsonResponse);

                return financeResponse;
            }

            return null;
        }

        public async Task UpdateFinance(Finance updatedFinance)
        {
            try
            {
                updatedFinance.userId = _authenticationService._auth.id;
                var request = new HttpRequestMessage(HttpMethod.Patch, $"https://localhost:7289/api/Finance/{updatedFinance.id}");
                request.Content = new StringContent(JsonSerializer.Serialize(updatedFinance), null, "application/json");

                await client.SendAsync(request);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteFinance(int deletedFinance)
        {
            var response =
                await client.DeleteAsync(
                    @$"https://localhost:7289/api/Finance/{deletedFinance}");

            var a = response.IsSuccessStatusCode;
        }
    }
}