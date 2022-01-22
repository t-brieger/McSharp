using System.Net.Sockets;
using MinecraftClient.InPackets;
using MinecraftClient.Util;

namespace MinecraftClient;

public class Client : IDisposable
{
    private static readonly Dictionary<int, Func<Stream, int, InPacket>> IncomingPacketTypes;

    static Client()
    {
        IncomingPacketTypes = new Dictionary<int, Func<Stream, int, InPacket>>();

        IncomingPacketTypes.Add(0x1A, DisconnectPacket.Parse);
    }

    private readonly TcpClient tcp;

    public Client(string serverAddress, int port = 25565)
    {
        tcp = new TcpClient(serverAddress, port);
    }

    /*
     * Blocks if none are available
     * Also filters out non-user-relevant ones like Encryption, Compression or Heartbeat.
     */
    public InPacket GetNextPacket()
    {
        NetworkStream stream = tcp.GetStream();
        int size = NumUtils.ReadVarInt(stream);
        int packId = NumUtils.ReadVarInt(stream, out int t);

        if (IncomingPacketTypes.ContainsKey(packId))
        {
            return IncomingPacketTypes[packId](stream, size - t);
        }

        throw new NotImplementedException($"Unknown Packet type: {packId}");
    }

    public void SendPacket(OutgoingPacket pack)
    {
        tcp.GetStream().Write(pack.ToByteArray(false));
    }

    public void Dispose()
    {
        tcp.Dispose();
    }
}