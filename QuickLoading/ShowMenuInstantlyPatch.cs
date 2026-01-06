using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace UICeo.QuickLoading;

[HarmonyPatch(typeof(MainMenuWorldController))]
static class ShowMenuInstantlyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuWorldController.ShowHideGameMenuPanels))]
    public static bool Prefix(MainMenuWorldController __instance, ref IEnumerator __result, bool status, bool isLoadingFromDesktop)
    {
        if (!Config.DefaultConfig.SkipAnimationsInMainMenu.Value)
        {
            return true; // Run original when animations enabled
        }

        // Intercept both showing and hiding
        __result = status
            ? ShowInstantly(__instance, isLoadingFromDesktop)
            : HideInstantly(__instance);
        return false;
    }

    private static IEnumerator ShowInstantly(MainMenuWorldController instance, bool isLoadingFromDesktop)
    {
        // Fade background (find by transform path to avoid Image type reference)
        var background = instance.transform.Find("Background");
        if (background != null)
        {
            instance.StartCoroutine(instance.FadeComponentColor(background, Color.clear, 100));
        }

        // Show logo
        instance.airportCEOLogoTransform.EnableDisableAnimation(status: true, 50, 1, null);

        // Show main menu panel
        instance.mainMenuPanel.ShowHidePanel(status: true);

        yield return null; // Single frame

        instance.mainMenuLoaded = true;

        // Show ALL social buttons at once - no delay between them
        var icons = instance.transform.Find("Icons");
        if (icons != null)
        {
            foreach (var handler in icons.GetComponentsInChildren<PanelAnimationHandler>(true))
            {
                handler.EnableDisableAnimation(status: true, 25, -1, null);
            }
        }

        instance.updateMenuPanel.DisplayOnlyUpdateButtons(checkIfAlreadyOpen: true);
        instance.mainMenuFullyLoaded = true;
    }

    private static IEnumerator HideInstantly(MainMenuWorldController instance)
    {
        instance.StartCoroutine(MainMenuUI.Instance.SetSceneChangeButtonTexts());
        Singleton<AudioController>.Instance.FadeOutAllMainMenuAudioSources();

        instance.airportCreationMenuPanel.ShowHidePanel(status: false);
        instance.mainMenuPanel.ShowHidePanel(status: false, 50);
        instance.updateMenuPanel.ShowHidePanel(status: false);
        instance.updateMenuPanel.ShowHidePanel(status: false, attemptSetUpdateContenent: false, hidePerimanently: true);

        // Hide ALL social buttons at once - no delay between them
        var icons = instance.transform.Find("Icons");
        if (icons != null)
        {
            foreach (var handler in icons.GetComponentsInChildren<PanelAnimationHandler>(true))
            {
                handler.EnableDisableAnimation(status: false, 10, -1, null);
            }
        }

        var background = instance.transform.Find("Background");
        if (background != null)
        {
            // Enable the background Image component (avoid direct UI type reference)
            foreach (var comp in background.GetComponents<Behaviour>())
            {
                if (comp.GetType().Name == "Image")
                {
                    comp.enabled = true;
                    break;
                }
            }
        }

        instance.loadingAnimation.EnableDisableAnimation(status: true, 25, -1, null);
        instance.airportCEOLogoTransform.EnableDisableAnimation(status: true, 25, 2, null);

        if (background != null)
        {
            yield return instance.StartCoroutine(instance.FadeComponentColor(background, Color.black, 50));
        }

        while (instance.loadingAnimation.IsMoving)
        {
            yield return null;
        }
    }
}
