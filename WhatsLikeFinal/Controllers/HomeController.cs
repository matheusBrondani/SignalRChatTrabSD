using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsLikeFinal.DAO;
using WhatsLikeFinal.Models;

namespace WhatsLikeFinal.Controllers
{
    public class HomeController : Controller
    {
        private UserRepository userRepo = new UserRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User LoginUser)
        {
            User u = userRepo.GetUserByEmail(LoginUser.Email);

            if (u != null && u.Email.Equals(LoginUser.Email) && u.Senha.Equals(LoginUser.Senha)) {
                return RedirectToAction("Chat", "Home", new { id = u.IdUser });
            } else {
                return View();
            }            
        }

        public ActionResult Chat(int id)
        {
            ViewBag.User = userRepo.GetUserById(id);
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(User usuario)
        {
            userRepo.Add(usuario);
            return RedirectToAction("Chat", "Home", new { id = userRepo.GetIdByEmail(usuario.Email) });
        }
    }
}