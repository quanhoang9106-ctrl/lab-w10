using System;

namespace SmartHomeMini;

public static class DeviceFactory
{
    public static IDevice Create(string type, string name)
    {
        type = type.Trim().ToLower();

        if (type == "light")
            return new SmartLight(name);
        else if (type == "fan")
            return new SmartFan(name);
        else if (type == "door")
            return new SmartDoor(name);
        else
            throw new ArgumentException(
                "Unsupported device type: " + type);
    }
}