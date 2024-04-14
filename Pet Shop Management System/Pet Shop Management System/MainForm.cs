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
    public partial class MainForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        
        public MainForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            btnDashboard.PerformClick();
            loadDailySale();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new TrangChu());
        }  

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new FormKH());
          
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (lblRole.Text == "Quản trị viên")
            {
                openChildForm(new FormNguoiDung());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new FormThuCung());
            
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new FormThanhToan(this));
            
        }

        private void btnLSGD_Click(object sender, EventArgs e)
        {
            openChildForm(new FormLSGD());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Đăng xuất ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                 LoginForm login = new LoginForm();
                 this.Dispose();
                 login.ShowDialog();
            }
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (progress.IsHandleCreated)
            {
                progress.Invoke(new Action(() =>
                {
                    progress.Text = DateTime.Now.ToString("hh:mm:ss");
                    progress.Value = DateTime.Now.Second;
                }));
            }
        }
        #region Method
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void loadDailySale()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(Tong),0) AS Tong FROM tbThanhToan WHERE Sogiaodich LIKE'" + sdate + "%'", cn);
                lblDailySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Method
    }
}
