using chuong3_qlytaptin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace chuong3_qlytaptin
{
    public partial class frmSinhVien : Form
    {
        private List<Student> dsSinhVien = new List<Student>();

        public frmSinhVien()
        {
            InitializeComponent();

            // Đăng ký các sự kiện cho Form và các Control
            this.Load += frmSinhVien_Load;
            this.FormClosing += frmSinhVien_FormClosing;
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            btnThoat.Click += BtnThoat_Click;
            dgvDanhSach.CellClick += DgvDanhSach_CellClick;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            // Tải danh sách từ tập tin txt khi vừa mở Form
            dsSinhVien = FileManager.DocDanhSachSinhVien();
            HienThiLenDataGrid(dsSinhVien);
            HienThiNoiDungFile();
        }

        private void HienThiLenDataGrid(List<Student> danhSach)
        {
            dgvDanhSach.Rows.Clear();
            foreach (var sv in danhSach)
            {
                dgvDanhSach.Rows.Add(sv.MSSV, sv.HoTenLot, sv.Ten, sv.NgaySinh.ToString("dd/MM/yyyy"),
                                     sv.Lop, sv.SoCMND, sv.SoDT, sv.DiaChi);
            }
        }

        private bool KiemTraHopLe(bool kiemTraTrungMSSV = true)
        {
            if (txtMSSV.Text == "" || txtTen.Text == "" || txtCMND.Text == "" || txtSoDT.Text == "" || cboLop.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo");
                return false;
            }

            // Kiểm tra độ dài
            if (txtMSSV.Text.Length != 7)
            {
                MessageBox.Show("MSSV phải gồm đúng 7 chữ số!", "Cảnh báo");
                return false;
            }
            if (txtCMND.Text.Length != 9) { MessageBox.Show("CMND phải 9 số!"); return false; }
            if (txtSoDT.Text.Length != 10) { MessageBox.Show("Số ĐT phải 10 số!"); return false; }

            // Kiểm tra định dạng MSSV theo Lớp (AABBCCC)
            string AA = cboLop.Text.Substring(0, 2); // Ví dụ lớp '20DTH1' -> AA = '20'
            if (txtMSSV.Text.Substring(0, 2) != AA || txtMSSV.Text.Substring(2, 2) != "10")
            {
                MessageBox.Show($"MSSV không hợp lệ. Phải có định dạng {AA}10xxx", "Cảnh báo");
                return false;
            }

            // Kiểm tra trùng MSSV
            if (kiemTraTrungMSSV && dsSinhVien.Any(s => s.MSSV == txtMSSV.Text))
            {
                MessageBox.Show("MSSV đã tồn tại trong danh sách!", "Cảnh báo");
                return false;
            }

            return true;
        }

        private Student TaoSinhVienTuForm()
        {
            Student sv = new Student
            {
                MSSV = txtMSSV.Text,
                HoTenLot = txtHoTenLot.Text,
                Ten = txtTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                SoCMND = txtCMND.Text,
                SoDT = txtSoDT.Text,
                DiaChi = txtDiaChi.Text,
                Lop = cboLop.Text,
                GioiTinh = radNam.Checked ? "Nam" : "Nữ"
            };

            foreach (var item in clbMonHoc.CheckedItems)
            {
                sv.MonHocDangKy.Add(item.ToString());
            }
            return sv;
        }

        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            if (!KiemTraHopLe(kiemTraTrungMSSV: true)) return;

            Student svMoi = TaoSinhVienTuForm();
            dsSinhVien.Add(svMoi);

            FileManager.LuuDanhSachSinhVien(dsSinhVien);
            HienThiLenDataGrid(dsSinhVien);
            HienThiNoiDungFile();

            MessageBox.Show("Thêm mới thành công và đã lưu vào file!", "Thông báo");
        }

        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (!KiemTraHopLe(kiemTraTrungMSSV: false)) return;

            Student svCanSua = dsSinhVien.FirstOrDefault(s => s.MSSV == txtMSSV.Text);
            if (svCanSua != null)
            {
                int index = dsSinhVien.IndexOf(svCanSua);
                dsSinhVien[index] = TaoSinhVienTuForm();

                FileManager.LuuDanhSachSinhVien(dsSinhVien);
                HienThiLenDataGrid(dsSinhVien);
                HienThiNoiDungFile();
                MessageBox.Show("Cập nhật thành công và đã lưu vào file!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Không tìm thấy MSSV này để cập nhật!", "Lỗi");
            }
        }

        private void DgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dsSinhVien.Count)
            {
                string mssvChon = dgvDanhSach.Rows[e.RowIndex].Cells[0].Value.ToString();
                Student sv = dsSinhVien.FirstOrDefault(s => s.MSSV == mssvChon);

                if (sv != null)
                {
                    txtMSSV.Text = sv.MSSV;
                    txtHoTenLot.Text = sv.HoTenLot;
                    txtTen.Text = sv.Ten;
                    dtpNgaySinh.Value = sv.NgaySinh;
                    txtCMND.Text = sv.SoCMND;
                    txtSoDT.Text = sv.SoDT;
                    txtDiaChi.Text = sv.DiaChi;
                    cboLop.Text = sv.Lop;

                    if (sv.GioiTinh == "Nam") radNam.Checked = true; else radNu.Checked = true;

                    for (int i = 0; i < clbMonHoc.Items.Count; i++)
                    {
                        clbMonHoc.SetItemChecked(i, sv.MonHocDangKy.Contains(clbMonHoc.Items[i].ToString()));
                    }
                }
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = HienThiHopThoaiTimKiem();
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                var kq = dsSinhVien.Where(s => s.MSSV.Contains(tuKhoa) || s.Ten.Contains(tuKhoa) || s.Lop.Contains(tuKhoa)).ToList();
                HienThiLenDataGrid(kq);
            }
            else
            {
                HienThiLenDataGrid(dsSinhVien);
            }
        }

        private string HienThiHopThoaiTimKiem()
        {
            using (Form hopThoai = new Form())
            using (TextBox oNhap = new TextBox())
            using (Button oDongY = new Button())
            using (Button oHuy = new Button())
            {
                hopThoai.Text = "Tìm kiếm";
                hopThoai.ClientSize = new Size(340, 105);
                hopThoai.FormBorderStyle = FormBorderStyle.FixedDialog;
                hopThoai.StartPosition = FormStartPosition.CenterParent;
                hopThoai.MinimizeBox = false;
                hopThoai.MaximizeBox = false;

                oNhap.Location = new Point(15, 15);
                oNhap.Size = new Size(310, 22);
                oDongY.Text = "Đồng ý";
                oDongY.DialogResult = DialogResult.OK;
                oDongY.Location = new Point(160, 55);
                oHuy.Text = "Hủy";
                oHuy.DialogResult = DialogResult.Cancel;
                oHuy.Location = new Point(250, 55);

                hopThoai.Controls.Add(oNhap);
                hopThoai.Controls.Add(oDongY);
                hopThoai.Controls.Add(oHuy);
                hopThoai.AcceptButton = oDongY;
                hopThoai.CancelButton = oHuy;

                return hopThoai.ShowDialog(this) == DialogResult.OK ? oNhap.Text.Trim() : string.Empty;
            }
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDocFile_Click(object sender, EventArgs e)
        {
            dsSinhVien = FileManager.DocDanhSachSinhVien();
            HienThiLenDataGrid(dsSinhVien);
            HienThiNoiDungFile();
        }

        private void HienThiNoiDungFile()
        {
            string duongDanFile = Path.Combine(Application.StartupPath, "students.txt");
            
        }

        private void frmSinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtNoiDungFile_TextChanged(object sender, EventArgs e)
        {
                    }
    }
}