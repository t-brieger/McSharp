using System.Net.Sockets;
using MinecraftClient.InPackets;
using MinecraftClient.OutPackets;
using MinecraftClient.Util;

namespace MinecraftClient;

public partial class Client : IDisposable
{
    public enum ProtocolState
    {
        HANDSHAKING,
        STATUS,
        LOGIN,
        PLAY,
        ALL,
        // for keep-alive packets, for example.
        DO_NOT_RETURN
    }

    private static readonly Dictionary<ProtocolState, Dictionary<int, Func<Stream, int, Client, InPacket>>> IncomingPacketTypes;

    static Client()
    {
        IncomingPacketTypes = new Dictionary<ProtocolState, Dictionary<int, Func<Stream, int, Client, InPacket>>>();
        foreach (ProtocolState ps in Enum.GetValues(typeof(ProtocolState)))
            IncomingPacketTypes.Add(ps, new Dictionary<int, Func<Stream, int, Client, InPacket>>());

        IncomingPacketTypes[ProtocolState.STATUS].Add(0x00, StatusPacket.Parse);
        IncomingPacketTypes[ProtocolState.STATUS].Add(0x01, PingResponsePacket.Parse);
        
        IncomingPacketTypes[ProtocolState.ALL].Add(0x1A, DisconnectPacket.Parse);
    }

    private readonly TcpClient tcp;
    public ProtocolState CurrentState;

    public Client(string serverAddress, int port = 25565)
    {
        tcp = new TcpClient(serverAddress, port);
        this.CurrentState = ProtocolState.HANDSHAKING;
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

        if (IncomingPacketTypes[this.CurrentState].ContainsKey(packId))
            return IncomingPacketTypes[this.CurrentState][packId](stream, size - t, this);
        if (IncomingPacketTypes[ProtocolState.ALL].ContainsKey(packId))
            return IncomingPacketTypes[ProtocolState.ALL][packId](stream, size - t, this);

        for (int i = 0; i < size - t; i++)
            stream.ReadByte();

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
