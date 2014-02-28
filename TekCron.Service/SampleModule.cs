using Common.Logging;
using Common.Logging.Simple;
using Ninject.Modules;
using NuGet;
using ScriptCs;
using ScriptCs.Contracts;
using ScriptCs.Engine.Roslyn;
using ScriptCs.Hosting.Package;
using IFileSystem = ScriptCs.Contracts.IFileSystem;

namespace TekCron.Service
{
	public class SampleModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IFileSystem>().To<FileSystem>().InSingletonScope();

			Bind<ILog>().To<ConsoleOutLogger>()
				.WithConstructorArgument("logName", @"Custom ScriptCs from C#")
				.WithConstructorArgument("logLevel", Common.Logging.LogLevel.All)
				.WithConstructorArgument("showLevel", true)
				.WithConstructorArgument("showDateTime", true)
				.WithConstructorArgument("showLogName", true)
				.WithConstructorArgument("dateTimeFormat", @"yyyy-mm-dd hh:mm:ss");

			Bind<IFilePreProcessor>().To<FilePreProcessor>().InSingletonScope();

			Bind<IScriptHostFactory>().To<ScriptHostFactory>().InSingletonScope();

			Bind<IScriptEngine>().To<RoslynScriptEngine>();

			Bind<IScriptExecutor>().To<ScriptExecutor>();

			Bind<IInstallationProvider>().To<NugetInstallationProvider>().InSingletonScope();

			Bind<IPackageAssemblyResolver>().To<PackageAssemblyResolver>().InSingletonScope();

			Bind<IPackageContainer>().To<PackageContainer>().InSingletonScope();

			Bind<IPackageInstaller>().To<PackageInstaller>().InSingletonScope();

			Bind<IPackageManager>().To<PackageManager>().InSingletonScope();

			Bind<IScriptPackResolver>().To<ScriptPackResolver>().InSingletonScope();

			Bind<ExecuteScriptCs>().ToSelf();
		}
	}
}