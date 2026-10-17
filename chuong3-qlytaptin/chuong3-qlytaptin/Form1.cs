using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;    
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace chuong3_qlytaptin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void WriteReadText(string filename, string[] text)
        {
            // Ghi dữ liệu text vào file filename
            File.WriteAllLines(filename, text);

            // Đọc dữ liệu từ file filename hiển thị ra màn hình Console
            // (Lưu ý: Để thấy dòng Console này, bạn cần vào Project Properties -> Đổi Output type thành Console Application, hoặc ta có thể sửa lại in ra MessageBox cho dễ nhìn trong Windows Form)
            string output = "";
            foreach (string s in File.ReadAllLines(filename))
            {
                output += s + "\n";
            }
            MessageBox.Show(output, "Nội dung file Text");
        }

        // Gọi hàm khi bấm nút
        private void button1_Click(object sender, EventArgs e)
        {
            string[] mangChuoi = { "Xin chao", "Day la bai tap", "Thao tac tap tin" };
            WriteReadText("data.txt", mangChuoi);
        }
        private List<StudentInfo> LoadJSON(string path)
        {
            // Đọc toàn bộ nội dung file json
            string jsonContent = File.ReadAllText(path);
            // Parse chuỗi json thành JObject
            JObject jsonObject = JObject.Parse(jsonContent);
            // Trích xuất mảng "sinhvien" và chuyển đổi thành List<StudentInfo>
            List<StudentInfo> list = jsonObject["sinhvien"].ToObject<List<StudentInfo>>();
            return list;
        }

        private void btnReadJSON_Click(object sender, EventArgs e)
        {
            string Str = ""; // chuỗi lưu trữ
            string Path = "../../students.json"; // Đường dẫn tập tin

            // Gọi phương thức LoadJSON
            List<StudentInfo> List = LoadJSON(Path);

            for (int i = 0; i < List.Count; i++) // Đọc danh sách
            {
                StudentInfo info = List[i];
                Str += string.Format("Sinh viên {0} có MSSV: {1}, họ tên: {2}," +
                                     " điểm TB: {3}\r\n", (i + 1), info.MSSV, info.Hoten, info.Diem);
            }
            MessageBox.Show(Str);
        }
    }
}
