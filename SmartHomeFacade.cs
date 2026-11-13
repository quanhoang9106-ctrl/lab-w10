using System;

namespace SmartHomeMini;

public class SmartHomeFacade
{
    private readonly IDevice light;
    private readonly IDevice fan;
    private readonly ILockable door;

    public SmartHomeFacade(
        IDevice light,
        IDevice fan,
        ILockable door)
    {
        this.light = light ?? throw new ArgumentNullException(nameof(light));
        this.fan = fan ?? throw new ArgumentNullException(nameof(fan));
        this.door = door ?? throw new ArgumentNullException(nameof(door));
    }

    public void GoToWork()
    {
        Console.WriteLine("=== GO TO WORK ===");

        light.TurnOff();
        fan.TurnOff();
        door.Lock();
    }

    public void GoToSleep()
    {
        Console.WriteLine("=== GO TO SLEEP ===");

        fan.TurnOff();
        door.Lock();
        light.TurnOn();

        int level =
            HomeConfiguration.Instance.NightLightLevel;

        Console.WriteLine(
            "Night light level: " + level + "%");
    }
}
