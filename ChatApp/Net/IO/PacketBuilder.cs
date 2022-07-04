using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net.IO
{
    class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();
        }

        public void WriteOpcode(byte msg)
        {
            ms.WriteByte(msg);
        }

        public void WriteString(string msg)
        {
            var msgL = msg.Length;
            ms.Write(BitConverter.GetBytes(msgL));
            ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketB()
        {
            return ms.ToArray();
        }
    }
}
