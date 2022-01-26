using MinecraftClient.Util;

namespace MinecraftClient.OutPackets;

public class HandshakePacket : OutgoingPacket
{
    public enum HandshakeTypes
    {
        STATUS, STATUSPING, PLAY
    }

    private readonly HandshakeTypes type;
    private readonly string address;
    private readonly ushort port;
    
    public HandshakePacket(HandshakeTypes hst, string addr = "", ushort port = 0)
    {
        type = hst;
        address = addr;
        this.port = port;
    }

    protected override IEnumerable<byte> GetContent()
    {
        List<byte> output = new List<byte>();
        output.AddRange(NumUtils.ToVarInt(757));
        output.AddRange(StringUtils.WriteString(address));
        output.AddRange(BitConverter.GetBytes(port));
        output.Add((byte) (type == HandshakeTypes.PLAY ? 2 : 1));
        return output;
    }

    public override byte[] ToByteArray(bool compression)
    {
        byte[] b = base.ToByteArray(compression);
        if (type == HandshakeTypes.PLAY)
            return b;
        byte[] output = new byte[b.Length + (type == HandshakeTypes.STATUS ? 2 : 12)];
        Array.Copy(b, 0, output, 0, b.Length);
        output[b.Length] = 1;
        output[b.Length + 1] = 0;
        if (type == HandshakeTypes.STATUS)
            return output;
        output[b.Length + 2] = 9;
        output[b.Length + 3] = 0x01;
        
        output[b.Length + 4] = 0x50;
        output[b.Length + 5] = 0x69;
        output[b.Length + 6] = 0x6E;
        output[b.Length + 7] = 0x67;
        output[b.Length + 8] = 0x50;
        output[b.Length + 9] = 0x6F;
        output[b.Length + 10] = 0x6E;
        output[b.Length + 11] = 0x67;
        return output;
    }

    protected override int PacketId() => 0;
}
