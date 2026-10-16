using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    public partial class frmSinhVien : Form
    {
        private QuanLySinhVien qlsv;
        private object txtHinh;

        public frmSinhVien()
        {
            InitializeComponent();
            qlsv = new QuanLySinhVien();
            Load += frmSinhVien_Load;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DanhSachSV.txt");
            qlsv.DocTuFile(filePath);
            LoadListView();
            DatMacDinh();
        }

        private void LoadListView()
        {
            lvSinhVien.Items.Clear();
            foreach (SinhVien sv in qlsv.dsSinhVien)
            {
                ThemSVVaoListView(sv);
            }
        }

        private void ThemSVVaoListView(SinhVien sv)
        {
            ListViewItem item = new ListViewItem(sv.MaSo ?? string.Empty);
            item.SubItems.Add(sv.HoTen ?? string.Empty);
            item.SubItems.Add(sv.NgaySinh.ToString("dd/MM/yyyy"));
            item.SubItems.Add(sv.DiaChi ?? string.Empty);
            item.SubItems.Add(sv.Lop ?? string.Empty);
            item.SubItems.Add(sv.GioiTinh ? "Nam" : "Nữ");
            item.SubItems.Add(string.Join(", ", sv.ChuyenNganh ?? new List<string>()));
            item.SubItems.Add(sv.Hinh ?? string.Empty);
            lvSinhVien.Items.Add(item);
        }

        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien
            {
                MaSo = mtxtMaSo.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value.Date,
                DiaChi = txtDiaChi.Text.Trim(),
                Lop = cboLop.Text.Trim(),
                Hinh = txtHinh.Text.Trim(),
                GioiTinh = rdNam.Checked
            };

            for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
            {
                if (clbChuyenNganh.GetItemChecked(i))
                {
                    sv.ChuyenNganh.Add(clbChuyenNganh.Items[i].ToString());
                }
            }

            return sv;
        }

        private bool DuLieuHopLe(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSo))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtxtMaSo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(sv.HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên sinh viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(sv.Lop))
            {
                MessageBox.Show("Vui lòng nhập lớp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLop.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            if (!DuLieuHopLe(sv)) return;

            SinhVien daTonTai = qlsv.Tim(sv.MaSo, delegate (object maSo, object sinhVien)
            {
                return string.Compare(maSo.ToString(), ((SinhVien)sinhVien).MaSo, StringComparison.OrdinalIgnoreCase);
            });

            if (daTonTai != null)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            qlsv.Them(sv);
            LoadListView();
            MessageBox.Show("Đã thêm sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lvSinhVien.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng đánh dấu sinh viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> maSoCanXoa = lvSinhVien.CheckedItems.Cast<ListViewItem>()
                .Select(item => item.SubItems[0].Text)
                .ToList();

            foreach (string maSo in maSoCanXoa)
            {
                qlsv.Xoa(maSo, delegate (object ma, object sinhVien)
                {
                    return string.Compare(ma.ToString(), ((SinhVien)sinhVien).MaSo, StringComparison.OrdinalIgnoreCase);
                });
            }

            LoadListView();
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            DatMacDinh();
        }

        private void DatMacDinh()
        {
            mtxtMaSo.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            txtDiaChi.Clear();
            cboLop.SelectedIndex = -1;
            cboLop.Text = string.Empty;
            txtHinh.Clear();
            rdNam.Checked = true;
            rdNu.Checked = false;

            for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
            {
                clbChuyenNganh.SetItemChecked(i, false);
            }

            if (pbHinh.Image != null)
            {
                Image oldImage = pbHinh.Image;
                pbHinh.Image = null;
                oldImage.Dispose();
            }

            mtxtMaSo.Focus();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ChonHinh();
        }

        private void pbHinh_Click(object sender, EventArgs e)
        {
            ChonHinh();
        }

        private void ChonHinh()
        {
            if (OpenFileDialogHinh.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (Image image = Image.FromFile(OpenFileDialogHinh.FileName))
                {
                    Image imagePreview = new Bitmap(image);
                    Image oldImage = pbHinh.Image;
                    pbHinh.Image = imagePreview;
                    if (oldImage != null) oldImage.Dispose();
                }

                string fileName = OpenFileDialogHinh.FileName;
                txtHinh.Text = fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở hình ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblDiaChi_Click(object sender, EventArgs e)
        {

        }
    }
}
