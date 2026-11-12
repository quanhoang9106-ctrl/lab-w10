using System;

namespace SmartHomeMini;

public static class PatternTests
{
    private static void Check(string testName, bool result)
    {
        Console.WriteLine(
            testName + ": " + (result ? "PASS" : "FAIL"));

        Console.WriteLine();
    }

    public static void Run()
    {
        IDevice light = DeviceFactory.Create(
            "LIGHT", "Living room light");

        IDevice fan = DeviceFactory.Create(
            " fan ", "Living room fan");

        IDevice door = DeviceFactory.Create(
            "door", "Main door");

        // T1: Kiem tra Factory
        Check(
            "T1 - Factory",
            light is SmartLight &&
            fan is SmartFan &&
            door is SmartDoor);

        // T2: Tao thiet bi khong hop le
        bool gotError = false;

        try
        {
            DeviceFactory.Create("camera", "Camera");
        }
        catch (ArgumentException)
        {
            gotError = true;
        }

        Check("T2 - Invalid type", gotError);

        // T3: Kiem tra Singleton
        var c1 = HomeConfiguration.Instance;
        var c2 = HomeConfiguration.Instance;

        Check(
            "T3 - Singleton",
            Object.ReferenceEquals(c1, c2));

        // Tao Facade
        SmartHomeFacade facade =
            new SmartHomeFacade(
                light, fan, (ILockable)door);

        // T4: Che do di lam
        light.TurnOn();
        fan.TurnOn();
        ((ILockable)door).Unlock();

        facade.GoToWork();

        bool ok4 =
            !((SmartLight)light).IsOn &&
            !((SmartFan)fan).IsOn &&
            ((SmartDoor)door).IsLocked;

        Check("T4 - GoToWork", ok4);

        // T5: Che do di ngu
        facade.GoToSleep();

        bool ok5 =
            ((SmartLight)light).IsOn &&
            !((SmartFan)fan).IsOn &&
            ((SmartDoor)door).IsLocked;

        Check(
            "T5 - GoToSleep (check level on screen)",
            ok5);
    }
}