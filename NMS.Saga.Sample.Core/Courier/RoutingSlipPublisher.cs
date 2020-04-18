using MassTransit;
using MassTransit.Courier;
using NMS.Saga.Sample.Contracts;
using NMS.Saga.Sample.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace NMS.Saga.Sample.Core.Courier
{
    public class RoutingSlipPublisher
    {
        private readonly IBusControl _bus;

        public RoutingSlipPublisher(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task<Guid> PublishInsertCoding(Coding coding)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity("Core_Coding_Insert", new Uri($"{RabbitMqConstants.RabbitMqUri}Core_Coding_Insert"));
            builder.AddActivity("Kachar_Coding_Insert", new Uri($"{RabbitMqConstants.RabbitMqUri}Kachar_Coding_Insert"));
            builder.AddActivity("Rahavard_Coding_Insert", new Uri($"{RabbitMqConstants.RabbitMqUri}Rahavard_Coding_Insert"));
            builder.SetVariables(coding);
            var routingSlip = builder.Build();
            await _bus.Execute(routingSlip);
            return routingSlip.TrackingNumber;
        }
    }
}
