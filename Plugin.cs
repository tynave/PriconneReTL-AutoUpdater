using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace PriconneReTLAutoUpdater;

[PatcherPluginInfo("PriconneReTlAutoUpdater", "PriconneReTLAutoUpdater by Farleena", "1.0.0")]
class EntrypointPatcher : BasePatcher
{
    private string priconnePath;
    private bool priconnePathValid;

    public override void Initialize() 
    {
        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("PriconneReTLAutoUpdater");
        try
        {
            /*string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PriconneReTLAutoUpdaterApp.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            process.WaitForExit();*/

            (priconnePath, priconnePathValid) = GetGamePath();

        }
        catch (Exception ex)
        {
            myLogSource.LogError("An error occurred: " + ex.Message);
        }


    }
    public override void Finalizer() { }

    public (string priconnePath, bool priconnePathValid) GetGamePath()
    {
        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("PriconneReTLAutoUpdater");
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string versionString = assembly.GetName().Version.ToString();

            myLogSource.LogInfo($"PriconneReTLAutoUpdater version: {versionString}");

            string cfgFileContent = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dmmgameplayer5", "dmmgame.cnf"));
            dynamic cfgJson = JsonConvert.DeserializeObject(cfgFileContent);

            if (cfgJson != null && cfgJson.contents != null)
            {
                foreach (var content in cfgJson.contents)
                {
                    if (content.productId == "priconner")
                    {
                        priconnePath = content.detail.path;
                        // priconnePath = "C:\\Test"; // -- set fixed path for testing purposes
                        myLogSource.LogInfo("Found Princess Connect Re:Dive in " + priconnePath);
                        return (priconnePath, priconnePathValid = true);
                    }
                }
            }
            myLogSource.LogError("Cannot find the game path! Did you install Princess Connect Re:Dive from DMMGamePlayer?");
            
            return (priconnePath = "Not found", priconnePathValid = false);
        }
        catch (FileNotFoundException)
        {
            myLogSource.LogError("Cannot find the DMMGamePlayer config file! Do you have DMMGamePlayer installed?");
            
            return (priconnePath = "Not found", priconnePathValid = false);
        }
        catch (Exception ex)
        {
            myLogSource.LogError("Error getting game path: " + ex.Message);
            
            return (priconnePath = "ERROR!", priconnePathValid = false);
        }
    }

}