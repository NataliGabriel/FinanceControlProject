namespace FinanceControl.Data
{
    public class FinanceService
    {
        private List<Finance> Finances = new List<Finance>(); // Simulação de banco de dados

        public Task<List<Finance>> GetFinances()
        {
            // Retorna todas as despesas existentes (simulação)
            return Task.FromResult(Finances);
        }

        public void AddFinance(Finance Finance)
        {
            // Simulação de ID único para a despesa
            Finance.Id = Finances.Count + 1;

            // Adiciona a despesa à lista (simulação de banco de dados)
            Finances.Add(Finance);
        }

        public Task<List<Finance>> FilterByMonthAndYear(int month, int year)
        {
            // Filtrar as despesas com base no mês e ano fornecidos
            var filteredFinances = Finances.Where(Finance =>
                Finance.Date.Month == month && Finance.Date.Year == year).ToList();

            return Task.FromResult(filteredFinances);
        }
        public async Task<Finance> GetFinanceById(int id)
        {
            // Simula uma busca por ID na lista de despesas
            return await Task.FromResult(Finances.FirstOrDefault(e => e.Id == id));
        }
        public async Task UpdateFinance(Finance updatedFinance)
        {
            try
            {
                var FinanceToUpdate = Finances.FirstOrDefault(e => e.Id == updatedFinance.Id);

                if (FinanceToUpdate != null)
                {
                    FinanceToUpdate.Name = updatedFinance.Name;
                    FinanceToUpdate.Amount = updatedFinance.Amount;
                    FinanceToUpdate.Category = updatedFinance.Category;

                    Console.WriteLine($"Despesa atualizada: {updatedFinance.Id}");
                }
                else
                {
                    throw new KeyNotFoundException($"Despesa com ID {updatedFinance.Id} não encontrada.");
                }
            }
            catch (Exception ex) { }
        }
        public async Task DeleteFinance(Finance deletedFinance)
        {
           var expenseToRemove = Finances.FirstOrDefault(e => e.Id == deletedFinance.Id);
            if (expenseToRemove != null)
            {
                Finances.Remove(expenseToRemove);
            }
        }

    }
}
