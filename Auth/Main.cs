using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;

namespace Auth
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            создатьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            заблокироватьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            удалитьToolStripMenuItem.ShortcutKeys = Keys.Delete;
            изменитьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            входToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void заблокироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Name = WindowsIdentity.GetCurrent().Name;
            var Token = WindowsIdentity.GetCurrent().Token;
            var IsAuthenticated = WindowsIdentity.GetCurrent().IsAuthenticated;
            var SID = WindowsIdentity.GetCurrent().User;
            textBox1.Text += ("Сведения о текущем пользователе" + Environment.NewLine);
            textBox1.Text += ("Имя: " + Name +Environment.NewLine);
            textBox1.Text += ("Аутентифицирован: " + IsAuthenticated + Environment.NewLine);
            textBox1.Text += ("SID: " + SID + Environment.NewLine);

        }
    }
}
