using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wa1.Models;

namespace Wa1.Controllers
{
    public class NganhHocController : Controller
    {
        private static readonly List<PhieuDangKy> dsPhieu = new List<PhieuDangKy>();
        // GET: NganhHoc
        public ActionResult DangKyChuyenNganh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LuuThongTin()
        {
            string hoTen = Request.Form["hoten"];
            string mssv = Request.Form["mssv"];
            string lop = Request.Form["lop"];
            string chuyenNganh = Request.Form["chuyennganh"];

            /*ViewBag.Hoten = hoTen;
            ViewBag.MSSV = mssv;
            ViewBag.Lop = lop;
            ViewBag.ChuyenNganh = chuyenNganh;*/
            var phieuDangKy = new PhieuDangKy()
            {
                HoTen = hoTen,
                ChuyenNganh = chuyenNganh,
                Lop = lop,
                MSSV = mssv,
            };

            dsPhieu.Add(phieuDangKy);
            var dem = dsPhieu.Count(x => x.ChuyenNganh == chuyenNganh);

            ViewBag.dem = dem;

            return View(phieuDangKy);
        }
    }
}