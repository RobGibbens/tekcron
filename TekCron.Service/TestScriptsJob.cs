using System;
using Quartz;

namespace TekCron.Service
{
	public class TestScriptsJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Console.WriteLine("TestScriptsJob : The current time is: {0}", DateTime.Now);
		}
	}
}