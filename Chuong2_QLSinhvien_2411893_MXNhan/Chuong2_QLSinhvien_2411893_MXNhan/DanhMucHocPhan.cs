using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuong2_QLSinhvien_2411893_MXNhan
{
    internal class DanhMucHocPhan
    {
        private List<HocPhan> ds;

        public DanhMucHocPhan()
        {
            ds = new List<HocPhan>();
        }

        // Indexer
        public HocPhan this[int index]
        {
            get { return ds[index]; }
            set { ds[index] = value; }
        }

        public void Them(HocPhan hp)
        {
            ds.Add(hp);
        }

        public override string ToString()
        {
            string result = "";
            foreach (HocPhan hp in ds)
            {
                result += hp.TenHP + "; ";
            }
            return result;
        }
    }
}

