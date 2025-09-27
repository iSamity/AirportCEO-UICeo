# Aiport CEO - ShortcutCEO Plugin

Plugin that manage keyboard shortcuts in Airport CEO.

Starts with default shortcuts and lets other plugins add their own shortcuts. While checking for conflicts with existing shortcuts.

## Use plugin in Airport CEO
How to start using the plugin:

### Manual installation
1. Download dll from [github releases](https://github.com/iSamity/AirportCEO-ShortcutCeo-V4/releases). 
2. Reference the dll in your plugin project 

### Steam
Go to [steam page](https://steamcommunity.com/sharedfiles/filedetails/?id=3560363115) and subscribe there. Don't forget to install the required [AirportCEO Mod Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=3109136766)

## Dev

### Local installation
1. Copy the repository
2. Open the project in Visual Studio and Build -> Rebuild
3. Copy the generated `ShortcutCeo.dll` to your Airport CEO plugins directory, typically located at `AirportCEO/BepInEx/plugins/`.

### Installation
How to use the plugin in your own plugin:

#### Manual installation
1. Download dll from releases
2. Reference the dll in your plugin project 

#### Nuget package
Is available on the [Nuget website](https://www.nuget.org/packages/ShortcutCeo/). 
Add it to your project using Nuget Manager from visual studio

don't forget to add `[BepInDependency("org.iSamity.plugins.ShortcutCeo")]`

### Usage
Add the following code to your plugin's `Start` method:
```csharp	
    using ShortcutCeo.config;

    [BepInPlugin()]
    [BepInDependency("AirportCEOShortcutCEO")]
    public class Plugin 
    {    
        var copyKey = ConfigReference.Bind("General", "Copy entity below cursor", new KeyboardShortcut(KeyCode.C, KeyCode.LeftControl), "Default in game is control + c");

        ConfigManager.AddShortcut(copyKey, () =>
        {
            Singleton<SelectionController>.Instance.CopyHoveredBuilding();
        });
    }
```
