using AirportCEOModLoader.Core;
using HarmonyLib;
using System.Collections;
using UICeo.Config;
using UnityEngine;

namespace UICeo.QuickLoading;

[HarmonyPatch(typeof(MainMenuWorldController))]
static class AutoContinuePatch
{
    private static bool hasAttemptedAutoContinue = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MainMenuWorldController.Start))]
    public static void Patch_Start(MainMenuWorldController __instance)
    {
        if (hasAttemptedAutoContinue)
        {
            return;
        }

        if (DefaultConfig.AutoContinueLastGame.Value)
        {
            hasAttemptedAutoContinue = true;
            __instance.StartCoroutine(WaitAndContinue());
        }
    }

    private static IEnumerator WaitAndContinue()
    {
        // Wait until the menu is fully loaded
        while (!Singleton<MainMenuWorldController>.Instance.mainMenuFullyLoaded)
        {
            yield return null;
        }

        string mostRecentSave = LoadHandler.GetMostRecentlySavedGame(MainMenuWorldLoadHandler.Instance.userSavedDataSearchPath);

        if (!string.IsNullOrEmpty(mostRecentSave))
        {
            Singleton<MainMenuWorldController>.Instance.LaunchAirport(Enums.GameLoadSetting.ContinueGame, mostRecentSave, isMod: false);
        }
        else
        {
            DialogUtils.QueueDialog("Auto continue is enabled but no save files were found. Create a save first.");
        }
    }
}

