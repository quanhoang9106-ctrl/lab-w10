using System;

namespace SmartHomeMini;

public class SmartDoor : IDevice, ILockable
{
    public string Name { get; set; }
    public bool IsOn { get; private set; }
    public bool IsLocked { get; private set; }

    public SmartDoor(string name)
    {
        Name = name;
    }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine(Name + ": ON");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine(Name + ": OFF");
    }

    public void Lock()
    {
        IsLocked = true;
        Console.WriteLine(Name + ": LOCKED");
    }

    public void Unlock()
    {
        IsLocked = false;
        Console.WriteLine(Name + ": UNLOCKED");
    }
}