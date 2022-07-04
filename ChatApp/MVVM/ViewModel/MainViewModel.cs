using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Net;
using ChatApp.MVVM;
using ChatApp.MVVM.Core;
using ChatApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChatApp.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Msgs { get; set; }
        public RelayCommand ConnectComand { get; set; }
        public RelayCommand SendMsgComand { get; set; }

        private Server server;
        public string name { get; set; }
        public string Msg { get; set; }

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Msgs = new ObservableCollection<string>();
            server = new Server();
            server.connectedEv += UserConnected;
            server.msgRec += MsgReceived;
            server.UserDisc += UserDisconnect;
            ConnectComand = new RelayCommand(o=>server.Connect(name),o=>!string.IsNullOrEmpty(name));
            SendMsgComand = new RelayCommand(o => server.SendMsgServer(Msg), o => !string.IsNullOrEmpty(Msg));

        }

        private void UserDisconnect()
        {
            var id = server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.ID == id).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void MsgReceived()
        {
            var msg = server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Msgs.Add(msg));
        }

        private void UserConnected()
        {
            var usr = new UserModel
            {
                Name = server.PacketReader.ReadMessage(),
                ID = server.PacketReader.ReadMessage(),
            };

            if(!Users.Any(x =>x.ID == usr.ID))
            {
                Application.Current.Dispatcher.Invoke(()=> Users.Add(usr));
            }
        }
    }
}
