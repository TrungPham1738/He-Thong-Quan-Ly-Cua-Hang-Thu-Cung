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

namespace Pet_Shop_Management_System
{
    public partial class ThanhToanKH : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        FormThanhToan cash;
        public ThanhToanKH(FormThanhToan form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if(colName == "Choice")
            {
                dbcon.executeQuery("UPDATE tbThanhToan SET idKH=" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE Sogiaodich=" + cash.lblTransno.Text + "");
                cash.loadCash();
                this.Dispose();
            }

        }

        #region method
        public void LoadCustomer()
        {
            
            try
            {
                int i = 0;
                dgvCustomer.Rows.Clear();
                cm = new SqlCommand("SELECT idKH,TenKH,Sdt FROM tbKhachHang WHERE Tenkh LIKE N'%" + txtSearch.Text + "%'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method
    }
}
