namespace MinecraftClient.Util;

using System.Numerics;

public static class NumUtils
{
    public static int ReadVarInt(Stream s, out int size)
    {
        int val = 0;
        int current = s.ReadByte();
        int len = 0;
        while ((current & 0x80) != 0)
        {
            val |= (current & 0x7F) << (len++ * 7);
            current = s.ReadByte();
        }
        val |= (current & 0x7F) << (len * 7);

        size = len + 1;
        return val;
    }

    public static int ReadVarInt(Stream s)
    {
        return ReadVarInt(s, out int _);
    }

    public static byte[] ToVarInt(uint x)
    {
        byte[] arr = new byte[BitOperations.Log2(x) / 7 + 1];
        for (int i = 0; i < arr.Length; i++)
        {
            byte curr = 0;
            if (i != arr.Length - 1)
                curr |= 0x80;
            curr = (byte) (curr | ((x & (0x7F << (7 * i))) >> (7 * i)));
            arr[i] = curr;
        }

        return arr;
    }
    
    public static long ReadVarLong(Stream s)
    {
        long val = 0;
        // not TECHNICALLY required to be a long, but otherwise some of the bitwise stuff below might get truncated to 4 bytes.
        long current = s.ReadByte();
        int len = 0;
        while ((current & 0x80) != 0)
        {
            val |= (current & 0x7F) << (len++ * 7);
            current = s.ReadByte();
        }
        val |= (current & 0x7F) << (len * 7);

        return val;
    }
    
    public static byte[] ToVarLong(ulong x)
    {
        byte[] arr = new byte[BitOperations.Log2(x) / 7 + 1];
        for (int i = 0; i < arr.Length; i++)
        {
            byte curr = 0;
            if (i != arr.Length - 1)
                curr |= 0x80;
            curr = (byte) (curr | ((x & (ulong) (0x7F << (7 * i))) >> (7 * i)));
            arr[i] = curr;
        }

        return arr;
    }
}