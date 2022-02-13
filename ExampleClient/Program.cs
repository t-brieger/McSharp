using MinecraftClient;
using MinecraftClient.InPackets;
using MinecraftClient.OutPackets;

namespace ExampleClient;

public static class ExampleClient
{
    public static int Main(string[] args)
    {
        if (args.Length < 1)
            return 1;
        if (args.Length < 2 && !args[0].Contains(':'))
            return 1;
        string address = args.Length >= 2 ? args[0] : args[0].Split(':')[0];
        ushort port = ushort.Parse(args.Length >= 2 ? args[1] : args[0].Split(':')[1]);

        Client c = new Client(address, port);
        c.CurrentState = Client.ProtocolState.HANDSHAKING;
        c.SendPacket(new HandshakePacket(HandshakePacket.HandshakeTypes.STATUS));
        c.CurrentState = Client.ProtocolState.STATUS;
        while (true)
        {
            InPacket ip = c.GetNextPacket();
            Console.WriteLine(ip);
        }
    }
}
