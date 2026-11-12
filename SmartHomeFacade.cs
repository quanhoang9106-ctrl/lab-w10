using System;

namespace SmartHomeMini;

public class SmartHomeFacade
{
    private IDevice light;
    private IDevice fan;
    private ILockable door;

    public SmartHomeFacade(
        IDevice light,
        IDevice fan,
        ILockable door)
    {
        this.light = light;
        this.fan = fan;
        this.door = door;
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