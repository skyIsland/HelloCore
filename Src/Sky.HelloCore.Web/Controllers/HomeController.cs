using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sky.Gallery;
using Sky.HelloCore.Web.Models;

namespace Sky.HelloCore.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 关于我
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Title"] = "关于我";
            ViewData["Message"] = "克莱登大学南极洲文学史副教授.";

            return View();
        }

        /// <summary>
        /// 联系我
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Title"] = "联系方式";
            ViewData["Message"] = "不告诉你呀";

            return View();
        }

        /// <summary>
        /// 画廊
        /// </summary>
        /// <returns></returns>
        public IActionResult Gallery()
        {
            var factory = new DouBan();
            ViewBag.List = factory.GetListBelle(null,4);
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
