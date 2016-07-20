using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEncode
{
    public class IVFWriter
    {

        public uint GetFileHeaderSize() { return 32; }
        public uint GetFrameHeaderSize() { return 12; }

        public uint WriteFileHeader(byte[] mem,
                                    string codec,
                                    int width,
                                    int height)
        {
            mem[0] = (byte)'D';
            mem[1] = (byte)'K';
            mem[2] = (byte)'I';
            mem[3] = (byte)'F';
            PutLE16(mem, 4, 0);                               // version
            PutLE16(mem, 6, 32);                              // header size
            PutLE32(mem, 8, fourcc(codec[0], codec[1], codec[2], '0'));  // fourcc
            PutLE16(mem, 12, (ushort)(width));   // width
            PutLE16(mem, 14, (ushort)(height));  // height
            PutLE32(mem, 16, 1000);                           // rate
            PutLE32(mem, 20, 1);                              // scale
            PutLE32(mem, 24, 0xffffffff);                     // length
            PutLE32(mem, 28, 0);                              // unused

            return 32;
        }

        public uint WriteFrameHeader(byte[] mem,
                                     uint offset,
                                     long pts,
                                     uint frame_size)
        {
            PutLE32(mem, offset, frame_size);
            PutLE32(mem, offset + 4, (uint)(pts & 0xFFFFFFFF));
            PutLE32(mem, offset + 8, (uint)(pts >> 32));

            return 12;
        }
        static uint fourcc(char a, char b, char c, char d)
        {
            return (((uint)(a) << 0) | ((uint)(b) << 8) | ((uint)(c) << 16) |
             ((uint)(d) << 24));
        }

        void PutLE16(byte[] mem, uint offset, uint val)
        {
            mem[offset++] = (byte)((val >> 0) & 0xff);
            mem[offset++] = (byte)((val >> 8) & 0xff);
        }
        void PutLE32(byte[] mem, uint offset, uint val)
        {
            mem[offset++] = (byte)((val >> 0) & 0xff);
            mem[offset++] = (byte)((val >> 8) & 0xff);
            mem[offset++] = (byte)((val >> 16) & 0xff);
            mem[offset++] = (byte)((val >> 24) & 0xff);
      }
    }
}
