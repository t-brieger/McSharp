using System.Text;

namespace MinecraftClient.Util;

public static class StringUtils
{
    private static readonly Encoding Encode = Encoding.UTF8;
    
    public static string ReadString(Stream s)
    {
        int length = NumUtils.ReadVarInt(s);
        byte[] bytes = new byte[length];
        int read = 0;
        while (read != length)
            read += s.Read(bytes, read, length - read);
        return Encode.GetString(bytes);
    }
    
    public static byte[] WriteString(string s)
    {
        List<byte> bytes = new();
        byte[] strBytes = Encode.GetBytes(s);
        bytes.AddRange(NumUtils.ToVarInt((uint) strBytes.Length));
        bytes.AddRange(strBytes);
        return bytes.ToArray();
    }
}