using HarmonyLib;
using UICeo.Config;

namespace UICeo.CameraZoom;

[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Awake))]
internal class CameraZoomPatch
{
    [HarmonyPostfix]
    static void Postfix(GenericMoveCamera __instance)
    {
        __instance.ZRangeMax = DefaultConfig.CameraZoomMin.Value;
        __instance.ZRangeMin = DefaultConfig.CameraZoomMax.Value;

        Plugin.Logger.LogInfo($"[CameraZoomPatch] Camera zoom set to Min: {__instance.ZRangeMax}, Max: {__instance.ZRangeMin}");
    }
}

