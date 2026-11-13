using System;
using System.Globalization;
using System.IO;

namespace SmartHomeMini;

public static class PatternTests
{
    private static int failures;
    private static int total;

    private static void Check(string name, bool result)
    {
        total++;
        if (!result) failures++;
        Console.WriteLine($"{name}: {(result ? "PASS" : "FAIL")}");
    }

    private static bool Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return true; }
        catch (Exception e)
        {
            Console.WriteLine($"Expected {typeof(T).Name}, got {e.GetType().Name}");
        }
        return false;
    }

    private static string Capture(Action action)
    {
        var original = Console.Out;
        using var output = new StringWriter();
        try
        {
            Console.SetOut(output);
            action();
            return output.ToString();
        }
        finally { Console.SetOut(original); }
    }

    public static void Run()
    {
        failures = total = 0;
        var config = HomeConfiguration.Instance;
        int originalLevel = config.NightLightLevel;
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            IDevice light = DeviceFactory.Create("LIGHT", "Living room light");
            IDevice fan = DeviceFactory.Create(" fan ", "Living room fan");
            IDevice door = DeviceFactory.Create("door", "Main door");
            Check("T1 - Factory", light is SmartLight && fan is SmartFan && door is SmartDoor);
            Check("T2 - Invalid type", Throws<ArgumentException>(() => DeviceFactory.Create("camera", "Camera")));
            Check("T3 - Singleton", ReferenceEquals(config, HomeConfiguration.Instance));

            foreach (string name in new string[] { null, "", "   " })
            {
                Check("Light - Invalid constructor name", Throws<ArgumentException>(() => new SmartLight(name)));
                Check("Fan - Invalid constructor name", Throws<ArgumentException>(() => new SmartFan(name)));
                Check("Door - Invalid constructor name", Throws<ArgumentException>(() => new SmartDoor(name)));
            }
            Check("Devices - Trim names", new SmartLight(" Light ").Name == "Light"
                && new SmartFan(" Fan ").Name == "Fan" && new SmartDoor(" Door ").Name == "Door");
            Check("Devices - Initially off and unlocked", !((SmartLight)light).IsOn
                && !((SmartFan)fan).IsOn && !((SmartDoor)door).IsOn && !((SmartDoor)door).IsLocked);

            var facade = new SmartHomeFacade(light, fan, (ILockable)door);
            light.TurnOn();
            fan.TurnOn();
            ((ILockable)door).Unlock();
            string workOutput = Capture(facade.GoToWork);
            Check("T4 - GoToWork", !((SmartLight)light).IsOn && !((SmartFan)fan).IsOn && ((SmartDoor)door).IsLocked);
            Check("T4 - Operation order", workOutput == string.Join(Environment.NewLine,
                "=== GO TO WORK ===", "Living room light: OFF", "Living room fan: OFF", "Main door: LOCKED", ""));

            config.NightLightLevel = 65;
            light.TurnOff();
            fan.TurnOn();
            ((ILockable)door).Unlock();
            string sleepOutput = Capture(facade.GoToSleep);
            Check("T5 - GoToSleep", ((SmartLight)light).IsOn && !((SmartFan)fan).IsOn && ((SmartDoor)door).IsLocked);
            Check("T5 - Configured level and order", sleepOutput == string.Join(Environment.NewLine,
                "=== GO TO SLEEP ===", "Living room fan: OFF", "Main door: LOCKED", "Living room light: ON", "Night light level: 65%", ""));

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
            Check("Factory - Turkish culture", DeviceFactory.Create("LIGHT", "Light") is SmartLight);
            foreach (string type in new string[] { null, "", "   " })
                Check("Factory - Empty type", Throws<ArgumentException>(() => DeviceFactory.Create(type, "Device")));
            foreach (string name in new string[] { null, "", "   " })
                Check("Factory - Empty name", Throws<ArgumentException>(() => DeviceFactory.Create("light", name)));

            foreach (int level in new[] { 0, 100 })
            {
                config.NightLightLevel = level;
                Check($"Configuration - Boundary {level}", config.NightLightLevel == level);
            }
            foreach (int level in new[] { -1, 101 })
            {
                Check($"Configuration - Reject {level}", Throws<ArgumentOutOfRangeException>(() => config.NightLightLevel = level));
                Check("Configuration - Keep last valid value", config.NightLightLevel == 100);
            }
            Check("Facade - Null light", Throws<ArgumentNullException>(() => new SmartHomeFacade(null, fan, (ILockable)door)));
            Check("Facade - Null fan", Throws<ArgumentNullException>(() => new SmartHomeFacade(light, null, (ILockable)door)));
            Check("Facade - Null door", Throws<ArgumentNullException>(() => new SmartHomeFacade(light, fan, null)));

            door.TurnOn();
            door.TurnOff();
            Check("Door - Independent power and lock", !((SmartDoor)door).IsOn && ((SmartDoor)door).IsLocked);
            ((ILockable)door).Unlock();
            Check("Door - Unlock", !((SmartDoor)door).IsLocked && !((SmartDoor)door).IsOn);
            light.TurnOn();
            light.TurnOn();
            fan.TurnOn();
            fan.TurnOn();
            Check("Devices - Repeated ON", ((SmartLight)light).IsOn && ((SmartFan)fan).IsOn);
            light.TurnOff();
            light.TurnOff();
            fan.TurnOff();
            fan.TurnOff();
            Check("Devices - Repeated OFF", !((SmartLight)light).IsOn && !((SmartFan)fan).IsOn);
        }
        finally
        {
            config.NightLightLevel = originalLevel;
            CultureInfo.CurrentCulture = originalCulture;
        }
        Console.WriteLine($"Tests: {total}, passed: {total - failures}, failed: {failures}");
        Environment.ExitCode = failures == 0 ? 0 : 1;
    }
}
