using System;
using System.Windows.Forms;

namespace Space_Trespassers
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayForm playForm = new PlayForm();
            this.Hide();
            playForm.ShowDialog();
            this.Close();

        }

        private void Menu_Load(object sender, EventArgs e)
        {
            this.Focus();
        }
    }
}
