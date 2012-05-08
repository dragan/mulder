using System;
using System.IO;
using System.Reflection;

using Mulder.Base.Domain;
using Mulder.Base.IO;

namespace Mulder.Base.Commands
{
	public class CreateSiteCommand : ICommand
	{
		static readonly string usage = "create site [path]";
		readonly TextWriter writer;
		readonly IFileSystem fileSystem;
		readonly ISite site;
		
		public string Usage { get { return usage; } }
		
		public CreateSiteCommand(TextWriter writer, IFileSystem fileSystem, ISite site)
		{
			this.writer = writer;
			this.fileSystem = fileSystem;
			this.site = site;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length != 1 || string.IsNullOrEmpty(arguments[0])) {
				writer.WriteLine("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			string path = arguments[0];
			if (fileSystem.DirectoryExists(path)) {
				writer.WriteLine("A site at '{0}' already exists.", path);
				return ExitCode.Error;
			}
			
			fileSystem.CreateDirectory(path);
			
			fileSystem.ChangeDirectory(path, () => {
				CreateMinimalSite();
				PopulateSite();
			});
			
			writer.WriteLine("Created a blank mulder site at '{0}'. Enjoy!", path);
			
			return ExitCode.Success;
		}
		
		void CreateMinimalSite()
		{
			CreateFileFromResourceName("DEFAULT_CONFIG", "config.yaml");
			writer.WriteLine("\tcreate config.yaml");
			
			CreateFileFromResourceName("DEFAULT_RULES", "Rules");
			writer.WriteLine("\tcreate Rules");
		}
		
		void PopulateSite()
		{
			using (var resourceStream = GetResourceStreamFromName("DEFAULT_LAYOUT")) {
				site.CreateLayout("/default/", resourceStream);
			}
			
			using (var resourceStream = GetResourceStreamFromName("DEFAULT_HOME_PAGE")) {
				site.CreateItem("/", resourceStream, new { Title = "Home" });
			}
			
			using (var resourceStream = GetResourceStreamFromName("DEFAULT_STYLE_SHEET")) {
				site.CreateItem("/stylesheet/", resourceStream, ".css");
			}
			
			fileSystem.CreateDirectory("lib");
		}
		
		Stream GetResourceStreamFromName(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}
		
		void CreateFileFromResourceName(string resourceName, string path)
		{
			using (var resourceStream = GetResourceStreamFromName(resourceName)) {
				fileSystem.WriteStreamToFile(path, resourceStream);
			}
		}
	}
}
