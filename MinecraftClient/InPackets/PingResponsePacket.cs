namespace MinecraftClient.InPackets;

public class PingResponsePacket : InPacket
{
    public static InPacket Parse(Stream s, int length, Client _)
    {
        // possible todo; maybe compare it to what we sent?
        for (int i = 0; i < 8; i++)
            s.ReadByte();
        return new PingResponsePacket();
    }

    public override string ToString()
    {
        return "Ping successful.";
    }
}
