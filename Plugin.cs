using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PriconneReTLAutoUpdater;

[PatcherPluginInfo("PriconneReTlAutoUpdater", "PriconneReTLAutoUpdater by Farleena", "1.0.0")]
class EntrypointPatcher : BasePatcher
{

    public override void Initialize() 
    {
        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("PriconneReTLAutoUpdater");
        try
        {
            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PriconneReTLAutoUpdaterApp.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            process.WaitForExit();

        }
        catch (Exception ex)
        {
            myLogSource.LogError("An error occurred: " + ex.Message);
        }
    }
    public override void Finalizer() { }

}