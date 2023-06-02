using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    public partial class MainWindow : Window, IServiceChatCallback
    {
        ServiceChat.ServiceChatClient client;
        bool isConnected = false;
        int Id;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && client != null)
            {
                client.SendMsg(tbMessage.Text, Id);
                tbMessage.Text = string.Empty;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                Id = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnDisc.Content = "Disconnect";
                isConnected = true;
            }
        }
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(Id);
                client = null;
                tbUserName.IsEnabled = true;
                bConnDisc.Content = "Connect";
                isConnected = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallBack(string msg)
        {
            lbChat.Items.Add(msg);
            //lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }
    }
}