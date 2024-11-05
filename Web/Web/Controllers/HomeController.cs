using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        K22CNT2_HOANGKIMOANH_2210900053Entities db = new K22CNT2_HOANGKIMOANH_2210900053Entities();

        public ActionResult Index()
        {
            return View(db.SAN_PHAM.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.SAN_PHAM.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Thông tin";
            return View();
        }

        public ActionResult Giohang(int? id)
        {
            var cart = Session["Cart"] as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return View(cart);
        }

        public ActionResult dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult dangnhap(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var khachhang = db.KHACH_HANG.FirstOrDefault(u => u.Tai_khoan == username && u.Mat_khau == password);
                if (khachhang != null)
                {
                    Session["UserId"] = khachhang.MaKH;
                    Session["UserName"] = khachhang.Tai_khoan;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangky(KHACH_HANG khachhang)
        {
            ViewBag.Message = "Đăng ký.";
            if (ModelState.IsValid)
            {
                var existingUser = db.KHACH_HANG.FirstOrDefault(u => u.Tai_khoan == khachhang.Tai_khoan);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Tài khoản đã tồn tại. Vui lòng chọn tên khác.");
                    return View(khachhang);
                }

                db.KHACH_HANG.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("dangnhap");
            }
            return View(khachhang);
        }

        public ActionResult DangXuat()
        {
            Session.Remove("UserId");
            Session.Remove("UserName");

            TempData["LogoutMessage"] = "Bạn đã đăng xuất thành công.";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddToCart(int id)
        {
            var product = db.SAN_PHAM.Find(id);
            if (product != null)
            {
                var cartItem = new CartItem
                {
                    MaSP = product.MaSP,
                    TenSP = product.TenSP,
                    Gia = product.Gia,
                    Soluong = 1
                };
                CartManager.AddToCart(cartItem);
            }
            return RedirectToAction("Giohang");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            var cart = Session["Cart"] as List<CartItem>;

            if (cart != null)
            {
                var itemToRemove = cart.FirstOrDefault(c => c.MaSP == productId);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                }

                Session["Cart"] = cart;
            }

            return RedirectToAction("Giohang");
        }

        [HttpPost]
        public ActionResult PlaceOrder(string customerName, string customerPhone)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Giohang");
            }

            var order = new DON_HANG
            {
                Ngaylap = DateTime.Now,
                Tongtien = cart.Sum(item => item.Gia * item.Soluong)
            };

            if (Session["UserId"] != null)
            {
                order.MaKH = (int)Session["UserId"];
            }

            db.DON_HANG.Add(order);
            db.SaveChanges();

            foreach (var item in cart)
            {
                var orderDetail = new CHI_TIET_DON_HANG
                {
                    MaDH = order.MaDH,
                    MaSP = item.MaSP,
                    So_luong = item.Soluong,
                    Tong_tien = item.Gia * item.Soluong
                };
                db.CHI_TIET_DON_HANG.Add(orderDetail);
            }
            db.SaveChanges();

            Session["Cart"] = null;

            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Index");
        }
    }
}
