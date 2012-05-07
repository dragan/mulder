using System;
using System.IO;

using Mulder.Base.IO;

namespace Mulder.Base.Commands
{
	public class CreateSiteCommand : ICommand
	{
		static readonly string usage = "create site [path]";
		readonly TextWriter writer;
		readonly IFileSystem fileSystem;
		
		public string Usage { get { return usage; } }
		
		public CreateSiteCommand(TextWriter writer, IFileSystem fileSystem)
		{
			this.writer = writer;
			this.fileSystem = fileSystem;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length != 1 || string.IsNullOrEmpty(arguments[0])) {
				this.writer.WriteLine("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			string path = arguments[0];
			if (fileSystem.DirectoryExists(path)) {
				this.writer.WriteLine("A site at '{0}' already exists.", path);
				return ExitCode.Error;
			}
			
			return ExitCode.Success;
		}
	}
}
