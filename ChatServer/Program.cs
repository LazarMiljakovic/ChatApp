using System;
using System.Net;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Program
    {
        static List<Client> clients;
        static TcpListener listener;
        static void Main(string[] args)
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            listener.Start();

            while(true)
            {
                var client = new Client(listener.AcceptTcpClient());
                clients.Add(client);

                BroadCast();
            }
            
        }

        static void BroadCast()
        {
            foreach (var user in clients)
            {
                foreach (var clin in clients)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpcode(1);
                    broadcastPacket.WriteString(clin.User);
                    broadcastPacket.WriteString(clin.Id.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketB());
                }
            }
        }

        public static void BroadCastMsg(string msg)
        {
            foreach (var user in clients)
            {
                var msgpacket = new PacketBuilder();
                msgpacket.WriteOpcode(5);
                msgpacket.WriteString(msg);
                user.ClientSocket.Client.Send(msgpacket.GetPacketB());
            }
        }

        public static void BroadCastDisconnect(string id)
        {
            var disc = clients.Where(x => x.Id.ToString() == id).FirstOrDefault();
            clients.Remove(disc);
            foreach (var user in clients)
            {
                var brodPacket = new PacketBuilder();
                brodPacket.WriteOpcode(10);
                brodPacket.WriteString(id);
                user.ClientSocket.Client.Send(brodPacket.GetPacketB());
            }
            BroadCastMsg($"{disc.User}:Disconnected");
        }
    }
}