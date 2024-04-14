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
    public partial class TTTC : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "Pet Shop Management System";
        bool check = false;
        FormThuCung product;
        public TTTC(FormThuCung form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            product = form;
            cbCategory.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn đăng ký thú cưng này không?", "Đăng ký thú cưng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbThuCung(Tenthucung, Kieuloai, Loai, Soluong, Giatien)VALUES(@pname, @ptype, @pcategory, @pqty, @pprice)", cn);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@ptype", txttype.Text);
                        cm.Parameters.AddWithValue("@pcategory", cbCategory.Text);
                        cm.Parameters.AddWithValue("@pqty", int.Parse(txtQty.Text));
                        cm.Parameters.AddWithValue("@pprice", double.Parse(txtPrice.Text));
                        

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Thú cưng đã được đăng ký thành công!", title);
                        Clear();
                        product.LoadProduct();
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
                    if (MessageBox.Show("Bạn có chắc chắn muốn Chỉnh sửa thú cưng này không?", "Thú cưng đã được chỉnh sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbThuCung SET Tenthucung=@pname, Kieuloai=@ptype, Loai=@pcategory, Soluong=@pqty, Giatien=@pprice WHERE idThucung=@pcode", cn);
                        cm.Parameters.AddWithValue("@pcode", lblPcode.Text);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@ptype", txttype.Text);
                        cm.Parameters.AddWithValue("@pcategory", cbCategory.Text);
                        cm.Parameters.AddWithValue("@pqty", int.Parse(txtQty.Text));
                        cm.Parameters.AddWithValue("@pprice", double.Parse(txtPrice.Text));


                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Thú cưng đã được cập nhật thành công!", title);                        
                        product.LoadProduct();
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

       
        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number 
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number 
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if((e.KeyChar=='.')&& ((sender as TextBox).Text.IndexOf('.')>-1))
            {
                e.Handled = true;
            }

        }

        #region Method
        public void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            txttype.Clear();
            cbCategory.SelectedIndex = 0;

            btnUpdate.Enabled = false;
        }

        public void CheckField()
        {
            if (txtName.Text == "" | txtPrice.Text == "" | txtQty.Text == "" | txttype.Text == "")
            {
                MessageBox.Show("Trường dữ liệu bắt buộc!", "Cảnh báo");
                return;
            }
            check = true;
        }

        #endregion Method

    }
}
