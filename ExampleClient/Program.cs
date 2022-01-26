using System.Diagnostics;
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

        Stopwatch sw = new Stopwatch();
        
        c.SendPacket(new HandshakePacket(HandshakePacket.HandshakeTypes.STATUSPING, address, port));
        // something about this is very slow, not sure why - i get 30 ping using notchian client, 200+ from this
        sw.Start();
        c.currentState = Client.ProtocolState.STATUS;

        while (true)
        {
            InPacket ip = c.GetNextPacket();
            if (ip is PingResponsePacket prp)
            {
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds + "ms ping");
            }else
                Console.WriteLine(ip);
        }
    }
}
