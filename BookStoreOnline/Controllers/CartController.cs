using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Areas.Admin.Models;
using BookStoreOnline.Models;
using PayPal.Api;

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

            CartItem sanPham = myCart.FirstOrDefault(p => p.MaSanPham == id);
            if (sanPham == null)
            {
                sanPham = new CartItem(id);
                sanPham.SoLuong = quantity;
                myCart.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong += quantity;
            }
            return RedirectToAction("GetCartInfo", "Cart");
        }
        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.SoLuong);
            return totalNumber;
        }

        public ActionResult AddSingleProduct(int id)
        {
            List<CartItem> myCart = GetCart();


            int quantity = 1;

            CartItem sanPham = myCart.FirstOrDefault(p => p.MaSanPham == id);
            if (sanPham == null)
            {
                sanPham = new CartItem(id);
                sanPham.SoLuong = quantity;
                myCart.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong += quantity;
            }
            return RedirectToAction("GetCartInfo", "Cart");
        }

        public ActionResult Remove(int id)
        {
            List<CartItem> myCart = GetCart();
            CartItem Product = myCart.FirstOrDefault(p => p.MaSanPham == id);
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
            CartItem sanPham = myCart.FirstOrDefault(p => p.MaSanPham == id);
            sanPham.SoLuong = quantity;
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
                chiTietDonHang.MaSanPham = item.MaSanPham;
                chiTietDonHang.SoLuong = item.SoLuong;
                //prod.UnitPrice = item.FinalPrice();

                db.CHITIETDONHANGs.Add(chiTietDonHang);
                db.SaveChanges();
            }
            Session["GioHang"] = null;
            return RedirectToAction("Index/" + khachHangDaDangNhap.MaKH, "Order");
        }

        public ActionResult FailureView() 
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/cart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Giá sản phẩm",
                currency = "USD",
                price = "0",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0.4",
                subtotal = "22"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "22.4", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }


    }
}