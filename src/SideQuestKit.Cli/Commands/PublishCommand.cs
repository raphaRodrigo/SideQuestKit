using System.CommandLine;

namespace SideQuestKit.Cli.Commands;

public static class PublishCommand
{
    public static Command Create()
    {
        var command =
            new Command(
                "publish",
                "Publishes an APK to SideQuest.");

        command.Options.Add(
            new Option<string>(
                "--app-id")
            {
                Required = true
            });

        command.Options.Add(
            new Option<FileInfo>(
                "--apk")
            {
                Required = true
            });

        command.Options.Add(
            new Option<string>(
                "--refresh-token")
            {
                Required = true
            });

        command.SetAction(
            result =>
            {
                Console.WriteLine(
                    "Publish command invoked.");
            });

        return command;
    }
}