using MinecraftClient.Util;

namespace MinecraftClient.OutPackets;

public class HandshakePacket : OutgoingPacket
{
    private readonly bool status;
    private readonly string address;
    private readonly ushort port;
    
    public HandshakePacket(bool onlyStatus, string addr, ushort port)
    {
        status = onlyStatus;
        address = addr;
        this.port = port;
    }

    protected override IEnumerable<byte> GetContent()
    {
        List<byte> output = new List<byte>();
        output.AddRange(NumUtils.ToVarInt(757));
        output.AddRange(StringUtils.WriteString(address));
        output.AddRange(BitConverter.GetBytes(port));
        output.Add((byte) (status ? 1 : 2));
        return output;
    }

    public override byte[] ToByteArray(bool compression)
    {
        byte[] b = base.ToByteArray(compression);
        if (!status)
            return b;
        byte[] output = new byte[b.Length + 2];
        Array.Copy(b, 0, output, 0, b.Length);
        output[^2] = 1;
        output[^1] = 0;
        return output;
    }

    protected override int PacketId() => 0;
}