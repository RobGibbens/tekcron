using System.Web.Http;
using Quartz;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Quartz.Ninject;
using Topshelf.WebApi;
using Topshelf.WebApi.Ninject;

namespace TekCron.Service
{
	class Program
	{
		static void Main()
		{
			HostFactory.Run(c =>
			{
				c.UseNinject(new SampleModule());

				c.Service<ScriptsService>(s =>
				{
					// Topshelf.Quartz (Optional) - Construct service using Ninject
					s.ConstructUsingNinject();

					s.WhenStarted((service, control) => service.Start());
					s.WhenStopped((service, control) => service.Stop());

					// Topshelf.Quartz.Ninject (Optional) - Construct IJob instance with Ninject
					s.UseQuartzNinject();

					// Schedule a job to run in the background every 5 seconds.
					// The full Quartz Builder framework is available here.
					s.ScheduleQuartzJob(q =>
							q.WithJob(() =>
									JobBuilder.Create<ScriptsJob>().Build())
									.AddTrigger(() =>
											TriggerBuilder.Create()
													.WithSimpleSchedule(builder => builder.WithIntervalInSeconds(2).RepeatForever()).Build())
							);

					s.ScheduleQuartzJob(q =>
							q.WithJob(() =>
									JobBuilder.Create<TestScriptsJob>().Build())
									.AddTrigger(() =>
											TriggerBuilder.Create()
													.WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5).RepeatForever()).Build())
							);

					s.WebApiEndpoint(api =>
						//Topshelf.WebApi - Uses localhost as the domain, defaults to port 8080.
						//You may also use .OnHost() and specify an alternate port.
												 api.OnLocalhost()
													 //Topshelf.WebApi - Pass a delegate to configure your routes
														 .ConfigureRoutes(Configure)
													 //Topshelf.WebApi.Ninject (Optional) - You may delegate controller 
													 //instantiation to Ninject.
													 //Alternatively you can set the WebAPI Dependency Resolver with
													 //.UseDependencyResolver()
														 .UseNinjectDependencyResolver()
													 //Instantaties and starts the WebAPI Thread.
														 .Build());
				});




			});
		}

		private static void Configure(HttpRouteCollection routes)
		{
			routes.MapHttpRoute(
							"DefaultApiWithId",
							"Api/{controller}/{id}",
							new { id = RouteParameter.Optional },
							new { id = @"\d+" });
		}
	}


}
