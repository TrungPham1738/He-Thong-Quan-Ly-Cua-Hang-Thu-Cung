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
    public partial class FormKH : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public FormKH()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadCustomer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TTKH module = new TTKH(this);
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                TTKH module = new TTKH(this);
                module.lblcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
       
                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa hồ sơ khách hàng này không?", "Xóa bản ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.executeQuery("DELETE FROM tbKhachHang WHERE idKH LIKE'" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Dữ liệu khách hàng đã được xóa thành công", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }

            LoadCustomer();
        }

        #region method
        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbKhachHang WHERE CONCAT(TenKH,Diachi,Sdt) LIKE N'%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion method
    }
}
