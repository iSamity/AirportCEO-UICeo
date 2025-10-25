using HarmonyLib;
using UICeo.Config;

namespace UICeo.Sorting;

[HarmonyPatch(typeof(EmployeePanelUI), nameof(EmployeePanelUI.InitializePanel))]
internal class RefreshPanelOnSettingChangedPatch
{
    [HarmonyPostfix]
    static void PostfixPatch(EmployeePanelUI __instance)
    {
        DefaultConfig.SortByEmployeeType.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
        DefaultConfig.SortOptions.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
        DefaultConfig.SortDirection.SettingChanged += (s, e) =>
        {
            RefreshPanelOnSettingChanged(__instance);
        };
    }

    private static void RefreshPanelOnSettingChanged(EmployeePanelUI instance)
    {
        if (!instance)
        {
            return;
        }

        instance.GenerateEmployeeContainers();
    }
}
