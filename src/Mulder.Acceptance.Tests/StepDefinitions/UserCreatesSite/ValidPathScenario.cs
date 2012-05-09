using System;
using System.Collections.Generic;
using System.IO;

using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class ValidPathScenario
	{
		string validPath;
		StringWriter writer;
		EntryPoint entryPoint;
		ExitCode exitCode;
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			writer = new StringWriter();
			
			var log = new Log(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			var createCommands = new Dictionary<string, ICommand>();
			var fileSystem = new FileSystem();
			createCommands.Add("site", new CreateSiteCommand(log, fileSystem, new FileSystemUnified(log, fileSystem)));
			
			var commands = new Dictionary<string, ICommand>();
			commands.Add("create", new CreateCommand(log, createCommands));
			
			entryPoint = new EntryPoint(log, commands);
		}
		
		[AfterScenario]
		public void CleanUp()
		{
			if (Directory.Exists(validPath))
				Directory.Delete(validPath, true);
		}
		
		[Given(@"I have a valid path")]
		public void Given_I_have_a_valid_path()
		{
			validPath = Path.Combine(Path.GetTempPath(), "default-mulder-site");
		}

		[When(@"I run the create site command with a valid path")]
		public void When_I_run_the_create_site_command_with_valid_path()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site", validPath });
		}
		
		[Then(@"I should see ""create config\.yaml"" message")]
		public void Then_I_should_see_create_config_yaml_message()
		{
			writer.ToString().ShouldContain("create config.yaml");
		}
		
		[Then(@"I should see ""create Rules"" message")]
		public void Then_I_should_see_create_rules_message()
		{
			writer.ToString().ShouldContain("create Rules");
		}
		
		[Then(@"I should see ""create layouts/default\.html"" message")]
		public void Then_I_should_see_create_layouts_default_html_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("layouts", "default.html")));
		}
		
		[Then(@"I should see ""create content/index\.html"" message")]
		public void Then_I_should_see_create_content_index_html_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("content", "index.html")));
		}
		
		[Then(@"I should see ""create content/stylesheet\.css"" message")]
		public void Then_I_should_see_create_content_stylesheet_css_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("content", "stylesheet.css")));
		}
		
		[Then(@"I should see ""Created a blank mulder site at '\[path]'\. Enjoy!"" message")]
		public void Then_I_should_see_created_a_blank_mulder_site_at_path_enjoy_message()
		{
			writer.ToString().ShouldContain(string.Format("Created a blank mulder site at '{0}'. Enjoy!", validPath));
		}
		
		[Then(@"I should have a directory created for my site")]
		public void Then_I_should_see_a_directory_created_for_my_site()
		{
			Directory.Exists(validPath).ShouldBe(true);
		}
		
		[Then(@"I should have my site directory populated with the default bare-bones site")]
		public void Then_I_should_have_my_site_directory_populated_with_the_default_bare_bones_site()
		{
			File.Exists(Path.Combine(validPath, "config.yaml")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "Rules")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "layouts")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "layouts", "default.html")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "content")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "content", "index.html")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "content", "stylesheet.css")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "lib")).ShouldBe(true);
		}
		
		[Then(@"the ""config\.yaml"" file should contain the default config")]
		public void Then_the_config_yaml_file_should_contain_the_default_config()
		{
			File.ReadAllText(Path.Combine(validPath, "config.yaml")).ShouldBe(DefaultSite.ConfigContent);
		}
		
		[Then(@"the ""Rules"" file should contain the default rules")]
		public void Then_the_rules_file_should_contain_the_default_rules()
		{
			File.ReadAllText(Path.Combine(validPath, "Rules")).ShouldBe(DefaultSite.RulesContent);
		}
		
		[Then(@"the ""layouts/default\.html"" file should contain the default layout")]
		public void Then_the_layouts_default_html_file_should_contain_the_default_layout()
		{
			File.ReadAllText(Path.Combine(validPath, "layouts", "default.html")).ShouldBe(DefaultSite.LayoutHtml);
		}
		
		[Then(@"the ""content/index\.html"" file should contain the default content")]
		public void Then_the_content_index__html_file_should_contain_the_default_content()
		{
			File.ReadAllText(Path.Combine(validPath, "content", "index.html")).ShouldBe(DefaultSite.HomePageHtml);
		}
		
		[Then(@"the ""content/stylesheet\.css"" file should contain the default styles")]
		public void Then_the_content_stylesheet_css_file_should_contain_the_default_styles()
		{
			File.ReadAllText(Path.Combine(validPath, "content", "stylesheet.css")).ShouldBe(DefaultSite.StylesheetCss);
		}
		
		[Then(@"mulder should terminate with an success exit code")]
		public void Then_mulder_should_terminate_with_an_success_exit_code()
		{
			exitCode.ShouldBe(ExitCode.Success);
		}
	}
	
	public static class DefaultSite
	{
		public static readonly string ConfigContent = @"# A configuration file allowing you to change Mulder's conventions

# The path to the directory where all generated files will be written to. This
# can be an absolute path starting with a slash, but it can also be path
# relative to the site directory.
output_directory: public

# A list of file extensions that Mulder will consider to be textual rather than
# binary. If an item with an extension not in this list is found,  the file
# will be considered as binary.
text_extensions: [ 'htm', 'html', 'css', 'liquid', 'js', 'less', 'markdown', 'md', 'txt', 'xhtml', 'xml' ]

# A list of index filenames, i.e. names of files that will be served by a web
# server when a directory is requested. Usually, index files are named
# “index.html”, but depending on the web server, this may be something else,
# such as “default.htm”. This list is used by Mulder to generate pretty URLs.
index_filenames: [ 'index.html' ]

# The data sources where Mulder loads its data from. This is an array of
# hashes; each array element represents a single data source. By default,
# there is only a single data source that reads data from the “content/” and
# “layout/” directories in the site directory.
data_sources:
  -
    # The type is the identifier of the data source. By default, this will be
    # `filesystem_unified`.
    type: filesystem_unified

    # The path where items should be mounted (comparable to mount points in
    # Unix-like systems). This is “/” by default, meaning that items will have
    # “/” prefixed to their identifiers. If the items root were “/en/”
    # instead, an item at content/about.html would have an identifier of
    # “/en/about/” instead of just “/about/”.
    items_root: /

    # The path where layouts should be mounted. The layouts root behaves the
    # same as the items root, but applies to layouts rather than items.
    layouts_root: /
";
		
		public static readonly string RulesContent = @"// Not sure how this is going to look yet, could be a C# or Shade file
";
		
		public static readonly string LayoutHtml = @"<!DOCTYPE HTML>
<html lang=""en"">
	<head>
		<meta charset=""utf-8"">
		<title>Mulder Generated Site - {{ item.title }}</title>
		<link rel=""stylesheet"" type=""text/css"" href=""/style.css"" media=""screen"">
	</head>
	<body>
		<div id=""main"">
			{{ content }}
		</div>
	</body>
</html>
";
		
		public static readonly string HomePageHtml = @"---
title: Home
---

<h1>The Truth Is Out There</h1>

<p>You’ve just created a new site using Mulder. The page you are looking at right now is the home page for your site. To get started, consider replacing this default homepage with your own customized homepage. Some pointers on how to do so:</p>

<ul>
	<li><strong>Change this page’s content</strong> by editing the “index.html” file in the “content” directory. This is the actual page content, and therefore doesn’t include the header or style information (those are part of the layout).</li>
	<li><strong>Change the layout</strong>, which is the “default.html” file in the “layouts” directory, and create something unique (and hopefully less bland).</li>
</ul>
";
		
		public static readonly string StylesheetCss = @"body {
  margin: 0;
  padding: 0;
  font-family: ""Palatino Linotype"",""Book Antiqua"",Palatino,serif;
  font-size: 100%;
}

a {
  color: #227CE8;
}

a:hover {
  color: #F70;
  text-decoration: none;
}

#main {
	width: 42em;
	margin: 0 auto;
}
";
	}
}
