﻿using System;
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
    public partial class FormThuCung : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public FormThuCung()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TTTC module = new TTTC(this);
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if(colName =="Edit")
            {
                TTTC module = new TTTC(this);
                module.lblPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txttype.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.cbCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtQty.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if(colName=="Delete")
            {
                if(MessageBox.Show("Bạn có chắc chắn muốn xóa các mục này không?", "Xóa bản ghi", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    dbcon.executeQuery("DELETE FROM tbThuCung WHERE idThucung LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");                    
                    MessageBox.Show("Bản ghi mục đã được xóa thành công!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            LoadProduct();
        }

        #region Method

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbThuCung WHERE CONCAT(Tenthucung,Kieuloai,Loai) LIKE N'%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion Method
    }
}
