using HarmonyLib;

namespace UICeo.QuickMainMenuLoading;

[HarmonyPatch(typeof(MainMenuWorldController))]
static class QuickMainMenuLoading
{

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuWorldController.InvokeMainMenuLoadingAnimations))]
    public static bool TestPrefix(MainMenuWorldController __instance)
    {
        if (Config.DefaultConfig.SkipLogosOnStartUp.Value == false)
        {
            return true;
        }

        __instance.StartCoroutine(__instance.LoadMainMenuFromGameWorld());

        Singleton<AudioController>.Instance.InitializeAudio(launchInMainMenu: true);

        // We can skip it because we are doing this ourself now
        return false;
    }
}
