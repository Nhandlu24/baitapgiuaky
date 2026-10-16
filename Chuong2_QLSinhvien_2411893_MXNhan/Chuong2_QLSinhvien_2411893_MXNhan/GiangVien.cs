using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    internal class GiangVien
    {
        public DanhMucHocPhan dsHocPhan;
        public string GioiTinh;
        public string Mail;
        public string[] NgoaiNgu;
        public string SoDT;

        public string HoTen { get; set; }
        public string MaSo { get; set; }
        public DateTime NgaySinh { get; set; }

        public GiangVien()
        {
            dsHocPhan = new DanhMucHocPhan();
            NgoaiNgu = new string[10];
        }

        public GiangVien(string maso, string sdt, string hoten, string mail, string gioitinh, DateTime ngaysinh, string[] ngoaingu, DanhMucHocPhan ds)
        {
            this.MaSo = maso;
            this.SoDT = sdt;
            this.HoTen = hoten;
            this.Mail = mail;
            this.GioiTinh = gioitinh;
            this.NgaySinh = ngaysinh;
            this.NgoaiNgu = ngoaingu;
            this.dsHocPhan = ds;
        }

        public override string ToString()
        {
            string dsNgoaiNgu = string.Join(", ", NgoaiNgu);
            return $"Mã số: {MaSo}\nHọ tên: {HoTen}\nGiới tính: {GioiTinh}\nNgày sinh: {NgaySinh.ToShortDateString()}\nSố ĐT: {SoDT}\nMail: {Mail}\nNgoại ngữ: {dsNgoaiNgu}\nMôn dạy: {dsHocPhan.ToString()}";
        }
    }
}
