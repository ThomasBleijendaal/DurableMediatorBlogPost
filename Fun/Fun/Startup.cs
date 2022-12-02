using Fun;
using Fun.Services;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Fun;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<OrderGateway>();

        builder.Services.AddSingleton<PurchaseService>();
        builder.Services.AddSingleton<MediatorPurchaseService>();
        builder.Services.AddSingleton<SplitMediatorPurchaseService>();

        builder.Services.AddMediatR(typeof(Startup));
    }
}

