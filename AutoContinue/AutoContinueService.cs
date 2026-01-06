using System;
using BepInEx.Configuration;
using UnityEngine;
using AirportCEOModLoader;

namespace UICeo.QuickLoading;

static class AutoContinueService
{
    public static void OnAutoContinueSettingChanged(object sender, EventArgs e)
    {
        var args = e as SettingChangedEventArgs;
        if (args?.ChangedSetting.BoxedValue is not true)
        {
            return;
        }

        // Check if there's a save file available
        string savePath = MainMenuWorldLoadHandler.Instance?.userSavedDataSearchPath;
        if (string.IsNullOrEmpty(savePath))
        {
            AirportCEOModLoader.Core.DialogUtils.QueueDialog("Auto continue enabled but no save path is available yet. The setting will take effect next time you start the game.");
            return;
        }

        string mostRecentSave = LoadHandler.GetMostRecentlySavedGame(savePath);
        if (string.IsNullOrEmpty(mostRecentSave))
        {
            AirportCEOModLoader.Core.DialogUtils.QueueDialog("Auto continue enabled but no save files were found. Create a save first, then this setting will work on next startup.");
        }
    }
}

