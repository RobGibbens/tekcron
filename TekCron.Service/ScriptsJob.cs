using System;
using Quartz;

namespace TekCron.Service
{
	public class ScriptsJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Console.WriteLine("ScriptsJob : The current time is: {0}", DateTime.Now);
		}
	}
}