using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    internal class QuanLySinhVien
    {
        public delegate int SoSanh(object obj1, object obj2);

        public List<SinhVien> dsSinhVien { get; private set; }

        public QuanLySinhVien()
        {
            dsSinhVien = new List<SinhVien>();
        }

        public SinhVien this[int index]
        {
            get { return dsSinhVien[index]; }
            set { dsSinhVien[index] = value; }
        }

        public void Them(SinhVien sv)
        {
            if (sv != null) dsSinhVien.Add(sv);
        }

        public void Xoa(object obj, SoSanh ss)
        {
            for (int i = dsSinhVien.Count - 1; i >= 0; i--)
            {
                if (ss(obj, dsSinhVien[i]) == 0)
                {
                    dsSinhVien.RemoveAt(i);
                }
            }
        }

        public SinhVien Tim(object obj, SoSanh ss)
        {
            foreach (SinhVien sv in dsSinhVien)
            {
                if (ss(obj, sv) == 0) return sv;
            }
            return null;
        }

        public bool Sua(SinhVien svSua, object obj, SoSanh ss)
        {
            for (int i = 0; i < dsSinhVien.Count; i++)
            {
                if (ss(obj, dsSinhVien[i]) == 0)
                {
                    dsSinhVien[i] = svSua;
                    return true;
                }
            }
            return false;
        }

        public void DocTuFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename)) return;

            foreach (string line in File.ReadAllLines(filename))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] data = line.Split('\t');
                if (data.Length < 8) continue;

                DateTime ngaySinh;
                if (!DateTime.TryParse(data[2], CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh) &&
                    !DateTime.TryParse(data[2], out ngaySinh))
                {
                    ngaySinh = DateTime.Today;
                }

                SinhVien sv = new SinhVien
                {
                    MaSo = data[0].Trim(),
                    HoTen = data[1].Trim(),
                    NgaySinh = ngaySinh,
                    DiaChi = data[3].Trim(),
                    Lop = data[4].Trim(),
                    Hinh = data[5].Trim(),
                    GioiTinh = data[6].Trim() == "1"
                };

                foreach (string chuyenNganh in data[7].Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(chuyenNganh))
                    {
                        sv.ChuyenNganh.Add(chuyenNganh.Trim());
                    }
                }
                Them(sv);
            }
        }
    }
}
