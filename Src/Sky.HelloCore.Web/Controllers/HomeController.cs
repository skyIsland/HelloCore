using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sky.Gallery;
using Sky.HelloCore.Web.Models;

namespace Sky.HelloCore.Web.Controllers
{
    public class HomeController : Controller
    {
        public static string Key = "shadameng";
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
            ViewData["Message"] = "ismatch/丹麦的面包不单卖:克莱登大学南极洲文学史副教授.";

            return View();
        }

        /// <summary>
        /// 联系我
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Title"] = "联系方式";
            ViewData["Message"] = "其实我不想告诉你呀";

            return View();
        }

        /// <summary>
        /// 画廊
        /// </summary>
        /// <returns></returns>
        public IActionResult Gallery(int cid = 4,int page = 0)
        {
            var factory = new DouBan();
            string requestUrl = null;
            if (page > 0) requestUrl = $"https://www.dbmeinv.com/dbgroup/show.htm?cid={cid}&pager_offset={page}";
            ViewBag.List = factory.GetListBelle(requestUrl, cid);
            return View();
        }

        /// <summary>
        /// 音悦台 todo:前台有表单提交改成异步获取，添加分页
        /// </summary>
        /// <returns></returns>
        public IActionResult Music(Pager pager)
        {
            var listMusic = new List<Sky.Models.KwMusic>();
            if (string.IsNullOrEmpty(pager.MusicName)) pager.MusicName = "稻香";
            if (!string.IsNullOrEmpty(pager.MusicName))
            {
                try
                {
                    listMusic = new KwMusicAnalysis.Domain().SearchMusic(pager.MusicName, pager.PageNo);
                }
                catch (Exception e)
                {

                }
            }

            if (listMusic.Count > 0)
            {
                listMusic = DesEncryptUrl(listMusic);
            }
            return View(listMusic);
        }

        /// <summary>
        /// 根据加密的url下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="musicName"></param>
        /// <param name="singerName"></param>
        /// <returns></returns>
        public IActionResult GetFile(string url, string musicName, string singerName)
        {
            var deUrl = Common.CEntrypt.Decode(url, Key);
            byte[] fileStream;
            using (var http = new WebClient())
            {
                fileStream = http.DownloadData(deUrl);
            }

            return File(fileStream, "application/octet-stream", $"{musicName}-{singerName}.mp3");
        }

        public IActionResult StoryDownload()
        {
            return View();
        }
        /// <summary>
        /// 加密下载链接
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Sky.Models.KwMusic> DesEncryptUrl(List<Sky.Models.KwMusic> list)
        {
            var result = new List<Sky.Models.KwMusic>();
            var des = new DESCryptoServiceProvider();
            list.ForEach(p =>
            {
                result.Add(new Sky.Models.KwMusic()
                {
                    AlbumName = p.AlbumName,
                    DlUrl = Common.CEntrypt.Encode(p.DlUrl, Key),
                    SingerName = p.SingerName,
                    Name = p.Name
                });
            });

            return result;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
