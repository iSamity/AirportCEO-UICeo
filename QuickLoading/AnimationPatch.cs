using HarmonyLib;
using UnityEngine;

namespace UICeo.QuickLoading;

[HarmonyPatch(typeof(MainMenuWorldController))]
static class AnimationPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MainMenuWorldController.Awake))]
    public static void Patch_Awake(MainMenuWorldController __instance)
    {
        if (Config.DefaultConfig.SkipAnimationsInMainMenu.Value == false)
        {
            return;
        }

        // Disable animation on social buttons (Icons container)
        var iconsTransform = __instance.transform.Find("Icons");
        if (iconsTransform != null)
        {
            var iconHandlers = iconsTransform.GetComponentsInChildren<PanelAnimationHandler>(true);
            foreach (var handler in iconHandlers)
            {
                handler.disableAnimation = true;
            }
        }

        // Disable animation on main menu panel
        var mainMenuPanel = __instance.transform.Find("MainMenuPanel");
        if (mainMenuPanel != null)
        {
            var menuHandler = mainMenuPanel.GetComponent<PanelAnimationHandler>();
            if (menuHandler != null)
            {
                menuHandler.disableAnimation = true;
            }
        }

        // Disable animation on airport creation menu panel (new game screen)
        var airportCreationPanel = __instance.transform.Find("AirportCreationMenuPanel");
        if (airportCreationPanel != null)
        {
            var creationHandler = airportCreationPanel.GetComponent<PanelAnimationHandler>();
            if (creationHandler != null)
            {
                creationHandler.disableAnimation = true;
            }
        }
    }
}

