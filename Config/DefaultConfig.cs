using BepInEx.Configuration;
using UICeo.QuickLoading;
using UICeo.Sorting.Models;

namespace UICeo.Config;

static class DefaultConfig
{
    public static ConfigEntry<bool> SkipLogosOnStartUp;
    public static ConfigEntry<bool> AutoContinueLastGame;

    internal static ConfigEntry<bool> ShowHireConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowFireConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowTrainConfirmation { get; private set; }
    internal static ConfigEntry<bool> ShowRejectConfirmation { get; private set; }

    internal static ConfigEntry<bool> SortByEmployeeType { get; private set; }
    internal static ConfigEntry<SortByEnum> SortOptions { get; private set; }
    internal static ConfigEntry<SortDirectionEnum> SortDirection { get; private set; }


    internal static ConfigEntry<bool> SyncStaffFilters { get; private set; }

    internal static ConfigEntry<bool> SkipAnimationsInMainMenu { get; private set; }

    public static void Setup()
    {
        SkipLogosOnStartUp = ConfigReference.Bind("General", "Skip logos on startup", true, "Changing the value will take affect next time you startup the game");
        AutoContinueLastGame = ConfigReference.Bind("General", "Auto continue last game", false, "Automatically continue the last saved game on startup");
        AutoContinueLastGame.SettingChanged += AutoContinueService.OnAutoContinueSettingChanged;

        ShowHireConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Hire Confirmation", false, "Show a confirmation dialog when hiring an employee");
        ShowFireConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Fire Confirmation", true, "Show a confirmation dialog when firing an employee");
        ShowTrainConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Train Confirmation", false, "Show a confirmation dialog when training an employee");
        ShowRejectConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Reject Confirmation", true, "Show a confirmation dialog when rejecting an employee");

        SortByEmployeeType = ConfigReference.Bind("Sorting - Staff", "Sort By Employee Type", true, "Sort staff and applicants by their employee type");
        SortOptions = ConfigReference.Bind("Sorting - Staff", "Sort Options", SortByEnum.Skill, "Sort staff and applicants by skill when hiring");
        SortDirection = ConfigReference.Bind("Sorting - Staff", "Sort Direction", SortDirectionEnum.Descending, "Ascending means low to hight, Descending means high to low");

        SyncStaffFilters = ConfigReference.Bind("General Filters", "Sync Staff Filters", true, "Sync the staff filters between the staff and applicants screens");

        SkipAnimationsInMainMenu = ConfigReference.Bind("Main Menu", "Skip Animations", false, "Skip the animations in the main menu like the social buttons and the main menu panel");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
