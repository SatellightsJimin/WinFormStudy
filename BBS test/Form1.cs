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
        int pageno = 1;
        int pagesize = 15;
        public Form1()
        {
            InitializeComponent();
            InitializationGrid();
        }
        public void InitializationGrid(){
            bbsControl = new BBSControl();
            dataSet = bbsControl.GetContents(pageno, pagesize);

            dataGridView1.DataSource = dataSet.Tables[0];
            for (int i = 0 ; i < dataGridView1.Columns.Count; i++)
            {
               
                if (dataGridView1.Columns[i].Name == "rownum")
                {
                    dataGridView1.Columns[i].FillWeight = 5;
                }
                if (dataGridView1.Columns[i].Name == "USER_NAME")
                {
                    dataGridView1.Columns[i].FillWeight = 10;
                }
                if (dataGridView1.Columns[i].Name == "DATE")
                {
                    dataGridView1.Columns[i].FillWeight = 20;
                }

            }
        }

        private void writeBtn_Click(object sender, EventArgs e)
        {
            Form2 writeForm = new Form2(this);
            writeForm.Owner = this;
            writeForm.Show();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            pageno += 1;
            dataSet = bbsControl.GetContents(pageno, pagesize);
            if(dataSet.Tables[0].Rows.Count==0)
            {
                MessageBox.Show("게시물이 없음");
                pageno -= 1;
            }
            else
            {
                dataGridView1.DataSource = dataSet.Tables[0];
            }
                
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            pageno -= 1;
            dataSet = bbsControl.GetContents(pageno, pagesize);
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("첫페이지입니다");
                pageno += 1;
            }
            else
            {
                dataGridView1.DataSource = dataSet.Tables[0];
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            string rowno = dataGridView1[0, e.RowIndex].Value.ToString();
            string name = dataGridView1[1, e.RowIndex].Value.ToString();
            string subject = dataGridView1[2, e.RowIndex].Value.ToString();
            string date = dataGridView1[3, e.RowIndex].Value.ToString();
            string contents = showContent(rowno,name,date);

            Form3 showForm = new Form3(name, subject, date, contents);
            showForm.Owner = this;
            showForm.Show();

           
        }

        private string showContent(string rowno, string name, string date)
        {
            string strConn = "Data Source=D306;Initial Catalog=MyTestDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            string sql = "select A.CONTENTS from (select ROW_NUMBER() over (ORDER BY ID DESC) as rowno, CONTENTS, USER_NAME from MyBBS) as A where A.rowno=@rowno and A.USER_NAME = @name";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            SqlParameter paramName = new SqlParameter("@name", SqlDbType.VarChar, 20);
            SqlParameter paramRowno = new SqlParameter("@rowno", SqlDbType.Int);

            paramRowno.Value = rowno;
            paramName.Value = name;

           

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramRowno);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["CONTENTS"].ToString();
            }
            return "";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    public class BBSControl
    {
        public DataSet GetContents(int pageno, int pagesize)
        {
            DataSet ds = new DataSet();
            string strConn = "Data Source=D306;Initial Catalog=MyTestDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            string sql
            = "select a.rownum, a.USER_NAME, a.SUBJECT, a.DATE from (select row_number() OVER (ORDER BY ID DESC) as rownum, USER_NAME, SUBJECT, DATE from MyBBS) as a where a.rownum between (((@pageno-1)*@pagesize)+1) and (@pageno * @pagesize)";
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
