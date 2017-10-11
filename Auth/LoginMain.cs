using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;
using System.Data.SqlClient;

namespace Auth
{
    public partial class LoginMain : Form
    {
        private bool _allowUsersLogin = true;
        private User _user;
        
        public LoginMain()
        {
            InitializeComponent();
            SetHotKeys();

            if (!User.HasWindowsAdminRights())
            {
                BlockMenuOptionsForUser();
                изменитьToolStripMenuItem.Enabled = false;
            }
        }

        public LoginMain(User user, bool allowUsersLogin)
        {
            InitializeComponent();
            SetHotKeys();
            _user = user;
            _allowUsersLogin = allowUsersLogin;
            if (!_allowUsersLogin && user.IsAdmin) { заблокироватьToolStripMenuItem.Text = "Разблокировать"; }
            if (!user.IsAdmin) { BlockMenuOptionsForUser(); }
            /*
            textBox1.Text += user.Login;
            textBox1.Text += user.PasswordHash;
            textBox1.Text += user.RoleName;
            textBox1.Text += user.IsAdmin;
            */
        }

        public void SetUserLoginPermissions(bool condition)
        {
            _allowUsersLogin = condition;
        }

        private void SetHotKeys()
        {
            создатьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            заблокироватьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            удалитьToolStripMenuItem.ShortcutKeys = Keys.Delete;
            изменитьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            входToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
        }

        private void BlockMenuOptionsForUser()
        {
            создатьToolStripMenuItem.Enabled = false;
            заблокироватьToolStripMenuItem.Enabled = false;
            удалитьToolStripMenuItem.Enabled = false;
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addUserForm = new AddUser();
            addUserForm.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonLogin_Click(this, e);
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void заблокироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_allowUsersLogin)
            {
                MessageBox.Show("You have successfully blocked loggin-in for Users!");
                заблокироватьToolStripMenuItem.Text = "Разблокировать";
                _allowUsersLogin = false;
            }
            else
            {
                MessageBox.Show("You have successfully unblocked loggin-in for Users!");
                заблокироватьToolStripMenuItem.Text = "Заблокировать";
                _allowUsersLogin = true;
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var Name = WindowsIdentity.GetCurrent().Name;
            var Token = WindowsIdentity.GetCurrent().Token;
            var IsAuthenticated = WindowsIdentity.GetCurrent().IsAuthenticated;
            var SID = WindowsIdentity.GetCurrent().User;
            textBox1.Text += ("Сведения о текущем пользователе" + Environment.NewLine);
            textBox1.Text += ("Имя: " + Name + Environment.NewLine);
            textBox1.Text += ("Аутентифицирован: " + IsAuthenticated + Environment.NewLine);
            textBox1.Text += ("SID: " + SID + Environment.NewLine);

            
            //Определяется домен, в котором запущен поток 
            AppDomain myDomain = Thread.GetDomain();
            //Выполняется привязка к участнику при выполнении в этом домене приложения 
            myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            //Определяется текущий принципал 
            WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
            //Определяется аутентификатор принципала 
            WindowsIdentity identity = (WindowsIdentity)myPrincipal.Identity;
            //Выводятся идентификационные сведения о принципале 
            textBox1.Text += ("Тип идентификации: " + identity + Environment.NewLine);
            //Получение роли из перечисления WindowsBuiltInRole 
            textBox1.Text += ("Пользователь " + myPrincipal.IsInRole(WindowsBuiltInRole.User) + Environment.NewLine);
            //Получение роли из текстовой строки 
            textBox1.Text += ("Администратор " + myPrincipal.IsInRole(@"BUILTIN\Administrators") + Environment.NewLine);

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void GetUsers()
        {
            usersListBox.Items.Clear();
            SqlConnection con;
            string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

            con = new SqlConnection();
            con.ConnectionString = connectionStr;
            con.Open();

            // Create the command
            SqlCommand command = new SqlCommand("SELECT Name FROM Users", con);
            // Add the parameters.
            command.Parameters.Add(new SqlParameter("firstColumnValue", 1));

            // Create new SqlDataReader object and read data from the command.
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usersListBox.Items.Add(reader[0]);
                }
            }

            con.Close();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            GetUsers();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = "";
            if (usersListBox.SelectedItem != null)
            {
                login = usersListBox.SelectedItem.ToString();
            }

            this.Hide();
            var loginForm = new Login(_allowUsersLogin);
            loginForm.Closed += (s, args) => this.Close();
            loginForm.loginTextBox.Text = login;
            loginForm.Show();

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasWindowsAdminRights() || _user.IsAdmin == true)
            {
                var deleteUserForm = new DeleteUser();
                deleteUserForm.Show();
            }
            else
                MessageBox.Show("Sorry, you don't have rights for deleting users!", "No rights", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasWindowsAdminRights())
            {
                var changePasswordForm = new ChangePasswordForAdmin();
                changePasswordForm.Show();
            }
            else
            {
                var changePasswordForm = new ChangePassword(_user);
                changePasswordForm.Show();
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            GetUsers();
        }
    }
}
