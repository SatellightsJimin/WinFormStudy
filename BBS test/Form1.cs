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
   
    public partial class Form1 : Form
    {
        BBSControl bbsControl;
        DataSet dataSet;
        public Form1()
        {
            InitializeComponent();
            bbsControl = new BBSControl();
            dataSet = bbsControl.GetContents(1, 5);
  
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void writeBtn_Click(object sender, EventArgs e)
        {
            Form2 writeForm = new Form2();
            writeForm.Owner = this;
            writeForm.Show();
        }


    }
    public class BBSControl
    {
        public DataSet GetContents(int pageno, int pagesize)
        {
            DataSet ds = new DataSet();
            string strConn = "Data Source=D306;Initial Catalog=MyTestDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            string sql
            = "select a.rownum, a.ID, a.USER_NAME, a.SUBJECT, a.DATE from (select row_number() OVER (ORDER BY DATE DESC) as rownum, ID, USER_NAME, SUBJECT, DATE from MyBBS) as a where a.rownum between (@pageno * (@pageno-1)) and ((@pageno * (@pageno-1)) + @pagesize)";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            SqlParameter paramNo = new SqlParameter("@pageno", SqlDbType.Int);
            SqlParameter paramSize = new SqlParameter("@pagesize", SqlDbType.Int);
            paramNo.Value = pageno;
            paramSize.Value = pagesize;

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(paramNo);
            cmd.Parameters.Add(paramSize);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            return ds;
        }
    }
}
