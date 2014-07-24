using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBS_test
{
    public partial class Form3 : Form
    {

        public Form3(string name, string subject, string date, string contents)
        {
            InitializeComponent();
            this.nameBox.Text = name;
            this.subjectBox.Text = subject;
            this.dateBox.Text = date;
            this.contentBox.Text = contents;
            
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
