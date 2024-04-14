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

namespace Pet_Shop_Management_System
{
    public partial class TTND : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "Pet Shop Management System";

        bool check = false;
        FormNguoiDung userForm;
        public TTND(FormNguoiDung user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            userForm = user;
            cbRole.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn đăng ký người dùng này không?", "Đăng ký người dùng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbNguoiDung(Tennguoidung,Diachi,Sdt,Vaitro,Ngaysinh,Matkhau)VALUES(@name,@address,@phone,@role,@dob,@password)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Người dùng đã đăng ký thành công!", title);
                        Clear();
                        userForm.LoadUser();
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
                    if (MessageBox.Show("Bạn có chắc chắn muốn cập nhật bản ghi này không?", "Chỉnh sửa bản ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbNguoiDung SET Tennguoidung=@name, Diachi=@address, Sdt=@phone, Vaitro=@role, Ngaysinh=@dob, Matkhau=@password WHERE idNguoiDung=@id", cn);
                        cm.Parameters.AddWithValue("@id", lbluid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Dữ liệu của người dùng đã được cập nhật thành công!", title);
                        Clear();
                        userForm.LoadUser();
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


        #region Method

        public void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            cbRole.SelectedIndex = 0;
            dtDob.Value = DateTime.Now;

            btnUpdate.Enabled = false;
        }

        //to check field and date of birth
        public void CheckField()
        {
            if(txtName.Text == ""| txtAddress.Text=="")
            {
                MessageBox.Show("Trường dữ liệu bắt buộc!", "Cảnh báo");
                return;
            }
            
            if(checkAGe(dtDob.Value) < 18)
            {
                MessageBox.Show("Người dùng là trẻ em!. Dưới 18 tuổi", "Cảnh báo");
                return;
            }
            check = true;
        }

        // to Calculate Age for under 18 
        private static int checkAGe(DateTime dateofBirth)
        {
            int age = DateTime.Now.Year - dateofBirth.Year;
            if (DateTime.Now.DayOfYear < dateofBirth.DayOfYear)
                age = age - 1;
            return age;
        }
        #endregion Method
    }
}
