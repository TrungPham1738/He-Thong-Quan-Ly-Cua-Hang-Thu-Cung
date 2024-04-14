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
    public partial class FormThanhToan : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        MainForm main;
        public FormThanhToan(MainForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            main = form;
            getTransno();
            loadCash();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ThanhToanTC product = new ThanhToanTC(this);
            product.uname = main.lblUsername.Text;
            product.ShowDialog();

        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            ThanhToanKH customer = new ThanhToanKH(this);
            customer.ShowDialog();

            if(MessageBox.Show("Bạn có chắc chắn muốn mua không?", "Thanh toán", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                getTransno();
                main.loadDailySale();
                for (int i=0; i<dgvCash.Rows.Count; i++)
                {
                    dbcon.executeQuery("UPDATE tbThuCung SET Soluong= Soluong - " + int.Parse(dgvCash.Rows[i].Cells[4].Value.ToString()) + " WHERE idThucung LIKE " + dgvCash.Rows[i].Cells[2].Value.ToString() + "");
                }
                dgvCash.Rows.Clear();
            }
        }

       

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            removeitem:
            if(colName=="Delete")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này không?", "Xóa đơn hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.executeQuery("DELETE FROM tbThanhToan WHERE idThanhToan LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Đơn hàng đã được xóa thành công", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }               
            }
            else if (colName == "Increase")
            {
                int i = checkPqty(dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString());
                if(int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                {
                    dbcon.executeQuery("UPDATE tbThanhToan SET Soluong = Soluong + " + 1 + " WHERE idThanhToan LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                }
               else
                {
                    MessageBox.Show("Số lượng còn lại trong tay là " + i + "!", "Hết hàng ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "Decrease")
            {
                if(int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1)
                {
                    colName = "Delete";
                    goto removeitem;
                }
                dbcon.executeQuery("UPDATE tbThanhToan SET Soluong = Soluong - " + 1 + " WHERE idThanhToan LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
            }
            loadCash();
        }

        #region method
        public void getTransno()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 Sogiaodich FROM tbThanhToan WHERE Sogiaodich LIKE '" + sdate + "%' ORDER BY idThanhToan DESC", cn);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
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


        public void loadCash()
        {
            try
            {
                int i = 0;
                double total = 0;
                dgvCash.Rows.Clear();
                cm = new SqlCommand("SELECT idThanhToan,idThucung,Tenthucung,Soluong,Giatien,Tong, TT.idKH, NVTT FROM tbThanhToan as TT LEFT JOIN tbKhachHang KH ON TT.idKH = KH.idKH WHERE Sogiaodich LIKE " + lblTransno.Text + "", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    total += double.Parse(dr[5].ToString());
                }
                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        public  int checkPqty(string pcode)
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT Soluong FROM tbThuCung WHERE idThucung LIKE '" + pcode + "'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return i;
        }
        #endregion method
    }
}
