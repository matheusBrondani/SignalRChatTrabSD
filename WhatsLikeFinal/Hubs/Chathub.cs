using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WhatsLikeFinal.DAO;
using WhatsLikeFinal.Models;

namespace WhatsLikeFinal.Hubs
{
    public class Chathub : Hub
    {
        private UserRepository userRepo = new UserRepository();
        static List<User> ConnectedUsers = new List<User>();

        public void Connect(string userEmail, int idUser)
        {
            var connectionId = Context.ConnectionId;

            userRepo.UpdateConnectionId(idUser, connectionId);

            if (ConnectedUsers.Count(x => x.ConnectionId == connectionId) == 0)
            {
                ConnectedUsers.Add(new User { ConnectionId = connectionId, UserName = userEmail });
                
                // send to caller
                Clients.Caller.onConnected(connectionId, userEmail, userRepo.GetContatosByIdUser(idUser));

                // send to all except caller client
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userEmail);

            }

        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            //AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        public void SendMessageToConversa(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        //public override System.Threading.Tasks.Task OnDisconnected()
        //{
        //    var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        //    if (item != null)
        //    {
        //        ConnectedUsers.Remove(item);

        //        var id = Context.ConnectionId;
        //        Clients.All.onUserDisconnected(id, item.UserName);

        //    }

        //    return base.OnDisconnected();
        //}

        //private void AddMessageinCache(string userName, string message)
        //{
        //    CurrentMessage.Add(new Messages { UserName = userName, Message = message });

        //    if (CurrentMessage.Count > 100)
        //        CurrentMessage.RemoveAt(0);
        //}

        public void AddContato(string email, int idUser) {
            int idConversa = userRepo.AddContato(email,idUser);
            User u = userRepo.GetUserByEmail(email);
            Clients.Caller.addContatoLista(u.IdUser,u.UserName,idConversa);
        }

        private void GetMessagesByConversa(int idUser, int idCont, int idConversa)
        {
            List<Messages> mensagens = userRepo.GetMessagesByConversa(idConversa);
            Clients.Caller.AddMessagesConversa(mensagens);
        }
    }
}