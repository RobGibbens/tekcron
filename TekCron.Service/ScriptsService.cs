using System;

namespace TekCron.Service
{
	public class ScriptsService
	{
		public bool Start()
		{
			Console.WriteLine("Sample Service Started...");
			return true;
		}

		public bool Stop()
		{
			return true;
		}
	}

	public class TestScriptsService
	{
		public bool Start()
		{
			Console.WriteLine("Sample Service Started...");
			return true;
		}

		public bool Stop()
		{
			return true;
		}
	}
}