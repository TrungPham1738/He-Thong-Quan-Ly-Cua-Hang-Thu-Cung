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
    public partial class ThanhToanTC : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public string uname;
        FormThanhToan cash;
        public ThanhToanTC(FormThanhToan form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadProduct();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow dr in dgvProduct.Rows)
            {
                bool chkbox = Convert.ToBoolean(dr.Cells["Select"].Value);
                if(chkbox)
                {
                    try
                    {
                        cm = new SqlCommand("INSERT INTO tbThanhToan(Sogiaodich, idThucung, Tenthucung, Soluong, Giatien, NVTT) VALUES (@transno, @pcode, @pname, @qty, @price, @cashier)", cn);
                        cm.Parameters.AddWithValue("@transno",cash.lblTransno.Text);
                        cm.Parameters.AddWithValue("@pcode",dr.Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@pname", dr.Cells[2].Value.ToString());
                        cm.Parameters.AddWithValue("@qty",1);
                        cm.Parameters.AddWithValue("@price", Convert.ToDouble( dr.Cells[5].Value.ToString()));
                        cm.Parameters.AddWithValue("@cashier",uname);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message,title);
                    }
                }
            }

            cash.loadCash();
            this.Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        #region Method

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT idThucung, Tenthucung, Kieuloai, Loai, Giatien FROM tbThuCung WHERE CONCAT(Tenthucung,Kieuloai,Loai) LIKE N'%" + txtSearch.Text + "%' AND Soluong > "+0+"", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion Method
    }
}
