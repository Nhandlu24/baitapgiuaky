using System;
using System.Globalization;
using System.Windows.Forms;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    public partial class frmTrungTam : Form
    {
        private const int TienTinHocA = 300000;
        private const int TienTinHocB = 500000;
        private const int TienTiengAnhA = 400000;
        private const int TienTiengAnhB = 600000;

        private static readonly CultureInfo VietnameseCulture =
            CultureInfo.GetCultureInfo("vi-VN");

        public frmTrungTam()
        {
            InitializeComponent();
            HienThiHocPhi();
            reset();
        }

        private void HienThiHocPhi()
        {
            lblTienTHA.Text = DinhDangTien(TienTinHocA);
            lblTienTHB.Text = DinhDangTien(TienTinHocB);
            lblTienTAA.Text = DinhDangTien(TienTiengAnhA);
            lblTienTAB.Text = DinhDangTien(TienTiengAnhB);
        }

        private static string DinhDangTien(int soTien)
        {
            return soTien.ToString("N0", VietnameseCulture) + " đồng";
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void ib1TienTAA_Click(object sender, EventArgs e)
        {
        }

        private void ChkTinHocB_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            int tongTien = 0;

            if (chkTinHocA.Checked)
            {
                tongTien += TienTinHocA;
            }

            if (ChkTinHocB.Checked)
            {
                tongTien += TienTinHocB;
            }

            if (chkTiengAnhA.Checked)
            {
                tongTien += TienTiengAnhA;
            }

            if (chkTiengAnhB.Checked)
            {
                tongTien += TienTiengAnhB;
            }

            txtTongTien.Text = DinhDangTien(tongTien);
        }

        private void reset()
        {
            cboMAHV.SelectedIndex = -1;
            cboMAHV.Text = string.Empty;
            txtHoTen.Clear();
            dtpNgayDangKy.Value = DateTime.Today;
            rdNam.Checked = true;
            rdNu.Checked = false;
            chkTiengAnhA.Checked = false;
            chkTiengAnhB.Checked = false;
            chkTinHocA.Checked = false;
            ChkTinHocB.Checked = false;
            txtTongTien.Clear();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
