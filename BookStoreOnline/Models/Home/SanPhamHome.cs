using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreOnline.Models.Home
{
    public class SanPhamHome
    {
        public List<SANPHAM> Book { get; set; }
        public List<SANPHAM> NewsBook { get; set; }
        public List<SANPHAM> BookHot { get; set; }
    }
}