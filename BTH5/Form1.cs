using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Linq;
using System.IO;

namespace BTH5
{
    public partial class Form1 : Form
    {
        string path = "../../Hinh";
        Table<SINHVIEN> BANG_SINHVIEN;
        Table<LOP> BANG_LOP;
        LinQDataContext db;
        BindingManagerBase DSSV;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            db = new LinQDataContext();
            BANG_SINHVIEN = db.SINHVIENs;
            BANG_LOP = db.GetTable<LOP>();

            loadCBOLop();
            loadDGVHocSinh();

            txtMaSV.DataBindings.Add("text", BANG_SINHVIEN, "MaSV", true);
            txtHoTen.DataBindings.Add("text", BANG_SINHVIEN, "HoTen", true);
            dateNgaySinh.DataBindings.Add("text", BANG_SINHVIEN, "NgaySinh", true);
            radNam.DataBindings.Add("checked", BANG_SINHVIEN, "GioiTinh", true);
            cboLop.DataBindings.Add("selectedvalue", BANG_SINHVIEN, "MaLop", true);
            txtQueQuan.DataBindings.Add("text", BANG_SINHVIEN, "DiaChi", true);
            pHinh.DataBindings.Add("ImageLocation", BANG_SINHVIEN, "Hinh", true);

            DSSV = this.BindingContext[BANG_SINHVIEN];
            enableNutLenh(false);
        }
        private void loadCBOLop()
        {
            cboLop.DataSource = BANG_LOP;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }
        private void loadDGVHocSinh()
        {
            dgvDSSV.AutoGenerateColumns = false;
            dgvDSSV.DataSource = BANG_SINHVIEN;
        }
        private void enableNutLenh(bool capnhap)
        {
            btnThem.Enabled = !capnhap;
            btnXoa.Enabled = !capnhap;
            btnSua.Enabled = !capnhap;
            btnThoat.Enabled = !capnhap;
            btnLuu.Enabled = capnhap;
            btnHuy.Enabled = capnhap;
        }
        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Files|*.jpg|PNG Files|*.png|All Files|*.*";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string fileName = openFileDialog1.SafeFileName;
                string pathFile = path + "/" + fileName;
                if (!File.Exists(pathFile))
                    File.Copy(openFileDialog1.FileName, pathFile);
                pHinh.ImageLocation = pathFile;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DSSV.AddNew();
            enableNutLenh(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                db.SubmitChanges();
                enableNutLenh(false);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DSSV.CancelCurrentEdit();
            ChangeSet cs = db.GetChangeSet();
            db.Refresh(RefreshMode.OverwriteCurrentValues, cs.Updates);
            enableNutLenh(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DSSV.RemoveAt(DSSV.Position);
            db.SubmitChanges();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            enableNutLenh(true);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DSSV.Position = BANG_SINHVIEN.ToList().FindIndex(sv => sv.MaSV.Contains(txtTimKiem.Text));
        }

        private void txtTimKiem_MouseDown(object sender, MouseEventArgs e)
        {
            txtTimKiem.Text = "";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
