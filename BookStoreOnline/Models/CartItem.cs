using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreOnline.Models;

namespace BookStoreOnline.Models
{
    public class CartItem
    {
        NhaSachEntities db = new NhaSachEntities();
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string Anh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }


        public decimal FinalPrice()
        {
            return SoLuong * Gia;
        }
        public CartItem(int MaSanPham)
        {
            this.MaSanPham = MaSanPham;
            var productDB = db.SANPHAMs.Single(s => s.MaSanPham == this.MaSanPham);
            this.TenSanPham = productDB.TenSanPham;
            this.Anh = productDB.Anh;
            this.Gia = (decimal)productDB.Gia;
            this.SoLuong = 1;
        }
    }
}