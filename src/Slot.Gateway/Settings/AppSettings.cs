namespace Slot.Gateway.Settings
{
    public class AppSettings
    {
        public ExternalSettings Externals { get; set; }
    }

    public class ExternalSettings
    {
        public ExternalSettingItem SlotApi { get; set; }
    }

    public class ExternalSettingItem
    {
        public string Url { get; set; }
        public int Timeout { get; set; }
        public string Authorization { get; set; }
    }
}
