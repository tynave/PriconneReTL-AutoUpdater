using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PriconneReTLAutoUpdater;

[PatcherPluginInfo("PriconneReTLAutoUpdater", "PriconneReTLAutoUpdater by Farleena", "1.0.0")]
class EntrypointPatcher : BasePatcher
{
    private static string githubAPI = "https://api.github.com/repos/ImaterialC/PriconneRe-TL";
    private string assetLink;
    private string priconnePath;
    private string localVersion;
    private bool localVersionValid;
    private string latestVersion;
    private bool latestVersionValid;

    internal static new ManualLogSource Logger;
    public override void Initialize() 
    {
        EntrypointPatcher.Logger = base.Log;

        try
        {
            priconnePath = Paths.GameRootPath;
            (localVersion, localVersionValid) = GetLocalVersion();
            (latestVersion, latestVersionValid, assetLink) = GetLatestRelease();

            if (!latestVersionValid || !localVersionValid)
            {
                Logger.LogError("Could not determine local version or latest release! Cannot continue!");
                return;
            }

            int versioncompare = localVersion.CompareTo(latestVersion);

            if (versioncompare == 0)
            {
                Logger.LogWarning("You already have the latest version installed! Skipping update!");
                return;
            }

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PriconneReTLAutoUpdaterApp.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"{priconnePath} {localVersion} {latestVersion} {assetLink}"
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            Logger.LogInfo("New version found! Starting PriconneReTLAutoUpdaterApp..");

            process.Start();
            process.WaitForExit();

        }
        catch (Exception ex)
        {
            Logger.LogError("An error occurred: " + ex.Message);
        }


    }
    public override void Finalizer() { }

    public (string localVersion, bool localVersionValid) GetLocalVersion()
    {
        try
        {
            string versionFilePath = Path.Combine(priconnePath, "BepInEx", "Translation", "en", "Text", "Version.txt");

            if (!File.Exists(versionFilePath))
            {
                Logger.LogError("Version file not found!");
                return (localVersion: null, localVersionValid = false);
            }
            string rawVersionFile = File.ReadAllText(versionFilePath);
            localVersion = System.Text.RegularExpressions.Regex.Match(rawVersionFile, @"\d{8}[a-z]?").Value;

            if (localVersion == "")
            {
                Logger.LogError("Version string does not match regex pattern!");
                return (localVersion: null, localVersionValid: false);
            }

            Logger.LogInfo($"Local Version: {localVersion}");
            return (localVersion, localVersionValid: true);

        }
        catch (Exception ex)
        {
            Logger.LogError("Error getting local version: " + ex.Message);
            return (localVersion: null, localVersionValid: false);
        }
    }

    static (string latestVersion, bool latestVersionValid, string assetLink) GetLatestRelease()
    {
        try
        {
            string releaseUrl = githubAPI + "/releases/latest";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Add("User-Agent", "PriconneTLUpdater");

                HttpResponseMessage response = client.GetAsync(releaseUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    dynamic releaseJson = JsonConvert.DeserializeObject(responseBody);
                    string version = releaseJson.tag_name;
                    string assetLink = releaseJson.assets[0].browser_download_url;
                    Logger.LogInfo($"Latest Release Version: {version}");
                    Logger.LogInfo($"Link to compressed asset files: {assetLink}");
                    return (latestVersion: version, latestVersionValid: true, assetLink: assetLink);
                }
                else
                {
                    Logger.LogError($"Error getting latest release: {response.ReasonPhrase}");
                    return (latestVersion: null, latestVersionValid: false, assetLink: null);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Error getting latest release: " + ex.Message);
            return (latestVersion: null, latestVersionValid: false, assetLink: null);
        }
    }

}