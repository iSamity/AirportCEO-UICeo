using BepInEx.Configuration;

namespace UICeo.Config;

static class DefaultConfig
{
    public static ConfigEntry<bool> SkipLogosOnStartUp;

    public static void Setup()
    {
        SkipLogosOnStartUp = ConfigReference.Bind("General", "Skip logos on startup", true, "Changing the value will take affect next time you startup the game");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
