using MinecraftClient.InPackets;
using MinecraftClient.OutPackets;

namespace MinecraftClient;

public partial class Client
{
    public static StatusPacket.ServerStatus GetServerStatus(string ip, ushort port)
    {
        Client c = new(ip, port);
        c.SendPacket(new HandshakePacket(HandshakePacket.HandshakeTypes.STATUS, ip, port));
        c.CurrentState = ProtocolState.STATUS;
        InPacket pack = c.GetNextPacket();
        while (pack is not StatusPacket)
            pack = c.GetNextPacket();
        return (pack as StatusPacket)!.Status!;
    }
}