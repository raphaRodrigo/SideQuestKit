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
            var appId =
                parseResult.GetValue(
                    appIdOption)!;

            var apk =
                parseResult.GetValue(
                    apkOption)!;

            var refreshToken =
                parseResult.GetValue(
                    refreshTokenOption)!;

            if (!apk.Exists)
            {
                Console.Error.WriteLine(
                    $"APK not found: {apk.FullName}");

                return;
            }

            var client =
                new SideQuestClient();

            Console.WriteLine(
                "[1/4] Refreshing access token...");

            var token =
                await client.RefreshTokenAsync(
                    refreshToken);

            Console.WriteLine(
                $"OK (User: {token.UsersId})");

            Console.WriteLine(
                "[2/4] Creating upload...");

            var upload =
                await client.CreateUploadAsync(
                    token.AccessToken,
                    apk);

            Console.WriteLine(
                $"OK (FileId: {upload.FileId})");

            Console.WriteLine(
                "[3/4] Uploading APK...");

            await client.UploadFileAsync(
                upload.UploadUri,
                upload.ContentType,
                apk);

            Console.WriteLine(
                "OK");

            var app =
                await client.GetAppAsync(
                    token.AccessToken,
                    appId);

            Console.WriteLine(
                $"{app.Name}");

            Console.WriteLine(
                $"{app.Versionname}");

            Console.WriteLine(
                $"{app.Versioncode}");

            Console.WriteLine(
                "[4/4] Associating APK with App...");

            await client.AssociateApkAsync(
                token.AccessToken,
                appId,
                upload);

            Console.WriteLine(
                "OK");
        });

        return command;
    }
}