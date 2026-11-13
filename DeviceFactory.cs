using System;

namespace SmartHomeMini;

public static class DeviceFactory
{
    public static IDevice Create(string type, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return type.Trim().ToLowerInvariant() switch
        {
            "light" => new SmartLight(name.Trim()),
            "fan" => new SmartFan(name.Trim()),
            "door" => new SmartDoor(name.Trim()),
            _ => throw new ArgumentException($"Unsupported device type: {type}", nameof(type))
        };
    }
}
