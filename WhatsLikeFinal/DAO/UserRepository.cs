using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WhatsLikeFinal.ADOBD;
using WhatsLikeFinal.Models;

namespace WhatsLikeFinal.DAO
{
    public class UserRepository : IUser
    {
        //Add Usuario no sistema
        public int Add(User addUser) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                USUARIO user = new USUARIO
                {
                    email = addUser.Email,
                    senha = addUser.Senha,
                    id_conection = addUser.ConnectionId,
                    nome = addUser.UserName,
                    nick = addUser.UserNick,
                    online = addUser.OnLine
                };

                db.USUARIO.Add(user);
                db.SaveChanges();

                return GetIdByEmail(addUser.Email);
            }
        }

        // Add Contato para o usuario
        public int AddContato(string email, int idUser) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                User u = GetUserByEmail(email);

                CONTATOS cont = new CONTATOS
                {
                    id_usu = idUser,
                    id_cont = u.IdUser
                };

                CONTATOS otherCont = new CONTATOS
                {
                    id_usu = u.IdUser,
                    id_cont = idUser
                };

                CONVERSAS conversa = new CONVERSAS
                {
                    nome = null,
                    e_grupo = 0
                };
                
                db.CONTATOS.Add(cont);
                db.CONTATOS.Add(otherCont);
                db.CONVERSAS.Add(conversa);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand("INSERT INTO INTEGRANTES(id_usu, id_conversa) VALUES(@idUser, @id_conversa)", new SqlParameter("@idUser", idUser), new SqlParameter("@id_conversa", conversa.id_conversa));
                db.Database.ExecuteSqlCommand("INSERT INTO INTEGRANTES(id_usu, id_conversa) VALUES(@idUser, @id_conversa)", new SqlParameter("@idUser", u.IdUser), new SqlParameter("@id_conversa", conversa.id_conversa));

                return conversa.id_conversa;
            }
        }

        //Deleta contato do usuario
        public void DelContato(int idCont, int idUser)
        {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                CONTATOS cont = db.CONTATOS.Where(x => x.id_usu == idUser && x.id_cont == idCont).First();
                CONTATOS otherCont = db.CONTATOS.Where(x => x.id_usu == idCont && x.id_cont == idUser).First();
                db.CONTATOS.Remove(cont);
                db.CONTATOS.Remove(otherCont);
                db.SaveChanges();
            }
        }
        
        public List<Messages> GetMessagesByConversa(int idConversa)
        {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                List<MENSAGENS> ListMensagens = db.MENSAGENS.Where(x => x.id_conversa == idConversa).ToList();
                List<Messages> mensagens = new List<Messages>();

                foreach (MENSAGENS m in ListMensagens) {
                    Messages men = new Messages {
                        IdMensagem = m.id_mensagens,
                        UserName = GetUserById((int)m.id_sender).UserName,
                        IdConversa = (int)m.id_conversa,
                        IdSender = (int)m.id_sender,
                        Entregue = (int)m.entregue,
                        Lida = (int)m.lida,
                        Mensagem = m.mensagem
                    };
                    mensagens.Add(men);
                }

                return mensagens;
            }
        }

        public void AddMessage(Messages mensagem){
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                MENSAGENS men = new MENSAGENS
                {
                    id_conversa = mensagem.IdConversa,
                    id_sender = mensagem.IdSender,
                    lida = mensagem.Lida,
                    entregue = mensagem.Entregue,
                    mensagem = mensagem.Mensagem
                };

                db.MENSAGENS.Add(men);
                db.SaveChanges();
            }
        }

        //Atualiza informações do Usuario
        public void UpdateUser(User userUpdated)
        {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                USUARIO a = db.USUARIO.FirstOrDefault(u => u.id_usuario == userUpdated.IdUser);

                a.id_conection = userUpdated.ConnectionId;
                a.nick = userUpdated.UserNick;
                a.nome = userUpdated.UserName;
                a.online = userUpdated.OnLine;
                a.senha = userUpdated.Senha;

                db.SaveChanges();
            }
        }

        //Atualiza string de conexao no HUB e seta como online
        public void UpdateConnectionId(int idUser, string connectionId)
        {
            User u = GetUserById(idUser);
            u.ConnectionId = connectionId;
            u.OnLine = 1;
            UpdateUser(u);
        }

        //Retorna usuario pesquisando pelo e-mail
        public User GetUserByEmail(string email) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                USUARIO a = db.USUARIO.FirstOrDefault(u => u.email == email);

                if (!(a == null)) {
                    User user = new User
                    {
                        IdUser = a.id_usuario,
                        Email = a.email,
                        Senha = a.senha,
                        ConnectionId = a.id_conection,
                        UserName = a.nome,
                        UserNick = a.nick,
                        OnLine = (int)a.online
                    };
                    return user;
                } else {
                    User user = null;
                    return user;
                }                
            }            
        }

        //Retorna o id do usuario pesquisando pelo e-mail
        public int GetIdByEmail(string email) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                return db.USUARIO.FirstOrDefault(u => u.email == email).id_usuario;
            }
        }

        //Retorna o usuario pesquisando pelo id 
        public User GetUserById(int idUserGet) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                USUARIO a = db.USUARIO.FirstOrDefault(u => u.id_usuario == idUserGet);
                
                User user = new User
                { 
                    IdUser = a.id_usuario,
                    Email = a.email,
                    Senha = a.senha,
                    ConnectionId = a.id_conection,
                    UserName = a.nome,
                    UserNick = a.nick,
                    OnLine = (int)a.online
                };
                return user;
            }
        }

        //Retorna os contatos em forma de lista de usuarios pesquisando pelo o id do usuario
        public List<User> GetContatosByIdUser(int id) {
            using (SignalRBDEntities db = new SignalRBDEntities())
            {
                USUARIO a = db.USUARIO.FirstOrDefault(u => u.id_usuario == id);

                List<CONTATOS> listContatos = a.CONTATOS1.ToList();

                List<User> listUsers = new List<User>();

                foreach(CONTATOS c in listContatos)
                {
                    listUsers.Add(GetUserById((int)c.id_cont));
                }

                return listUsers;
            }
        }
    }
}