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
    public partial class FormNguoiDung : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public FormNguoiDung()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadUser();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TTND module = new TTND(this);
            module.ShowDialog();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if(colName=="Edit")
            {
                TTND module = new TTND(this);
                module.lbluid.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.dtDob.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if(colName=="Delete")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này không?", "Xóa bản ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.executeQuery("DELETE FROM tbNguoiDung WHERE idNguoiDung LIKE '" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Dữ liệu người dùng đã được xóa thành công", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }  
            }

            LoadUser();
        }

        #region Method
        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbNguoiDung WHERE CONCAT(Tennguoidung,Diachi,Sdt,Ngaysinh,Vaitro) LIKE N'%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        #endregion Method
    }
}
