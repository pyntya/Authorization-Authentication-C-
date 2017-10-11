using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace Auth
{
    public class User
    {
        private string _login, _passwordHash, _roleName;
        private bool isAdmin;

        public User(string login, string passwordHash, string roleName)
        {
            this._login = login;
            this._passwordHash = passwordHash;
            this._roleName = roleName;
            if (_roleName == "Admin")
                isAdmin = true;
            else
                isAdmin = false;
        }

        public string Login { get => _login; set => _login = value; }
        public string PasswordHash { get => _passwordHash; set => _passwordHash = value; }
        public string RoleName { get => _roleName; set => _roleName = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }


        public static bool HasWindowsAdminRights()
        {
            //Определяется домен, в котором запущен поток 
            AppDomain myDomain = Thread.GetDomain();
            //Выполняется привязка к участнику при выполнении в этом домене приложения 
            myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            //Определяется текущий принципал 
            WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
            //Определяется аутентификатор принципала 
            WindowsIdentity identity = (WindowsIdentity)myPrincipal.Identity;
            //Выводятся идентификационные сведения о принципале 
            return myPrincipal.IsInRole(@"BUILTIN\Administrators");
        }
    }
}
