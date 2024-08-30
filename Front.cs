using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodOrdering
{
    public partial class Front : Form
    {
        private Form currentchildForm;
        public static Front instance;
        public Front()
        {
            InitializeComponent();
            instance = this;
        }

        public void mngInventory_Click(object sender, EventArgs e)
        {
            Home.instance.OpenChildForm(new Inventory(), 5, 60);
        }

        private void orderFood_Click(object sender, EventArgs e)
        {
            Home.instance.OpenChildForm(new OrderFood(), 5, 60);
        }

        private void Front_Load(object sender, EventArgs e)
        {
            Home.instance.activeForm = false;
        }

        private void logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Log-Out?", "Notice!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
/*                Form1 f1 = new Form1();
                this.WindowState = FormWindowState.Minimized;
                Home.instance.WindowState = FormWindowState.Minimized;
                f1.Show();*/
            }
            else
            {

            }
        }
    }
}
