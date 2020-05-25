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

namespace Contacts
{
    public partial class Form1 : Form
    {
        readonly SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\DB\DBContacts.mdf;Integrated Security=True;Connect Timeout=30");
        int ContactID = 0; 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reset();
            FillDataGridView();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                     SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                     sqlCmd.CommandType = CommandType.StoredProcedure;
                     sqlCmd.Parameters.AddWithValue("@mode", "Add");
                     sqlCmd.Parameters.AddWithValue("@ContactID", 0);
                     sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                     sqlCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                     sqlCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNum.Text.Trim());
                     sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                     sqlCmd.ExecuteNonQuery();
                     MessageBox.Show("Saved Successfully");
                    
                }
                else
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNum.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully");
                }
                reset();
                FillDataGridView();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
        }
        void FillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewOrSearch",sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@ContactName", txtSearch.Text.Trim());
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv.DataSource = dtbl;
                dgv.Columns[0].Visible = false;
                sqlCon.Close();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            if(dgv.CurrentRow.Index != -1)
            {
                ContactID = Convert.ToInt32(dgv.CurrentRow.Cells[0].Value.ToString());
                txtName.Text = dgv.CurrentRow.Cells[1].Value.ToString();
                txtMobileNum.Text = dgv.CurrentRow.Cells[2].Value.ToString();
                txtEmail.Text = dgv.CurrentRow.Cells[3].Value.ToString();
                txtAddress.Text = dgv.CurrentRow.Cells[4].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        void reset()
        {
            txtName.Text = txtMobileNum.Text = txtAddress.Text = txtEmail.Text = "";
            btnSave.Text = "Save";
            ContactID = 0;
            btnDelete.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                    
                SqlCommand sqlCmd = new SqlCommand("ContactDelete", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");
                reset();
                FillDataGridView();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}