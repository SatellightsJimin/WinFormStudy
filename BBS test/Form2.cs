using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBS_test
{
    public partial class Form2 : Form
    {
        Form1 parentForm;
        public Form2(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nameBox.Text.Trim() == "" || subjectBox.Text.Trim() == "" || contentBox.Text.Trim() == "")
            {
                MessageBox.Show("게시물을 제대로 입력하시오.");
            }
            else
            {
                string strConn = "Data Source=D306;Initial Catalog=MyTestDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string sql
                        = "INSERT INTO MyBBS VALUES( @name , @subject , @contents , getdate() );";
                    
                    SqlParameter paramName = new SqlParameter("@name", SqlDbType.VarChar, 20);
                    SqlParameter paramSubject = new SqlParameter("@subject", SqlDbType.VarChar, 50);
                    SqlParameter paramContents = new SqlParameter("@contents", SqlDbType.Text);

                    paramName.Value = nameBox.Text.ToString();
                    paramSubject.Value = subjectBox.Text.ToString();
                    paramContents.Value = contentBox.Text.ToString();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    
                   
                    cmd.Parameters.Add(paramName);
                    cmd.Parameters.Add(paramSubject);
                    cmd.Parameters.Add(paramContents);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                    parentForm.InitializationGrid();
                    this.Close();
                }
            }
            
        }

    }
}
