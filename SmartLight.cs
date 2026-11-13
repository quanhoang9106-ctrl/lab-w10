using System;

namespace SmartHomeMini;

public class SmartLight : IDevice
{
    public string Name { get; private set; }
    public bool IsOn { get; private set; }

    public SmartLight(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
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
}
