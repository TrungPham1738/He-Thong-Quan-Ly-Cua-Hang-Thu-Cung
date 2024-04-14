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
    public partial class FormLSGD : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public FormLSGD()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadLS();
        }

        private void dgvLS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadLS();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLS();
        }

        #region method
        public void LoadLS()
        {
            dgvLS.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbThanhToan WHERE CONCAT(idThucung, idKH, NVTT) LIKE N'%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dgvLS.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion method
    }
}
