using Newtonsoft.Json;

namespace FinanceControl.Data
{
    public class Finance
    {
        public decimal _amount;
        [JsonProperty("id")] public int id { get; set; }
        [JsonProperty("name")] public string name { get; set; }

        [JsonProperty("amount")]
        public decimal amount
        {
            get => _amount;
            set => _amount = Math.Round(value, 2);
        }

        [JsonProperty("category")] public string category { get; set; }
        [JsonProperty("date")] public DateTime date { get; set; }
        public int userId { get; set; }
    }
}