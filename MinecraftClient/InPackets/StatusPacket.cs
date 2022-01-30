using MinecraftClient.Util;
using Newtonsoft.Json.Linq;

namespace MinecraftClient.InPackets;

public class StatusPacket : InPacket
{
    public class ServerStatus
    {
        public class StatusVersion
        {
            public readonly string Name = null!;
            public readonly int Protocol = -1;
        }

        public class StatusPlayers
        {
            public class StatusPlayer
            {
                public readonly string Name = null!;
                public readonly string Id = null!;
            }

            public readonly int Max = 0, Online = 0;
            public readonly StatusPlayer[] Sample = null!;
        }

        public class StatusDescription
        {
            public string Text = null!;
        }
        
        public StatusVersion Version = null!;
        public StatusPlayers Players = null!;
        public StatusDescription Description = null!;
        public string Favicon = null!;
        
        public override string ToString()
        {
            return $"Version:   {Version.Name} ({Version.Protocol})\n" +
                   $"Players:   {Players.Online}/{Players.Max}\n" +
                   $"MOTD:      {Description.Text}\n" + 
                   (string.IsNullOrEmpty(this.Favicon) ? "" : "Favicon is set.\n");
        }
    }

    public readonly ServerStatus? Status;
    
    private StatusPacket(string json)
    {
        JToken jo = JToken.Parse(json);
        // this is probably about as unstable as it looks, but idk how else to have description be either a string, or
        // an object with the field "text", instead of only one of the two
        Status = new ServerStatus
        {
            Players = jo["players"]!.ToObject<ServerStatus.StatusPlayers>()!,
            Description = new ServerStatus.StatusDescription
            {
                Text = (jo["description"]!.Type == JTokenType.String
                    ? jo.Value<string>("description")
                    : jo["description"]!.Value<string>("text"))!
            },
            Favicon = jo.Value<string>("favicon"),
            Version = jo["version"]!.ToObject<ServerStatus.StatusVersion>()!
        };
        
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
