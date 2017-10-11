using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace Auth
{
    public partial class Login : Form
    {
        private string dbLogin = "", dbPassword = "", dbRole = "";
        private bool _allowUsersLogin;
        public Login(bool allowUsersLogin)
        {
            InitializeComponent();
            this._allowUsersLogin = allowUsersLogin;
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection con;
            string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

            con = new SqlConnection();
            con.ConnectionString = connectionStr;
            con.Open();

            // Create the command
            SqlCommand command = new SqlCommand("SELECT Users.Name, Password, Roles.Name FROM Users Join Roles ON Roles.Id = Users.Role_Id where Users.Name = @user", con);
            // Add the parameters.
            command.Parameters.Add(new SqlParameter("user", loginTextBox.Text));

            // Create new SqlDataReader object and read data from the command.
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbLogin = reader[0].ToString();
                    dbPassword = reader[1].ToString();
                    dbRole = reader[2].ToString();
                }
            }

            if (dbRole != "Admin" && ! _allowUsersLogin)
            {
                MessageBox.Show("Sorry, administrator had blocked logging-in for users!", "Authorization error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Hide();
                var mainForm = new LoginMain();
                mainForm.Closed += (s, args) => this.Close();
                mainForm.SetUserLoginPermissions(_allowUsersLogin);
                mainForm.Show();
                return;
            }

            if (dbPassword == Hash.GetHash(passwordTextBox.Text))
            {
                MessageBox.Show("You've been logged in successfully!");
                User user = new User(dbLogin, dbPassword, dbRole);
                this.Hide();
                var mainForm = new LoginMain(user, _allowUsersLogin);
                mainForm.Closed += (s, args) => this.Close();
                mainForm.Show();



            }
            else
            {
                MessageBox.Show("Incorrect password or login!", "Authorization error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            con.Close();
        }
    }
}
