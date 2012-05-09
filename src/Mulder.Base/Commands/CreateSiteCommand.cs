using System;
using System.IO;
using System.Reflection;

using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CreateSiteCommand : ICommand
	{
		static readonly string usage = "create site [path]";
		readonly ILog log;
		readonly IFileSystem fileSystem;
		readonly IDataSource dataSource;
		
		public string Usage { get { return usage; } }
		
		public CreateSiteCommand(ILog log, IFileSystem fileSystem, IDataSource dataSource)
		{
			this.log = log;
			this.fileSystem = fileSystem;
			this.dataSource = dataSource;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length != 1 || string.IsNullOrEmpty(arguments[0])) {
				log.ErrorMessage("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			string path = arguments[0];
			if (fileSystem.DirectoryExists(path)) {
				log.ErrorMessage("A site at '{0}' already exists.", path);
				return ExitCode.Error;
			}
			
			fileSystem.CreateDirectory(path);
			
			fileSystem.ChangeDirectory(path, () => {
				CreateMinimalSite();
				PopulateSite();
			});
			
			log.InfoMessage("Created a blank mulder site at '{0}'. Enjoy!", path);
			
			return ExitCode.Success;
		}
		
		void CreateMinimalSite()
		{
			CreateFileFromResourceName("DEFAULT_CONFIG", "config.yaml");
			log.InfoMessage("\tcreate config.yaml");
			
			CreateFileFromResourceName("DEFAULT_RULES", "Rules");
			log.InfoMessage("\tcreate Rules");
		}
		
		void PopulateSite()
		{
			using (var resourceStream = GetResourceStreamFromResourceName("DEFAULT_LAYOUT")) {
				dataSource.CreateLayout("/default/", resourceStream);
			}
			
			using (var resourceStream = GetResourceStreamFromResourceName("DEFAULT_HOME_PAGE")) {
				dataSource.CreateItem("/", resourceStream, new { Title = "Home" });
			}
			
			using (var resourceStream = GetResourceStreamFromResourceName("DEFAULT_STYLE_SHEET")) {
				dataSource.CreateItem("/stylesheet/", resourceStream, ".css");
			}
			
			fileSystem.CreateDirectory("lib");
		}
		
		Stream GetResourceStreamFromResourceName(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}
		
		void CreateFileFromResourceName(string resourceName, string path)
		{
			using (var resourceStream = GetResourceStreamFromResourceName(resourceName)) {
				fileSystem.WriteStreamToFile(path, resourceStream);
			}
		}
	}
}
