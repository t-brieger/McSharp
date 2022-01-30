using MinecraftClient.Util;

namespace MinecraftClient.OutPackets;

public abstract class OutgoingPacket
{
    protected abstract IEnumerable<byte> GetContent();
    protected abstract int PacketId();
    
    public virtual byte[] ToByteArray(bool compression)
    {
        if (compression)
            throw new NotImplementedException();
        byte[] body = this.GetContent().ToArray();
        List<byte> output = new(body.Length + 10);
        byte[] packetIdBytes = NumUtils.ToVarInt((uint) this.PacketId());
        output.AddRange(NumUtils.ToVarInt((uint) (body.Length + packetIdBytes.Length)));
        output.AddRange(packetIdBytes);
        output.AddRange(body);
        return output.ToArray();
    }
}