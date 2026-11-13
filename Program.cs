using System;
using System.Collections.Generic;

namespace SmartHomeMini;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--test")
        {
            PatternTests.Run();
            return;
        }

        IDevice light = DeviceFactory.Create("light", "Living room light");
        IDevice fan = DeviceFactory.Create("fan", "Living room fan");
        IDevice door = DeviceFactory.Create("door", "Main door");
        var home = new SmartHomeFacade(light, fan, (ILockable)door);
        var devices = new Dictionary<string, IDevice>(StringComparer.OrdinalIgnoreCase)
        {
            ["light"] = light, ["fan"] = fan, ["door"] = door
        };

        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case null:
                case "0": return;
                case "1": home.GoToWork(); break;
                case "2": home.GoToSleep(); break;
                case "3": ControlDevice(devices); break;
                case "4": CreateDevice(devices); break;
                case "5": ChangeNightLightLevel(); break;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\n===== SMART HOME MINI =====");
        Console.WriteLine("1. Go to work");
        Console.WriteLine("2. Go to sleep");
        Console.WriteLine("3. Control a device");
        Console.WriteLine("4. Create a new device");
        Console.WriteLine("5. Change night light level");
        Console.WriteLine("0. Exit");
        Console.Write("Choose: ");
    }

    private static void ControlDevice(Dictionary<string, IDevice> devices)
    {
        Console.WriteLine("Devices: " + string.Join(", ", devices.Keys));
        Console.Write("Device key: ");
        string key = Console.ReadLine()?.Trim();
        if (key == null) return;
        if (!devices.TryGetValue(key, out IDevice selected))
        {
            Console.WriteLine("Device not found");
            return;
        }

        Console.Write(selected is ILockable
            ? "1. ON  2. OFF  3. LOCK  4. UNLOCK: "
            : "1. ON  2. OFF: ");
        string action = Console.ReadLine()?.Trim();
        switch (action)
        {
            case null: return;
            case "1": selected.TurnOn(); break;
            case "2": selected.TurnOff(); break;
            case "3" when selected is ILockable lockable: lockable.Lock(); break;
            case "4" when selected is ILockable unlockable: unlockable.Unlock(); break;
            default: Console.WriteLine("Invalid action"); break;
        }
    }

    private static void CreateDevice(Dictionary<string, IDevice> devices)
    {
        Console.Write("Type (light/fan/door): ");
        string type = Console.ReadLine();
        if (type == null) return;
        Console.Write("Name (also used as device key): ");
        string name = Console.ReadLine()?.Trim();
        if (name == null) return;
        if (devices.ContainsKey(name))
        {
            Console.WriteLine("Device key already exists. Choose another name.");
            return;
        }

        try
        {
            IDevice newDevice = DeviceFactory.Create(type, name);
            devices.Add(name, newDevice);
            newDevice.TurnOn();
            Console.WriteLine($"Created device. Select '{name}' in option 3.");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void ChangeNightLightLevel()
    {
        Console.Write("Night light level (0-100): ");
        string input = Console.ReadLine();
        if (input == null) return;
        if (!int.TryParse(input, out int level))
        {
            Console.WriteLine("Please enter a whole number from 0 to 100.");
            return;
        }

        try
        {
            HomeConfiguration.Instance.NightLightLevel = level;
            Console.WriteLine($"Night light level updated: {level}%");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Please enter a whole number from 0 to 100.");
        }
    }
}
