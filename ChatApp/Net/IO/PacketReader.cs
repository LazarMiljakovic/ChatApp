using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream ns;
        public PacketReader(NetworkStream ns):base(ns)
        {
            this.ns = ns;
        }

        public string ReadMessage()
        {
            byte[] buff;
            var len = ReadInt32();
            buff = new byte[len];
            ns.Read(buff, 0, len);

            var msg = Encoding.ASCII.GetString(buff);
            return msg;
        }
    }
}
