using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class Hasher
    {
        struct HashState
        {
            public uint a, b, c;
        }

        static void Mix(ref HashState s)
        {
            s.a -= s.b; s.a -= s.c; s.a ^= (s.c >> 13);
            s.b -= s.c; s.b -= s.a; s.b ^= (s.a << 8);
            s.c -= s.a; s.c -= s.b; s.c ^= (s.b >> 13);
            s.a -= s.b; s.a -= s.c; s.a ^= (s.c >> 12);
            s.b -= s.c; s.b -= s.a; s.b ^= (s.a << 16);
            s.c -= s.a; s.c -= s.b; s.c ^= (s.b >> 5);
            s.a -= s.b; s.a -= s.c; s.a ^= (s.c >> 3);
            s.b -= s.c; s.b -= s.a; s.b ^= (s.a << 10);
            s.c -= s.a; s.c -= s.b; s.c ^= (s.b >> 15);
        }

        static public uint JenkinsHash(byte[] data)
        {
            HashState s = new HashState();
            int len = data.Length;
            s.a = s.b = 0x9e3779b9;
            s.c = 0;
            int i = 0;
            while (i + 12 <= len)
            {
                s.a += (uint)data[i++] |
                    ((uint)data[i++] << 8) |
                    ((uint)data[i++] << 16) |
                    ((uint)data[i++] << 24);
                s.b += (uint)data[i++] |
                    ((uint)data[i++] << 8) |
                    ((uint)data[i++] << 16) |
                    ((uint)data[i++] << 24);
                s.c += (uint)data[i++] |
                    ((uint)data[i++] << 8) |
                    ((uint)data[i++] << 16) |
                    ((uint)data[i++] << 24);
                Mix(ref s);
            }
            s.c += (uint)len;
            if (i < len)
                s.a += data[i++];
            if (i < len)
                s.a += (uint)data[i++] << 8;
            if (i < len)
                s.a += (uint)data[i++] << 16;
            if (i < len)
                s.a += (uint)data[i++] << 24;
            if (i < len)
                s.b += (uint)data[i++];
            if (i < len)
                s.b += (uint)data[i++] << 8;
            if (i < len)
                s.b += (uint)data[i++] << 16;
            if (i < len)
                s.b += (uint)data[i++] << 24;
            if (i < len)
                s.c += (uint)data[i++] << 8;
            if (i < len)
                s.c += (uint)data[i++] << 16;
            if (i < len)
                s.c += (uint)data[i++] << 24;
            Mix(ref s);
            return s.c;
        }
    }
}
