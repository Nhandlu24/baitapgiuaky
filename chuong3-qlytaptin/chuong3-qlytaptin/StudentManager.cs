using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuong3_qlytaptin
{
    internal class StudentManager
    {
        public List<Student> DanhSachSV { get; set; }

        public StudentManager()
        {
            DanhSachSV = new List<Student>();
        }

        public void ThemSV(Student sv)
        {
            if (sv == null) return;
            DanhSachSV.Add(sv);
        }

        public void CapNhatSV(Student sv)
        {
            if (sv == null) return;

            Student sinhVienCu = DanhSachSV.FirstOrDefault(s => s.MSSV == sv.MSSV);
            if (sinhVienCu == null) return;

            int viTri = DanhSachSV.IndexOf(sinhVienCu);
            DanhSachSV[viTri] = sv;
        }

        public void XoaSV(List<string> dsMSSVCanXoa)
        {
            if (dsMSSVCanXoa == null) return;
            DanhSachSV.RemoveAll(s => dsMSSVCanXoa.Contains(s.MSSV));
        }

        public List<Student> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return new List<Student>(DanhSachSV);

            tuKhoa = tuKhoa.Trim();
            return DanhSachSV.Where(s =>
                (s.MSSV ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (s.HoTenLot ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (s.Ten ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (s.Lop ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }
}
