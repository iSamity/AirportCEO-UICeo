using HarmonyLib;

namespace UICeo.QuickLoading;

[HarmonyPatch(typeof(MainMenuWorldController))]
static class SkipLogosOnStartup
{

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuWorldController.InvokeMainMenuLoadingAnimations))]
    public static bool Patch_InvokeMainMenuLoadingAnimations(MainMenuWorldController __instance)
    {
        if (Config.DefaultConfig.SkipLogosOnStartUp.Value == false)
        {
            return true;
        }


        __instance.apoapsisStudiosLogoTransform.AttemptInvokeLoadSprites();
        __instance.StartCoroutine(__instance.ShowHideGameMenuPanels(status: true, isLoadingFromDesktop: true));

        Singleton<AudioController>.Instance.InitializeAudio(launchInMainMenu: true);

        // We can skip it because we are doing this ourself now
        return false;
    }
}
