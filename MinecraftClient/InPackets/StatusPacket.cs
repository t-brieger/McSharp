using MinecraftClient.Util;
using Newtonsoft.Json;

namespace MinecraftClient.InPackets;

public class StatusPacket : InPacket
{
    public class ServerStatus
    {
        public class StatusVersion
        {
            public string Name;
            public int Protocol;
        }

        public class StatusPlayers
        {
            public class StatusPlayer
            {
                public string Name;
                public string Id;
            }

            public int Max, Online;
            public StatusPlayer[] Sample;
        }

        public class StatusDescription
        {
            public string Text;
        }
        
        public StatusVersion Version;
        public StatusPlayers Players;
        public StatusDescription Description;
        public string Favicon;
        
        public override string ToString()
        {
            return $"Version:   {Version.Name} ({Version.Protocol})\n" +
                   $"Players:   {Players.Online}/{Players.Max}\n" +
                   $"MOTD:      {Description.Text}\n" + 
                   (Favicon == null ? "" : "Favicon is set.\n");
        }
    }

    public ServerStatus? Status;
    
    private StatusPacket(string json)
    {
        Status = JsonConvert.DeserializeObject<ServerStatus>(json);
    }
    
    public static InPacket Parse(Stream s, int length, Client _)
    {
        return new StatusPacket(StringUtils.ReadString(s));
    }
    
    public override string ToString()
    {
        return $"Server Status: \n{Status}";
    }
}
