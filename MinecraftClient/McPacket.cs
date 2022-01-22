using MinecraftClient.Util;

namespace MinecraftClient;

public abstract class McPacket
{
    public abstract int GetSize();
    public abstract byte[] GetContent();
    public int PacketId;
    
    public byte[] ToByteArray(bool compression)
    {
        if (compression)
            throw new NotImplementedException();
        List<byte> output = new List<byte>(GetSize() + 5);
        output.AddRange(NumUtils.GetVarInt((uint)GetSize()));
        output.AddRange(NumUtils.GetVarInt((uint)PacketId));
        output.AddRange(GetContent());
        return output.ToArray();
    }
}