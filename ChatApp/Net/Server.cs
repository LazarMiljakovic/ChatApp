using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Net.IO;
using ChatApp.MVVM;
using ChatApp.MVVM.ViewModel;

namespace ChatApp.Net
{
    class Server
    {
       
        TcpClient tcpClient;
        public PacketReader PacketReader;

        public event Action connectedEv;
        public event Action msgRec;
        public event Action UserDisc;
        public Server()
        {
            tcpClient = new TcpClient();
        }

        public void Connect(string name)
        {
            if(!tcpClient.Connected)
            {
                tcpClient.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(tcpClient.GetStream());

                if(!string.IsNullOrEmpty(name))
                {
                    var connpacket = new PacketBuilder();
                    connpacket.WriteOpcode(0);
                    connpacket.WriteString(name);
                    tcpClient.Client.Send(connpacket.GetPacketB());
                }
                ReadPacket();
            }
        }

        private void ReadPacket()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var op = PacketReader.ReadByte();
                    switch (op)
                    {
                        case 1:
                            connectedEv?.Invoke();
                            break;
                        case 5:
                            msgRec?.Invoke();
                            break;
                        case 10:
                            UserDisc?.Invoke();
                            break;
                        default:
                            Console.WriteLine(" ");
                            break;
                    }
                }
            });
        }

        public void SendMsgServer(string msg)
        {
            var msgPacket = new PacketBuilder();

            msgPacket.WriteOpcode(5);
            msgPacket.WriteString(msg);

            tcpClient.Client.Send(msgPacket.GetPacketB());
        }
    }
}
