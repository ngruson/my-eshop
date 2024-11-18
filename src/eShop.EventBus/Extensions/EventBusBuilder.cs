using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.EventBus.Extensions;
public class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
{
    public IServiceCollection Services => services;
}
