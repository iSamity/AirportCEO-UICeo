using HarmonyLib;
using UICeo.Config;
using UnityEngine;

namespace UICeo.CameraZoom;

[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Awake))]
internal class CameraZoomPatch
{
    [HarmonyPostfix]
    static void Postfix(GenericMoveCamera __instance)
    {
        __instance.ZRangeMax = DefaultConfig.CameraZoomMin.Value;
        __instance.ZRangeMin = DefaultConfig.CameraZoomMax.Value;

        // Adjust far clip plane for extreme zoom out
        var mainCamera = __instance.GetComponent<Camera>();
        if (mainCamera != null)
        {
            float requiredFarClip = Mathf.Abs(DefaultConfig.CameraZoomMax.Value) + 100f;
            if (mainCamera.farClipPlane < requiredFarClip)
            {
                mainCamera.farClipPlane = requiredFarClip;
                Plugin.Logger.LogInfo($"[CameraZoomPatch] Camera far clip plane adjusted to: {requiredFarClip}");
            }
        }

        Plugin.Logger.LogInfo($"[CameraZoomPatch] Camera zoom set to Min: {__instance.ZRangeMax}, Max: {__instance.ZRangeMin}");
    }
}

