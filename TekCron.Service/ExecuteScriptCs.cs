using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ScriptCs.Contracts;


namespace TekCron.Service
{
	public class ExecuteScriptCs
	{
		// dependencies
		//private readonly ILog logger;
		private readonly IFileSystem fileSystem;
		private readonly IPackageAssemblyResolver packageAssemblyResolver;
		private readonly IPackageInstaller packageInstaller;
		private readonly IScriptPackResolver scriptPackResolver;
		private readonly IScriptExecutor scriptExecutor;

		public ExecuteScriptCs(IFileSystem fileSystem,
														IPackageAssemblyResolver packageAssemblyResolver,
														IPackageInstaller packageInstaller, IScriptPackResolver scriptPackResolver,
														IScriptExecutor scriptExecutor)
		{
			//this.logger = logger;
			this.fileSystem = fileSystem;
			this.packageAssemblyResolver = packageAssemblyResolver;
			this.packageInstaller = packageInstaller;
			this.scriptPackResolver = scriptPackResolver;
			this.scriptExecutor = scriptExecutor;
		}

		// run script from file
		public void Run(string scriptPath)
		{
			// preserve current directory
			var previousCurrentDirectory = Environment.CurrentDirectory;

			try
			{
				// set directory to where script is
				// required to find NuGet dependencies
				Environment.CurrentDirectory = Path.GetDirectoryName(scriptPath);

				// prepare NuGet dependencies, download them if required
				var nuGetReferences = PreparePackages(
																				scriptPath,
																				fileSystem, packageAssemblyResolver,
																				packageInstaller);

				// get script packs: not fully tested yet        
				var scriptPacks = scriptPackResolver.GetPacks();

				// execute script from file
				scriptExecutor.Initialize(nuGetReferences, scriptPacks);
				scriptExecutor.Execute(scriptPath);
			}
			finally
			{
				// restore current directory
				Environment.CurrentDirectory = previousCurrentDirectory;
			}
		}

		// prepare NuGet dependencies, download them if required
		private static IEnumerable<string> PreparePackages(
														string scriptPath,
														IFileSystem fileSystem, IPackageAssemblyResolver packageAssemblyResolver,
														IPackageInstaller packageInstaller)
		{
			var workingDirectory = Path.GetDirectoryName(scriptPath);
			var binDirectory = Path.Combine(workingDirectory, @"bin\debug"); //TODO : ScriptCs.Constants.BinFolder

			var packages = packageAssemblyResolver.GetPackages(workingDirectory);

			packageInstaller.InstallPackages(
													packages,
													allowPreRelease: true);

			// current implementeation of RoslynCTP required dependencies to be in 'bin' folder
			if (!fileSystem.DirectoryExists(binDirectory))
			{
				fileSystem.CreateDirectory(binDirectory);
			}

			// copy dependencies one by one from 'packages' to 'bin'
			foreach (var assemblyName
									in packageAssemblyResolver.GetAssemblyNames(workingDirectory))
			{
				var assemblyFileName = Path.GetFileName(assemblyName);
				var destFile = Path.Combine(binDirectory, assemblyFileName);

				var sourceFileLastWriteTime = fileSystem.GetLastWriteTime(assemblyName);
				var destFileLastWriteTime = fileSystem.GetLastWriteTime(destFile);

				if (sourceFileLastWriteTime == destFileLastWriteTime)
				{
					//outputCallback(string.Format("Skipped: '{0}' because it is already exists", assemblyName));
				}
				else
				{
					fileSystem.Copy(assemblyName, destFile, overwrite: true);

					//if (outputCallback != null)
					//{
					//	outputCallback(string.Format("Copy: '{0}' to '{1}'", assemblyName, destFile));
					//}
				}

				yield return destFile;
			}
		}
	}
}
