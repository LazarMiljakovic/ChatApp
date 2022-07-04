using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Client
    {
        public string User { get; set; }
        public Guid Id { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader packet;

        public Client(TcpClient tcpClient)
        {
            ClientSocket = tcpClient;
            Id = Guid.NewGuid();
            packet = new PacketReader(ClientSocket.GetStream());
            
            var op = packet.ReadByte();
            User = packet.ReadMessage();
            Console.WriteLine($"{User} has connected!");

            Task.Run(() => Process());
        }

        void Process()
        {
            while(true)
            {
                try
                {
                    var opcode = packet.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = packet.ReadMessage();
                            Console.WriteLine($"{User}:{msg}");
                            Program.BroadCastMsg($"{User}:{msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{User}:Disconnected");
                    Program.BroadCastDisconnect(Id.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
