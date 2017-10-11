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
    public partial class DeleteUser : Form
    {
        public DeleteUser()
        {
            InitializeComponent();
            SqlConnection con;
            string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

            con = new SqlConnection();
            con.ConnectionString = connectionStr;
            con.Open();

            // Create the command
            SqlCommand command = new SqlCommand("SELECT Name FROM Users", con);

            // Create new SqlDataReader object and read data from the command.
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader[0]);
                }
            }

            con.Close();

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("This will delete user permanently!", "Are you sure?", MessageBoxButtons.YesNo);
            if (response == DialogResult.Yes)
            {
                string userLogin = comboBox1.SelectedItem.ToString();
                string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "DELETE FROM USERS where Name = @user";
                        command.Parameters.AddWithValue("@user", userLogin);


                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Successfully deleted user!");
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
           
        }
    }
}
