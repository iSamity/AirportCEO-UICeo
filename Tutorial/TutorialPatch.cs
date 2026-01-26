using HarmonyLib;
using UICeo.Config;

namespace UICeo.Tutorial;

/// <summary>
/// Patches SetIntroductionCompleted to force the introduction as "completed"
/// when the DisableTutorial config is enabled.
/// </summary>
[HarmonyPatch(typeof(FrontTutorialUI), nameof(FrontTutorialUI.SetIntroductionCompleted))]
internal class DisableTutorialSetCompletedPatch
{
    [HarmonyPrefix]
    static void Prefix(ref bool status)
    {
        if (DefaultConfig.DisableTutorial.Value)
        {
            status = true; // Force introduction to be "completed"
        }
    }
}

/// <summary>
/// Patches ShowHideFrontTutorialPanel to prevent the tutorial panel from showing
/// when the DisableTutorial config is enabled.
/// </summary>
[HarmonyPatch(typeof(FrontTutorialUI), nameof(FrontTutorialUI.ShowHideFrontTutorialPanel))]
internal class DisableTutorialShowPanelPatch
{
    [HarmonyPrefix]
    static bool Prefix(bool status)
    {
        // If trying to show the panel and tutorial is disabled, skip the method
        if (status && DefaultConfig.DisableTutorial.Value)
        {
            return false; // Skip original method
        }
        return true; // Run original method
    }
}
