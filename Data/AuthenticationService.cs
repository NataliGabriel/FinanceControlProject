using System.Net;
using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
                    @$"https://finance-control-api.azurewebsites.net/api/Users/login?email={username}&password={password}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                _auth = JsonSerializer.Deserialize<Authentication>(jsonResponse);
                IsAuthenticated = true;
            }

            return IsAuthenticated;
        }

        public async Task<Authentication> BuscaSenhaUser(string email)
        {
            var response =
                await _client.GetAsync(
                    @$"https://finance-control-api.azurewebsites.net/api/Users/password?email={email}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<Authentication>(jsonResponse);
            }

            return null;
        }

        public async Task<bool> Cadastrar(Authentication userRegister)
        {
            if (ChecaEmail(userRegister.email).Result)
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://finance-control-api.azurewebsites.net/api/Users");
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

            return false;
        }

        public async Task<bool> ChecaEmail(string email)
        {
            var response =
                await _client.GetAsync(
                    @$"https://finance-control-api.azurewebsites.net/api/Users/Email?email={email}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task SendEmail(Authentication user, bool Cadastro)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "financecontrolweb@gmail.com";
                string senderPassword = "t@ti1311";

                if (IsValidEmail(user.email))
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Finance Control", senderEmail));
                    message.To.Add(new MailboxAddress(user.user, user.email));
                    message.Subject = "Finance Control";
                    if (Cadastro == true)
                    {
                        message.Body = new TextPart("plain")
                        {
                            Text =
                                @$"Olá {user.user}! Seja muito bem-vindo ao nosso site de gestão financeira pessoal, espero que goste do que oferecemos.
                        
                        Seu login é:
                        email: {user.email}
                        senha: {user.password}
                        
                        Essa é uma mensagem automática, não responda."
                        };
                    }
                    else
                    {
                        message.Body = new TextPart("plain")
                        {
                            Text =
                                @$"Olá {user.user}!
                        
                        Seu login é:
                        email: {user.email}
                        senha: {user.password}
                        
                        Essa é uma mensagem automática, não responda."
                        };
                    }

                    using var client = new SmtpClient();
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<int> RetornaId()
        {
            var response = await _client.GetAsync(
                @$"https://finance-control-api.azurewebsites.net/api/Users/UserID");
            return Convert.ToInt32(response.Content.ReadAsStringAsync().Result) + 1;
        }
    }
}