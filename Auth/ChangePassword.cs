using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Auth
{
    public partial class ChangePassword : Form
    {
        private User _user;
        public ChangePassword(User user)
        {
            InitializeComponent();
            _user = user;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword;
            if (_user.PasswordHash == Hash.GetHash(textBox1.Text))
            {
                newPassword =  Hash.GetHash(textBox2.Text);
                string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "UPDATE Users SET Password = @password where Name = @user";
                        command.Parameters.AddWithValue("@user", _user.Login);
                        command.Parameters.AddWithValue("@password", newPassword);


                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Successfully changed password!");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                            Close();
                        }
                    }
                }
            }

            else { MessageBox.Show("Sorry, incorrect old password!"); }
        }
    }
}
