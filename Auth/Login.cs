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
        private string dbPassword = "";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        public static string GetHash(string str)
        {
            //переводим строку в байт-массим 
            Byte[] strBytes = Encoding.Default.GetBytes(str);
            //создаем объект для получения средст шифрования 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //вычисляем хеш-представление в байтах 
            Byte[] hashBytes = md5.ComputeHash(strBytes);
            //формируем одну цельную строку из массива 
            string hash = string.Empty;
            foreach (var item in hashBytes)
            {
                hash += string.Format("{0:x2}", item);
            }
            return hash;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SqlConnection con;
            string connectionStr = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Вадим\\documents\\visual studio 2017\\Projects\\Auth\\Auth\\Users.mdf\";Integrated Security=True;Connect Timeout=30");

            con = new SqlConnection();
            con.ConnectionString = connectionStr;
            con.Open();

            // Create the command
            SqlCommand command = new SqlCommand("SELECT Password FROM Users where name = @user", con);
            // Add the parameters.
            command.Parameters.Add(new SqlParameter("user", textBox1.Text));

            // Create new SqlDataReader object and read data from the command.
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbPassword = reader[0].ToString(); 
                }
            }

            if(dbPassword == GetHash( textBox2.Text))
            {
                MessageBox.Show("You've been logged in successfully!");
               


            }
            else
            {
                MessageBox.Show("Incorrect password or login!");


            }

            con.Close();
        }
    }
}
