using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuong3_qlytaptin
{
    internal class Student
    {
        public string MSSV { get; set; }
        public string HoTenLot { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public string SoCMND { get; set; }
        public string GioiTinh { get; set; } // Hoặc dùng bool
        public string Lop { get; set; }
        public string SoDT { get; set; }
        public string DiaChi { get; set; }
        public List<string> MonHocDangKy { get; set; }

        public Student()
        {
            MonHocDangKy = new List<string>();
        }
    }
}
