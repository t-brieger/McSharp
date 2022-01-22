using System.Net.Sockets;

namespace MinecraftClient;
public class Client : IDisposable
{
    private TcpClient tcp;
    
    public Client(string serverAddress, int port = 25565)
    {
        tcp = new TcpClient(serverAddress, port);
    }

    /*
     * Blocks if none are available
     * Also filters out non-user-relevant ones like Encryption, Compression or Heartbeat.
     */
    public McPacket GetNextPacket()
    {
        return null;
    }

    public void SendPacket(McPacket pack)
    {
        tcp.GetStream().Write(pack.ToByteArray(false));
    }

    public void Dispose()
    {
        tcp.Dispose();
    }
}
