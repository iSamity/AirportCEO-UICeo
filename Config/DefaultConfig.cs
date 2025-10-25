using BepInEx.Configuration;

namespace UICeo.Config;

static class DefaultConfig
{
    public static ConfigEntry<bool> SkipLogosOnStartUp;

    public static void Setup()
    {
        SkipLogosOnStartUp = ConfigReference.Bind("General", "Skip logos on startup", true, "Changing the value will take affect next time you startup the game");

        ShowHireConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Hire Confirmation", false, "Show a confirmation dialog when hiring an employee");
        ShowFireConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Fire Confirmation", true, "Show a confirmation dialog when firing an employee");
        ShowTrainConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Train Confirmation", false, "Show a confirmation dialog when training an employee");
        ShowRejectConfirmation = ConfigReference.Bind("Confirmations - Staff", "Show Reject Confirmation", true, "Show a confirmation dialog when rejecting an employee");

    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
