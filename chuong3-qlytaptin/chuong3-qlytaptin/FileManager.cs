using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuong3_qlytaptin
{
    internal class FileManager
    {
        // Đường dẫn file lưu cùng thư mục với file chạy (.exe)
        private const string FilePath = "students.txt";
        private const char KytU_NganCach = '|';
        private const char KytU_MonHoc = ',';

        // 1. Hàm GHI danh sách sinh viên xuống file txt
        public static void LuuDanhSachSinhVien(List<Student> danhSachSV)
        {
            // Sử dụng StreamWriter với tham số append=false để ghi đè lại toàn bộ file mới
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                foreach (Student sv in danhSachSV)
                {
                    // Nối danh sách các môn học thành 1 chuỗi dài, cách nhau bởi dấu phẩy
                    string chuoiMonHoc = string.Join(KytU_MonHoc.ToString(), sv.MonHocDangKy);

                    // Ghép tất cả thuộc tính của sinh viên cách nhau bằng dấu '|'
                    // Lưu ý: Ép kiểu NgaySinh về chuẩn chuỗi "dd/MM/yyyy" để lúc đọc lên không bị lỗi
                    string line = $"{sv.MSSV}{KytU_NganCach}{sv.HoTenLot}{KytU_NganCach}{sv.Ten}{KytU_NganCach}" +
                                  $"{sv.NgaySinh.ToString("dd/MM/yyyy")}{KytU_NganCach}{sv.SoCMND}{KytU_NganCach}" +
                                  $"{sv.GioiTinh}{KytU_NganCach}{sv.Lop}{KytU_NganCach}{sv.SoDT}{KytU_NganCach}" +
                                  $"{sv.DiaChi}{KytU_NganCach}{chuoiMonHoc}";

                    // Ghi 1 dòng xuống file
                    sw.WriteLine(line);
                }
            }
        }

        // 2. Hàm ĐỌC danh sách sinh viên từ file txt
        public static List<Student> DocDanhSachSinhVien()
        {
            List<Student> danhSach = new List<Student>();

            // Nếu file chưa tồn tại (chạy lần đầu) thì trả về danh sách rỗng
            if (!File.Exists(FilePath))
                return danhSach;

            // Đọc toàn bộ các dòng trong file
            string[] lines = File.ReadAllLines(FilePath);

            foreach (string line in lines)
            {
                // Bỏ qua nếu dòng trống
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Cắt chuỗi dựa vào dấu '|'
                string[] parts = line.Split(KytU_NganCach);

                // Kiểm tra xem có đủ các trường thông tin cơ bản không (9 trường đầu)
                if (parts.Length >= 9)
                {
                    Student sv = new Student();
                    sv.MSSV = parts[0];
                    sv.HoTenLot = parts[1];
                    sv.Ten = parts[2];

                    // Parse lại ngày sinh từ chuỗi
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(parts[3], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        sv.NgaySinh = parsedDate;
                    }

                    sv.SoCMND = parts[4];
                    sv.GioiTinh = parts[5];
                    sv.Lop = parts[6];
                    sv.SoDT = parts[7];
                    sv.DiaChi = parts[8];

                    // Nếu có đăng ký môn học (trường số 10) thì tiến hành cắt bằng dấu phẩy
                    if (parts.Length == 10 && !string.IsNullOrWhiteSpace(parts[9]))
                    {
                        sv.MonHocDangKy = parts[9].Split(KytU_MonHoc).ToList();
                    }

                    // Thêm sinh viên vừa tạo vào danh sách
                    danhSach.Add(sv);
                }
            }
            return danhSach;
        }
    }
}

