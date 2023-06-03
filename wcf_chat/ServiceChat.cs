using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };

            nextId++;
            SendMsg($" {user.Name} connect!", 0);
            users.Add(user);

            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                SendMsg($" {user.Name} disconnect :(", 0);
                users.Remove(user);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    answer += $", {user.Name}: ";
                }
                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallBack(answer);
            }
        }
    }
}