using System;

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

        IDevice light = DeviceFactory.Create(
            "light", "Living room light");

        IDevice fan = DeviceFactory.Create(
            "fan", "Living room fan");

        IDevice door = DeviceFactory.Create(
            "door", "Main door");

        SmartHomeFacade home =
            new SmartHomeFacade(
                light, fan, (ILockable)door);

        while (true)
        {
            Console.WriteLine("\n===== SMART HOME MINI =====");
            Console.WriteLine("1. Go to work");
            Console.WriteLine("2. Go to sleep");
            Console.WriteLine("3. Control a device");
            Console.WriteLine("4. Create a new device");
            Console.WriteLine("5. Change night light level");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            if (choice == "0")
                break;

            else if (choice == "1")
                home.GoToWork();

            else if (choice == "2")
                home.GoToSleep();

            else if (choice == "3")
            {
                Console.Write("Device (light/fan/door): ");
                string type = Console.ReadLine();

                IDevice selected = null;

                if (type == "light")
                    selected = light;
                else if (type == "fan")
                    selected = fan;
                else if (type == "door")
                    selected = door;

                if (selected == null)
                {
                    Console.WriteLine("Device not found");
                    continue;
                }

                Console.Write(
                    "1. ON  2. OFF  3. LOCK  4. UNLOCK: ");

                string action = Console.ReadLine();

                if (action == "1")
                    selected.TurnOn();

                else if (action == "2")
                    selected.TurnOff();

                else if (action == "3" && selected is ILockable)
                    ((ILockable)selected).Lock();

                else if (action == "4" && selected is ILockable)
                    ((ILockable)selected).Unlock();

                else
                    Console.WriteLine("Invalid action");
            }

            else if (choice == "4")
            {
                Console.Write("Type (light/fan/door): ");
                string type = Console.ReadLine();

                Console.Write("Name: ");
                string name = Console.ReadLine();

                try
                {
                    IDevice newDevice =
                        DeviceFactory.Create(type, name);

                    newDevice.TurnOn();
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            else if (choice == "5")
            {
                Console.Write("Night light level: ");

                int level;

                if (int.TryParse(
                    Console.ReadLine(), out level))
                {
                    HomeConfiguration.Instance
                        .NightLightLevel = level;
                }
                else
                {
                    Console.WriteLine(
                        "Please enter a number");
                }
            }

            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
    }
}