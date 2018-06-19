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

                List<User> listCont = userRepo.GetContatosByIdUser(idUser);
                
                //Chama função de logon do usuario
                Clients.Caller.onConnected(connectionId, userEmail, listCont);                
            }

        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            //AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        public void SendMessageToContato(int idUser, int idConversa, string message)
        {
            //Armazena a mensagem
            Messages men = new Messages() {
                IdConversa = idConversa,
                IdSender = idUser,
                Lida = 0,
                Entregue = 1,
                Mensagem = message
            };

            userRepo.AddMessageInBD(men);

            //string fromUserId = Context.ConnectionId;

            //var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            //var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            // send to 
            //Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

            // send to caller user
            Clients.Caller.sendPrivateMessage(idUser, message);
          
        }

        public void SendMessageToPrivate(string toUserId, string message)
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
            userRepo.AddContato(email,idUser);
            User u = userRepo.GetUserByEmail(email);
            Clients.Caller.addContatoLista(u.UserName,u.IdUser);
        }

        public void GetMessagesByConversa(int idUser, int idCont)
        {
            int idConversa = userRepo.GetIdConversaByUserContato(idUser, idCont);
            List<Messages> mensagens = userRepo.GetMessagesByConversa(idConversa);
            Clients.Caller.addMessagesConversa(mensagens, idConversa);
        }
    }
}