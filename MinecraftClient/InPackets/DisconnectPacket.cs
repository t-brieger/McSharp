using MinecraftClient.Util;

namespace MinecraftClient.InPackets;

public class DisconnectPacket : InPacket
{
    public readonly string Reason;

    private DisconnectPacket(string r)
    {
        Reason = r;
    }
    
    public static InPacket Parse(Stream s, int length)
    {
        return new DisconnectPacket(StringUtils.ReadString(s));
    }

    public override string ToString()
    {
        return $"Disconnected from server for reason: {Reason}";
    }
}