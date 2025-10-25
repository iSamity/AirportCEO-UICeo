using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UICeo.Config;
using UICeo.Sorting.Strategy;

namespace UICeo;

[BepInPlugin($"org.iSamity.{MyPluginInfo.PLUGIN_GUID}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static ConfigFile ConfigReference { get; private set; }

    private void Awake()
    {
        SortService.Initialize();

        // Plugin startup logic
        Logger = base.Logger;
        ConfigReference = base.Config;

        DefaultConfig.Setup();

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
