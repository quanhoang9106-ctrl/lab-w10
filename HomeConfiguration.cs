using System;

namespace SmartHomeMini;

public sealed class HomeConfiguration
{
    private static readonly Lazy<HomeConfiguration> instance =
        new(() => new HomeConfiguration());

    public static HomeConfiguration Instance => instance.Value;

    public string HomeName { get; set; } = "My Home";

    private int nightLightLevel = 30;

    public int NightLightLevel
    {
        get => nightLightLevel;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Night light level must be from 0 to 100.");

            nightLightLevel = value;
        }
    }

    private HomeConfiguration()
    {
    }
}
