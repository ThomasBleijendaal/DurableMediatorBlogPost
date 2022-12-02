using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Fun.Example4;

public class Function
{
    [FunctionName("Example4")]
    public async Task OrchestrateAsync(
        [OrchestrationTrigger]IDurableOrchestrationContext context)
    {
        try
        {
            var data = context.GetInput<Data>();
            var productsProcessed = new List<string>();

            foreach (var product in data.Products)
            {
                await context.CallActivityAsync("ProductActivity", product);
                productsProcessed.Add(product);
            }

            await context.CallActivityAsync("ProductsActivity", productsProcessed);
        }
        catch (Exception ex)
        {
            await context.CallActivityAsync("FailureActivity", ex);
        }
        finally
        {
            await context.CallActivityAsync("FinalizingActivity", context.GetInput<string>());
        }
    }

    public record Data(string OrderId, Guid InvoiceId, IEnumerable<string> Products);
}   
