using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Fun.Example5;

public class Function
{
    public interface IBankAccount
    {
        Task AddAsync(decimal amount);

        Task RemoveAsync(decimal amount);
    }

    [FunctionName("Example5")]
    public async Task OrchestrateAsync(
        [OrchestrationTrigger]IDurableOrchestrationContext context)
    {
        var transaction = context.GetInput<Data>();

        var account1 = new EntityId("BankAccount", transaction.From);
        var account2 = new EntityId("BankAccount", transaction.To);

        var account1Entity = context.CreateEntityProxy<IBankAccount>(account1);
        var account2Entity = context.CreateEntityProxy<IBankAccount>(account2);

        using (await context.LockAsync(account1, account2))
        {
            await account1Entity.RemoveAsync(transaction.Amount);
            await account2Entity.AddAsync(transaction.Amount);
        } 
    }

    public class BankAccount : IBankAccount
    {
        public decimal CurrentAmount { get; set; }

        public Task AddAsync(decimal amount)
        {
            CurrentAmount += amount;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(decimal amount)
        {
            CurrentAmount -= amount;
            return Task.CompletedTask;
        }

        [FunctionName("BankAccount")]
        public static Task DispatchAsync([EntityTrigger] IDurableEntityContext context)
            => context.DispatchAsync<BankAccount>();
    }

    public record Data(decimal Amount, string From, string To);
}   
