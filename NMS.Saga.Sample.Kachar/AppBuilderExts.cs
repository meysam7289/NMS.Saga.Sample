using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace NMS.Saga.Sample.Kachar
{
    public static class AppBuilderExts
    {
		public static void UseMassTransit(this IApplicationBuilder app)
		{
			// start/stop the bus with the web application
			var appLifetime = (app ?? throw new ArgumentNullException(nameof(app)))
				.ApplicationServices.GetService<IApplicationLifetime>();

			var bus = app.ApplicationServices.GetService<IBusControl>();
			appLifetime.ApplicationStarted.Register(() => bus.Start());
			appLifetime.ApplicationStopped.Register(bus.Stop);
		}
	}
}
