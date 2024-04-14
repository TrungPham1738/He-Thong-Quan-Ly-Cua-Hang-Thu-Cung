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
    public partial class LoginForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public LoginForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string _name = "", _role = "";
                cn.Open();
                cm = new SqlCommand("SELECT Tennguoidung,Vaitro FROM tbNguoiDung WHERE Tennguoidung=@name and Matkhau=@password", cn);
                cm.Parameters.AddWithValue("@name", txtname.Text);
                cm.Parameters.AddWithValue("@password", txtpass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    _name = dr["Tennguoidung"].ToString();
                    _role = dr["Vaitro"].ToString();
                    MessageBox.Show("Chào mừng  " + _name + " |", "QUYỀN TRUY CẬP ĐƯỢC CẤP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    main.lblUsername.Text = _name;
                    main.lblRole.Text = _role;
                    if (_role == "Quản trị viên")
                        main.btnUser.Enabled = true;
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tên người dùng và mật khẩu không hợp lệ!", "TRUY CẬP BỊ TỪ CHỐI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }

        }

        private void btnForget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ với SẾP của bạn!", "QUÊN MẬT KHẨU", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
