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

        var versionNameOption =
            new Option<string>("--version-name")
            {
                Required = true
            };

        var versionCodeOption =
            new Option<int?>("--version-code")
            {
                Required = true
            };

        command.Options.Add(appIdOption);
        command.Options.Add(apkOption);
        command.Options.Add(refreshTokenOption);
        command.Options.Add(versionNameOption);
        command.Options.Add(versionCodeOption);

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

            var versionName =
                parseResult.GetValue(
                    versionNameOption)!;

            var versionCode =
                parseResult.GetValue(
                    versionCodeOption)!;

            if (!apk.Exists)
            {
                Console.Error.WriteLine(
                    $"APK not found: {apk.FullName}");

                return;
            }

            var client =
                new SideQuestClient();

            Console.WriteLine(
                "[1/5] Refreshing access token...");

            var token =
                await client.RefreshTokenAsync(
                    refreshToken);

            Console.WriteLine(
                $"OK (User: {token.UsersId})");

            Console.WriteLine(
                "[2/5] Creating upload...");

            var upload =
                await client.CreateUploadAsync(
                    token.AccessToken,
                    apk);

            Console.WriteLine(
                $"OK (FileId: {upload.FileId})");

            Console.WriteLine(
                "[3/5] Uploading APK...");

            await client.UploadFileAsync(
                upload.UploadUri,
                upload.ContentType,
                apk);

            Console.WriteLine(
                "OK");

            if (versionName is not null ||
                versionCode is not null)
            {
                Console.WriteLine(
                    "[4/5] Updating app metadata...");

                var app =
                    await client.GetAppAsync(
                        token.AccessToken,
                        appId);

                if (versionName is not null)
                {
                    app.Versionname =
                        versionName;
                }

                if (versionCode.HasValue)
                {
                    app.Versioncode =
                        versionCode.Value;
                }

                await client.UpdateAppAsync(
                    token.AccessToken,
                    app);

                Console.WriteLine(
                    "OK");
            }

            Console.WriteLine(
                "[5/5] Associating APK with App...");

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