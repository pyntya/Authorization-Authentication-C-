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
using System.Security.Cryptography;

namespace Auth
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;            
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Users ( Name, Password ) VALUES (@user, @password);";
                    command.Parameters.AddWithValue("@user", loginTextBox.Text);
                    command.Parameters.AddWithValue("@password", Hash.GetHash(passwordTextBox.Text));


                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successfully created user with login " + loginTextBox.Text);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Some SQL error occured.");
                    }
                    finally
                    {
                        connection.Close();
                        Close();

                    }
                }
            }

        }
    }
}
