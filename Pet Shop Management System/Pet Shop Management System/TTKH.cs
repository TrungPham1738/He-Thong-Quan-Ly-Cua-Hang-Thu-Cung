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
    public partial class TTKH : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "Pet Shop Management System";

        bool check = false;
        FormKH customer;
        public TTKH(FormKH form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            customer = form;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn đăng ký khách hàng này không?", "Đăng ký khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbKhachHang(TenKH,Diachi,Sdt)VALUES(@name,@address,@phone)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Khách hàng đã đăng ký thành công!", title);
                        Clear();
                        customer.LoadCustomer();
                    }

                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn Chỉnh sửa bản ghi này không?", "Ghi lại chỉnh sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbKhachHang SET TenKH=@name, Diachi=@address, Sdt=@phone WHERE idKH=@id", cn);
                        cm.Parameters.AddWithValue("@id", lblcid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Dữ liệu khách hàng đã được cập nhật thành công!", title);
                        Clear();
                        customer.LoadCustomer();
                        this.Dispose();
                    }

                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #region method
        public void CheckField()
        {
            if (txtName.Text == "" | txtAddress.Text == "" | txtPhone.Text=="")
            {
                MessageBox.Show("Trường dữ liệu bắt buộc!", "Cảnh báo");
                return;
            }
     
            check = true;
        }

        public void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        #endregion method
    }
}
