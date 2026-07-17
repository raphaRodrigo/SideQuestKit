using SideQuestKit.Api;
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

        var appIdOption =
            new Option<string>("--app-id")
            {
                Required = true
            };

        var apkOption =
            new Option<FileInfo>("--apk")
            {
                Required = true
            };

        var refreshTokenOption =
            new Option<string>("--refresh-token")
            {
                Required = true
            };

        command.Options.Add(appIdOption);
        command.Options.Add(apkOption);
        command.Options.Add(refreshTokenOption);

        command.SetAction(async parseResult =>
        {
            var refreshToken =
                parseResult.GetValue(
                    refreshTokenOption)!;

            Console.WriteLine(
                "[1/4] Refreshing access token...");

            var client =
                new SideQuestClient();

            var token =
                await client
                    .RefreshTokenAsync(
                        refreshToken);

            Console.WriteLine(
                $"Authenticated as {token.UsersId}");
        });

        return command;
    }
}