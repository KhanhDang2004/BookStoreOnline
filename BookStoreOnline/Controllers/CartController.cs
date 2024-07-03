using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class CartController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["GioHang"] as List<CartItem>;


            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["GioHang"] = myCart;
            }
            return myCart;
        }

        public ActionResult AddToCart(FormCollection product)
        {
            List<CartItem> myCart = GetCart();

            int id = int.Parse(product["ProductID"]);
            int quantity = int.Parse(product["Quantity"]);

            CartItem sanPham = myCart.FirstOrDefault(p => p.ProductID == id);
            if (sanPham == null)
            {
                sanPham = new CartItem(id);
                sanPham.Number = quantity;
                myCart.Add(sanPham);
            }
            else
            {
                sanPham.Number += quantity;
            }
            return RedirectToAction("GetCartInfo", "Cart");
        }
        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.Number);
            return totalNumber;
        }

        public ActionResult AddSingleProduct(int id)
        {
            List<CartItem> myCart = GetCart();


            int quantity = 1;

            CartItem sanPham = myCart.FirstOrDefault(p => p.ProductID == id);
            if (sanPham == null)
            {
                sanPham = new CartItem(id);
                sanPham.Number = quantity;
                myCart.Add(sanPham);
            }
            else
            {
                sanPham.Number += quantity;
            }
            return RedirectToAction("GetCartInfo", "Cart");
        }

        public ActionResult Remove(int id)
        {
            List<CartItem> myCart = GetCart();
            CartItem Product = myCart.FirstOrDefault(p => p.ProductID == id);
            myCart.Remove(Product);
            return RedirectToAction("GetCartInfo", "Cart");
        }

        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalPrice = myCart.Sum(sp => sp.FinalPrice());
            return totalPrice;
        }

        

        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();

            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("NullCart", "Cart");
            }
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart);
        }

        public ActionResult Update(FormCollection product)
        {
            int id = int.Parse(product["ProductID"]);
            int quantity = int.Parse(product["Quantity"]);

            List<CartItem> myCart = GetCart();
            CartItem sanPham = myCart.FirstOrDefault(p => p.ProductID == id);
            sanPham.Number = quantity;
            return RedirectToAction("GetCartInfo", "Cart");
        }

        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();

            return PartialView();
        }

        public ActionResult NullCart()
        {
            return View();
        }

        public ActionResult InsertOrder(string address)
        {
            DONHANG donHang = new DONHANG();
            var khachHangDaDangNhap = (KHACHHANG)Session["TaiKhoan"];
            donHang.ID = khachHangDaDangNhap.MaKH;
            donHang.NgayDat = DateTime.Now.Date;
            donHang.DiaChi = address;
            donHang.TrangThai = 0;
            db.DONHANGs.Add(donHang);
            db.SaveChanges();

            List<CartItem> cartItems = GetCart();
            foreach (var item in cartItems)
            {
                CHITIETDONHANG chiTietDonHang = new CHITIETDONHANG();
                chiTietDonHang.MaDonHang = donHang.MaDonHang;
                chiTietDonHang.MaSanPham = item.ProductID;
                chiTietDonHang.SoLuong = item.Number;
                //prod.UnitPrice = item.FinalPrice();

                db.CHITIETDONHANGs.Add(chiTietDonHang);
                db.SaveChanges();
            }
            Session["GioHang"] = null;
            return RedirectToAction("Index/" + khachHangDaDangNhap.MaKH, "Order");
        }



    }
}