using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fun.Models;

namespace Fun.Services;

public class OrderGateway
{
    public Task<Order?> GetOrderAsync(string id) => Task.FromResult(new Order(id, new List<Product> { new Product("1") }))!;
    public Task<Guid> CreateInvoiceAsync(string orderId) => Task.FromResult(Guid.NewGuid());
    public Task MarkProductAsSoldAsync(Product product) => Task.CompletedTask;
}
