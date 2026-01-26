using HarmonyLib;
using UICeo.Config;
using UnityEngine;

namespace UICeo.CameraZoom;

/// <summary>
/// Patches GenericMoveCamera.Update to scale movement speed with squared distance ratio.
/// This ensures panning feels appropriately fast at extreme zoom levels.
/// </summary>
[HarmonyPatch(typeof(GenericMoveCamera), nameof(GenericMoveCamera.Update))]
internal class CameraSpeedPatch
{
    private const float DefaultZoomMax = 350f;

    [HarmonyPostfix]
    static void Postfix(GenericMoveCamera __instance)
    {
        // Only apply scaling if zoom range is extended beyond default
        float configuredMax = Mathf.Abs(DefaultConfig.CameraZoomMax.Value);
        if (configuredMax <= DefaultZoomMax)
        {
            return;
        }

        float cameraDistance = Mathf.Abs(__instance.transform.position.z);

        // Calculate the ratio as if we're using default zoom (for curve evaluation)
        float curveRatio = Mathf.Clamp01(cameraDistance / DefaultZoomMax);

        // Get the curve value using the normalized ratio
        float curveValue = Mathf.Lerp(
            __instance.MovementSpeed.Evaluate(0f),
            __instance.MovementSpeed.Evaluate(1f),
            curveRatio
        );

        // Base speed from curve
        float baseSpeed = curveValue * __instance.MovementSpeedMultiplier;

        // Scale with squared ratio - exponential compensation for extreme zoom
        // At -350: 1x, at -700: 4x, at -3500: 100x, at -20000: 3265x
        float distanceRatio = Mathf.Max(1f, cameraDistance / DefaultZoomMax);
        float distanceMultiplier = distanceRatio * distanceRatio * distanceRatio;

        __instance.MovementSpeedMagnification = baseSpeed * distanceMultiplier;
    }
}

