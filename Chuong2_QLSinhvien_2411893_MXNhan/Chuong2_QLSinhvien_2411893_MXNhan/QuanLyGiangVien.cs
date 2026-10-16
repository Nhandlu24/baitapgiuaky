using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    internal class QuanLyGiangVien
    {
        public List<GiangVien> dsGiangVien;

        // Constructor
        public QuanLyGiangVien()
        {
            dsGiangVien = new List<GiangVien>();
        }

        // Indexer
        public GiangVien this[int index]
        {
            get { return dsGiangVien[index]; }
            set { dsGiangVien[index] = value; }
        }

        // Phương thức Tìm
        public GiangVien Tim(object temp, SoSanh ss)
        {
            foreach (GiangVien gv in dsGiangVien)
            {
                if (ss(gv, temp) == 0)
                    return gv;
            }
            return null;
        }

        // Phương thức Thêm (Kiểm tra trùng mã)
        public bool Them(GiangVien gv)
        {
            // Gọi hàm Tim để kiểm tra xem mã giảng viên đã tồn tại chưa
            GiangVien t = Tim(gv.MaSo, delegate (object obj1, object obj2)
            {
                return (obj1 as GiangVien).MaSo.CompareTo(obj2.ToString());
            });

            if (t != null) return false; // Nếu đã tồn tại thì trả về false

            dsGiangVien.Add(gv);
            return true;
        }

        // Phương thức Xóa
        public GiangVien Xoa(object temp, SoSanh ss)
        {
            GiangVien t = Tim(temp, ss);
            if (t != null)
            {
                dsGiangVien.Remove(t);
                return t;
            }
            return null;
        }

        // Phương thức Sắp xếp
        public void SapXep(SoSanh ss)
        {
            for (int i = 0; i < dsGiangVien.Count - 1; i++)
            {
                for (int j = i + 1; j < dsGiangVien.Count; j++)
                {
                    if (ss(dsGiangVien[i], dsGiangVien[j]) > 0)
                    {
                        GiangVien temp = dsGiangVien[i];
                        dsGiangVien[i] = dsGiangVien[j];
                        dsGiangVien[j] = temp;
                    }
                }
            }
        }
    }
}
