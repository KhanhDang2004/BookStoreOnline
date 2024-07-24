using BookStoreOnline.Models;
using BookStoreOnline.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            DateTime today = DateTime.Now;
            DateTime sevenDaysAgo = today.AddDays(-7);

            // Lấy danh sách đơn hàng trong 7 ngày gần nhất
            var lstDonHang = db.DONHANGs
                .Where(dh => dh.NgayDat.HasValue && dh.NgayDat.Value >= sevenDaysAgo)
                .ToList();

            // Tính doanh thu từng ngày
            var dailyRevenue = lstDonHang
                .GroupBy(dh => dh.NgayDat.Value.Date) // Sử dụng Value.Date
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(dh => dh.TongTien)
                })
                .OrderBy(d => d.Date)
                .ToList();

            // Tạo danh sách dữ liệu cho biểu đồ
            List<ChartData> chartData = dailyRevenue.Select(dr => new ChartData
            {
                Label = dr.Date.ToString("dd/MM/yyyy"), // Định dạng ngày theo yêu cầu
                Value = dr.Revenue
            }).ToList();
            ViewBag.ChartData = chartData;

            return View();
        }
    }
}