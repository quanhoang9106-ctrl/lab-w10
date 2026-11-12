namespace SmartHomeMini;

public sealed class HomeConfiguration
{
    private static HomeConfiguration instance =
        new HomeConfiguration();

    public static HomeConfiguration Instance
    {
        get { return instance; }
    }

    public string HomeName { get; set; } = "My Home";
    public int NightLightLevel { get; set; } = 30;

    private HomeConfiguration()
    {
    }
}