using SideQuestKit.Cli.Commands;
using System.CommandLine;

var root = new RootCommand("SideQuestKit");

root.Subcommands.Add(PublishCommand.Create());

return await root.Parse(args).InvokeAsync();