using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsLikeFinal.Models;

namespace WhatsLikeFinal.DAO
{
    interface IUser
    {
        int Add(User User);
        void UpdateUser(User userUpdated);
        User GetUserByEmail(string email);
    }
}
