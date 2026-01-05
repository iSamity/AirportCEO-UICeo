using System;
using BepInEx.Configuration;
using UICeo.Config;
using UnityEngine;

namespace UICeo.CameraZoom;

internal static class CameraZoomService
{
    private static readonly Vector2 DefaultNormalMap = new(700f, 700f);
    private static readonly Vector2 DefaultLargeMap = new(1050f, 700f);
    private const float DefaultZoomMax = -350f;

    internal static void OnCameraZoomMinChanged(object sender, EventArgs e)
    {
        var min = DefaultConfig.CameraZoomMin.Value;
        var max = DefaultConfig.CameraZoomMax.Value;

        // Min should be greater than max (less negative, e.g., -6 > -350)
        if (min <= max)
        {
            // Set min to be slightly above max
            DefaultConfig.CameraZoomMin.Value = max + 1f;
            Plugin.Logger.LogWarning($"[CameraZoomService] Camera Zoom Min must be greater than Max. Adjusted to: {DefaultConfig.CameraZoomMin.Value}");
        }

        ApplyZoomToActiveCamera();
    }

    internal static void OnCameraZoomMaxChanged(object sender, EventArgs e)
    {
        var min = DefaultConfig.CameraZoomMin.Value;
        var max = DefaultConfig.CameraZoomMax.Value;

        // Max should be less than min (more negative, e.g., -350 < -6)
        if (max >= min)
        {
            // Set max to be slightly below min
            DefaultConfig.CameraZoomMax.Value = min - 1f;
            Plugin.Logger.LogWarning($"[CameraZoomService] Camera Zoom Max must be less than Min. Adjusted to: {DefaultConfig.CameraZoomMax.Value}");
        }

        ApplyZoomToActiveCamera();
    }

    private static void ApplyZoomToActiveCamera()
    {
        var cameraController = Singleton<CameraController>.Instance;
        if (cameraController == null)
        {
            return;
        }

        var genericMoveCamera = cameraController.GetComponent<GenericMoveCamera>();
        if (genericMoveCamera == null)
        {
            return;
        }

        genericMoveCamera.ZRangeMax = DefaultConfig.CameraZoomMin.Value;
        genericMoveCamera.ZRangeMin = DefaultConfig.CameraZoomMax.Value;

        // Adjust far clip plane to accommodate extreme zoom out
        var mainCamera = cameraController.mainCamera;
        if (mainCamera != null)
        {
            float requiredFarClip = Mathf.Abs(DefaultConfig.CameraZoomMax.Value) + 100f;
            if (mainCamera.farClipPlane < requiredFarClip)
            {
                mainCamera.farClipPlane = requiredFarClip;
                Plugin.Logger.LogInfo($"[CameraZoomService] Camera far clip plane adjusted to: {requiredFarClip}");
            }
        }

        Plugin.Logger.LogInfo($"[CameraZoomService] Live camera zoom updated - Min: {genericMoveCamera.ZRangeMax}, Max: {genericMoveCamera.ZRangeMin}");
    }

    private static void OnMapSizeChanged(object sender, EventArgs e)
    {
        var mapSize = DefaultConfig.MapSize.Value;

        if (mapSize.x == 0 || mapSize.y == 0)
        {
            return;
        }

        // Check if map is larger than normal default
        if (!IsLargerThan(mapSize, DefaultNormalMap))
        {
            return;
        }

        // Determine baseline based on whether it's larger than large map
        Vector2 baseline = IsLargerThan(mapSize, DefaultLargeMap) ? DefaultLargeMap : DefaultNormalMap;

        // Calculate sensible zoom value
        float maxDimension = Mathf.Max(mapSize.x, mapSize.y);
        float baselineMax = Mathf.Max(baseline.x, baseline.y);
        float sensibleZoom = (maxDimension / baselineMax) * DefaultZoomMax;

        ShowZoomWarningDialog(sensibleZoom);
    }

    private static bool IsLargerThan(Vector2 mapSize, Vector2 defaultSize)
    {
        return mapSize.x > defaultSize.x || mapSize.y > defaultSize.y;
    }

    private static void ShowZoomWarningDialog(float sensibleZoom)
    {
        if (DialogPanel.Instance == null)
        {
            Plugin.Logger.LogWarning("[CameraZoomService] DialogPanel not available, cannot show warning");
            return;
        }

        string message = $"Your map size is larger than the default.\n\n" +
                         $"We recommend updating your camera zoom setting to {sensibleZoom:F0} " +
                         $"for a better experience.\n\n" +
                         $"Would you like to apply this setting?";

        DialogPanel.Instance.ShowQuestionPanelCustomOptions(
            (result) => OnZoomDialogResult(result, sensibleZoom),
            message,
            "OK",
            "Cancel",
            forceHide: true,
            suppressDefaultAudioEmission: false
        );
    }

    private static void OnZoomDialogResult(bool result, float sensibleZoom)
    {
        if (!result)
        {
            Plugin.Logger.LogInfo("[CameraZoomService] User cancelled zoom adjustment");
            return;
        }

        DefaultConfig.CameraZoomMax.Value = sensibleZoom;
        Plugin.Logger.LogInfo($"[CameraZoomService] Camera zoom max set to: {sensibleZoom}");
    }
}

