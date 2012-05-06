# Mulder
By Dale Ragan

Mulder is a simple static site generator inspired by [nanoc][nanoc] written in C#.  Mulder
"compiles" raw documents written in formats like Liquid and Markdown into a complete
static website, suitable for serving with your favorite web server.

## The Interface

Interacting with Mulder is done through a command line interface using it's available commands.

    $ mulder create site <path>

Creates a new Mulder powered site at the desired path with a bare-bones directory layout.  A good
convention is to treat the last directory in the path as your site name.

This is the only command you can execute outside the root directory of a Mulder powered site.  The
rest of the commands require you to be in the root directory of your site.

    $ mulder compile

Compiles a Mulder powered site into a complete static website.  The output is sent to the
configured output directory.

    $ mulder help

Shows the help for using Mulder's command line interface by printing out available commands.

    $ mulder <command> help

Shows the help for a specific command by printing out the name, usage, description, and options.

## Contributing

Mulder uses a modified version of the popular Git branching model by [Vincent Driessen][nvie].
Please follow it when developing on Mulder.  In short all development is done off the `unstable`
branch, leaving the `master` branch containing only stable, production ready code.  When
submitting pull requests, use the `unstable` branch.  Please use [etiquette][etiquette] when
contributing code and [don't "push" your pull requests][dont-push].

To ease supporting developers on different platforms, Mulder uses [Sake][sake] for it's automated
build system.  There's nothing to install to get up and developing on Mulder, except for Mono
or .NET of course, everything else is bootstrapped within the automated build.

After a fresh clone, navigate into the Mulder project directory and then issue the `sake` command
to compile and run the tests if you're on a Mac or Linux machine.

    $ ./sake.sh

If you're on Windows, it's the same command without the `./`.

    $ sake.cmd

## Roadmap

Mulder is currently in pre-Alpha state and doesn't have a public release yet.  The source code is
under active development, therefore be aware that important areas such as the commands and public
API may see backwards incompatible changes between released versions.  Mulder uses
[semantic versioning][semver] for each release.

## License
Mulder is released under the [MIT License][mit-license]. See LICENSE for more information.

[nanoc]: http://nanoc.stoneship.org/
[nvie]: http://nvie.com/posts/a-successful-git-branching-model/
[etiquette]: http://tirania.org/blog/archive/2010/Dec-31.html
[dont-push]: http://www.igvita.com/2011/12/19/dont-push-your-pull-requests/
[sake]: https://github.com/sakeproject/sake/
[semver]: http://semver.org/
[mit-license]: http://www.opensource.org/licenses/mit-license.php