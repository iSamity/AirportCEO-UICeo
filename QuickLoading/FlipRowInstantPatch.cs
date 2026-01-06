using System.Collections;
using HarmonyLib;

namespace UICeo.QuickLoading;

[HarmonyPatch(typeof(FlipRowHandler))]
static class FlipRowInstantPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(FlipRowHandler.HighlightRow))]
    public static bool Prefix(FlipRowHandler __instance, ref IEnumerator __result, Enums.ColorType color, string text)
    {
        if (!Config.DefaultConfig.SkipAnimationsInMainMenu.Value)
        {
            return true; // Run original animation
        }

        // Use instant methods instead of animated coroutine
        __instance.ForceRowColor(color);
        __instance.ForceRowText(text);
        
        // Return empty enumerator
        __result = EmptyEnumerator();
        return false;
    }

    private static IEnumerator EmptyEnumerator()
    {
        yield break;
    }
}

