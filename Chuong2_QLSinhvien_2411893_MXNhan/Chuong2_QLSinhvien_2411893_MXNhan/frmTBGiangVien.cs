using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics; // Dùng cho Process.Start
using System.Linq;
using System.Windows.Forms;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{

    public partial class frmGiangVien : Form
    {
        QuanLyGiangVien qlgv = new QuanLyGiangVien();
        public frmGiangVien()
        {
            InitializeComponent();
            rdNam.Checked = true;
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            foreach (object item in lbDanhSachHP.SelectedItems.Cast<object>().ToList())
            {
                if (!lbHocPhanDay.Items.Contains(item))
                {
                    lbHocPhanDay.Items.Add(item);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            foreach (object item in lbHocPhanDay.SelectedItems.Cast<object>().ToList())
            {
                lbHocPhanDay.Items.Remove(item);
            }
        }

        private void btnThongBao_Click(object sender, EventArgs e)
        {
            string maSo = cboMaSo.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSo))
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn mã số giảng viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaSo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên giảng viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            List<string> dsNgoaiNgu = chklbNgoaiNgu.CheckedItems
                .Cast<object>()
                .Select(item => item.ToString())
                .ToList();

            DanhMucHocPhan danhMucHocPhan = new DanhMucHocPhan();
            foreach (object item in lbHocPhanDay.Items)
            {
                danhMucHocPhan.Them(new HocPhan(item.ToString()));
            }

            string gioiTinh = rdNu.Checked ? "Nữ" : "Nam";
            GiangVien gv = new GiangVien(
                maSo,
                mtxtSoDT.Text.Trim(),
                hoTen,
                txtMail.Text.Trim(),
                gioiTinh,
                dtpNgaySinh.Value,
                dsNgoaiNgu.ToArray(),
                danhMucHocPhan);

            MessageBox.Show(gv.ToString(), "Thông tin giảng viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cboMaSo.SelectedIndex = -1;
            cboMaSo.Text = string.Empty;
            txtHoTen.Clear();
            rdNam.Checked = true;
            rdNu.Checked = false;
            mtxtSoDT.Clear();
            txtMail.Clear();
            dtpNgaySinh.Value = DateTime.Today;

            for (int i = 0; i < chklbNgoaiNgu.Items.Count; i++)
            {
                chklbNgoaiNgu.SetItemChecked(i, false);
            }

            lbDanhSachHP.ClearSelected();
            lbHocPhanDay.Items.Clear();
            cboMaSo.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linklbLienHe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://cntt.dlu.edu.vn/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở liên kết: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboMaSo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSo = cboMaSo.Text;
            string hoTen = txtHoTen.Text;
            DateTime ngaySinh = dtpNgaySinh.Value;
            string soDT = mtxtSoDT.Text;
            string mail = txtMail.Text;
            string gioiTinh = rdNam.Checked ? "Nam" : "Nữ";
            List<string> dsNgoaiNgu = new List<string>();
            for (int i = 0; i < chklbNgoaiNgu.Items.Count; i++)
            {
                if (chklbNgoaiNgu.GetItemChecked(i))
                    dsNgoaiNgu.Add(chklbNgoaiNgu.Items[i].ToString());
            }

            DanhMucHocPhan danhMucHocPhan = new DanhMucHocPhan();
            foreach (var item in lbHocPhanDay.Items)
            {
                danhMucHocPhan.Them(new HocPhan(item.ToString()));
            }

            // Khởi tạo đối tượng GiangVien
            GiangVien gv = new GiangVien(maSo, soDT, hoTen, mail, gioiTinh, ngaySinh, dsNgoaiNgu.ToArray(), danhMucHocPhan);

            // Gọi hàm thêm và kiểm tra kết quả
            bool ketQua = qlgv.Them(gv);

            if (!ketQua)
            {
                // Hiển thị thông báo lỗi biểu tượng dấu X màu đỏ nếu mã đã tồn tại
                MessageBox.Show("Mã giảng viên đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Thêm giảng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = HienThiHopThoaiTimKiem();
            if (string.IsNullOrWhiteSpace(tuKhoa)) return;

            GiangVien giangVien = qlgv.dsGiangVien.FirstOrDefault(gv =>
                string.Equals(gv.MaSo, tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrWhiteSpace(gv.HoTen) &&
                 gv.HoTen.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0));

            if (giangVien == null)
            {
                MessageBox.Show("Giảng viên không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HienThiGiangVien(giangVien);
        }

        private string HienThiHopThoaiTimKiem()
        {
            using (Form hopThoai = new Form())
            using (Label nhan = new Label())
            using (TextBox oNhap = new TextBox())
            using (Button btnDongY = new Button())
            using (Button btnHuy = new Button())
            {
                hopThoai.Text = "Tìm kiếm giảng viên";
                hopThoai.StartPosition = FormStartPosition.CenterParent;
                hopThoai.FormBorderStyle = FormBorderStyle.FixedDialog;
                hopThoai.MinimizeBox = false;
                hopThoai.MaximizeBox = false;
                hopThoai.ShowInTaskbar = false;
                hopThoai.ClientSize = new System.Drawing.Size(360, 125);

                nhan.AutoSize = true;
                nhan.Location = new System.Drawing.Point(12, 15);
                nhan.Text = "Hãy nhập mã số hoặc tên giảng viên:";

                oNhap.Location = new System.Drawing.Point(15, 42);
                oNhap.Size = new System.Drawing.Size(330, 20);

                btnDongY.DialogResult = DialogResult.OK;
                btnDongY.Location = new System.Drawing.Point(179, 78);
                btnDongY.Size = new System.Drawing.Size(78, 27);
                btnDongY.Text = "Tìm";

                btnHuy.DialogResult = DialogResult.Cancel;
                btnHuy.Location = new System.Drawing.Point(267, 78);
                btnHuy.Size = new System.Drawing.Size(78, 27);
                btnHuy.Text = "Hủy";

                hopThoai.Controls.Add(nhan);
                hopThoai.Controls.Add(oNhap);
                hopThoai.Controls.Add(btnDongY);
                hopThoai.Controls.Add(btnHuy);
                hopThoai.AcceptButton = btnDongY;
                hopThoai.CancelButton = btnHuy;

                return hopThoai.ShowDialog(this) == DialogResult.OK ? oNhap.Text.Trim() : string.Empty;
            }
        }

        private void HienThiGiangVien(GiangVien gv)
        {
            cboMaSo.Text = gv.MaSo ?? string.Empty;
            txtHoTen.Text = gv.HoTen ?? string.Empty;
            dtpNgaySinh.Value = gv.NgaySinh >= dtpNgaySinh.MinDate && gv.NgaySinh <= dtpNgaySinh.MaxDate
                ? gv.NgaySinh
                : DateTime.Today;
            rdNam.Checked = string.Equals(gv.GioiTinh, "Nam", StringComparison.OrdinalIgnoreCase);
            rdNu.Checked = !rdNam.Checked;
            mtxtSoDT.Text = gv.SoDT ?? string.Empty;
            txtMail.Text = gv.Mail ?? string.Empty;

            for (int i = 0; i < chklbNgoaiNgu.Items.Count; i++)
            {
                string ngoaiNgu = chklbNgoaiNgu.Items[i].ToString();
                bool daChon = gv.NgoaiNgu != null && gv.NgoaiNgu.Any(nn =>
                    string.Equals(nn, ngoaiNgu, StringComparison.OrdinalIgnoreCase));
                chklbNgoaiNgu.SetItemChecked(i, daChon);
            }

            lbHocPhanDay.Items.Clear();
            if (gv.dsHocPhan != null)
            {
                foreach (string hocPhan in gv.dsHocPhan.ToString().Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(hocPhan))
                    {
                        lbHocPhanDay.Items.Add(hocPhan.Trim());
                    }
                }
            }
        }
    }
}