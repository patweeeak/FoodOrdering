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
    public partial class Home : Form
    {
        public static Home instance;
        private Form currentchildForm;
        public bool activeForm = false;
        public Home()
        {
            instance = this;
            InitializeComponent();
        }
        #region Upperbuttons
        private void exit_Click(object sender, EventArgs e)
        {
            if(activeForm == true)
            {
                OpenChildForm(new Front(), 5, 60);
            }
            else
            {
                Application.Exit();
            }
        }

        private void maximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        private void Home_Load(object sender, EventArgs e)
        {
            OpenChildForm(new Front(), 5, 60);
        }

        public void OpenChildForm(Form childForm, int xpos, int ypos)
        {
            if (currentchildForm != null)
            {
                currentchildForm.Hide();
            }
            currentchildForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.None;
            this.Controls.Add(childForm);
            this.Tag = childForm;
            //childForm.BringToFront();
            childForm.Show();
            childForm.Size = new Size(1527, 800);
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Location = new Point(xpos, ypos);
        }
    }
}
