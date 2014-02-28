using System;
using Quartz;
using ScriptCs;
using ScriptCs.Contracts;

namespace TekCron.Service
{
	public class ScriptsJob : IJob
	{
		private readonly ExecuteScriptCs _executeScriptCs;

		public ScriptsJob(ExecuteScriptCs executeScriptCs)
		{
			_executeScriptCs = executeScriptCs;
		}

		public void Execute(IJobExecutionContext context)
		{
			//Console.WriteLine("ScriptsJob : The current time is: {0}", DateTime.Now);
			var scriptPath = @"C:\myscript.csx";
			try
			{
				_executeScriptCs.Run(scriptPath);
			}
			catch (Exception ex)
			{
				//logger.Error(ex);
				throw;
			}
		}
	}
}